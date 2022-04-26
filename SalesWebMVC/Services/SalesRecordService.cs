using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;

namespace SalesWebMVC.Services
{
  public class SalesRecordService
  {
    private readonly SalesWebMVCContext _context;

    public SalesRecordService(SalesWebMVCContext context) { _context = context; }

    public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
    {
      var result = from obj in _context.SalesRecord select obj;

      if (minDate.HasValue) { result = result.Where(x => x.Date >= minDate.Value); }
      if (maxDate.HasValue) { result = result.Where(x => x.Date <= maxDate.Value); }

      return await result
        .Include(x => x.Seller)
        .Include(x => x.Seller.Department)
        .OrderBy(x => x.Date).ThenBy(x => x.Amount)
        .ToListAsync();
    }

    public async Task<Lookup<string, SalesRecord>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
    {
      var result = await _context.SalesRecord
        .Where(x => x.Date >= minDate.Value && x.Date <= maxDate.Value)
        .Include(x => x.Seller)
        .Include(x => x.Seller.Department)
        .OrderBy(x => x.Date)
        .ToListAsync();

      Lookup<string, SalesRecord> lookup = (Lookup<string, SalesRecord>)result.ToLookup(p => p.Seller.Department.Name, p => p);

      return lookup;
    }
    /// <summary>
    /// abc
    /// </summary>
    /// <param name="lookup"></param>
    /// <param name="mindate"></param>
    /// <param name="maxDate"></param>
    /// <returns></returns>
    public Dictionary<string, double> TotalSalesByDepartment(Lookup<string, SalesRecord> lookup, DateTime? mindate, DateTime? maxDate)
    {
      double accumulator = 0.00;
      HashSet<string> previusSellersName = new HashSet<string>();

      Dictionary<string, double> result = new Dictionary<string,double>();

      List<string> keys = new List<string>();

      foreach (var item in lookup)
      {
        keys.Add(item.Key);
      }

      foreach (var key in keys)
      {
        foreach(var sel in lookup[key])
        {
          if (!previusSellersName.Contains(sel.Seller.Name))
          {
            accumulator += sel.Seller.TotalSales(mindate.Value, maxDate.Value);
            result[key] = accumulator;
          }
          previusSellersName.Add(sel.Seller.Name);
        }
        accumulator = 0.00;
      }

      return result;
    }
  }
}


