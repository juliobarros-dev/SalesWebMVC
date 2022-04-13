using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Services.Exceptions;

namespace SalesWebMVC.Services
{
  public class SellerService
  {
    private readonly SalesWebMVCContext _context;

    public SellerService(SalesWebMVCContext context)
    {
      _context = context;
    }

    public List<Seller> FindAll()
    {
      return _context.Seller
        .Include(x => x.Department)
        .OrderBy(x => x.Name).ThenBy(x => x.Department.Name)
        .ToList();
    }

    public Seller FindById(int id)
    {
      return _context.Seller.Include(x => x.Department).FirstOrDefault(x => x.Id == id);
    }

    public void Remove(int id)
    {
      var seller = FindById(id);
      _context.Seller.Remove(seller);
      _context.SaveChanges();
    }

    public void Insert(Seller seller)
    {
      _context.Add(seller);
      _context.SaveChanges();
    }

    public void Update(Seller seller)
    {
      if(!_context.Seller.Any(x => x.Id == seller.Id))
      {
        throw new NotFoundException("Id not found");
      }
      try
      {
        _context.Update(seller);
        _context.SaveChanges();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        throw new DbConcurrencyException(ex.Message);
      }
    }
  }
}
