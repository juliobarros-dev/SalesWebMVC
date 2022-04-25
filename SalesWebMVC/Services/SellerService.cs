﻿using Microsoft.EntityFrameworkCore;
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

    public async Task<List<Seller>> FindAllAsync()
    {
      return await _context.Seller
        .Include(x => x.Department)
        .OrderBy(x => x.Name).ThenBy(x => x.Department.Name)
        .ToListAsync();
    }

    public async Task<Seller> FindByIdAsync(int id)
    {
      return await _context.Seller.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task RemoveAsync(int id)
    {
      var seller = await FindByIdAsync(id);
      _context.Seller.Remove(seller);
      await _context.SaveChangesAsync();
    }

    public async Task InsertAsync(Seller seller)
    {
      _context.Add(seller);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Seller seller)
    {
      bool hasAny = await _context.Seller.AnyAsync(x => x.Id == seller.Id);
      if (!hasAny)
      {
        throw new NotFoundException("Id not found");
      }
      try
      {
        _context.Update(seller);
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException ex)
      {
        throw new DbConcurrencyException(ex.Message);
      }
    }
  }
}
