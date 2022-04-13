﻿using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;
using System.Diagnostics;

namespace SalesWebMVC.Controllers
{
  public class SellersController : Controller
  {
    private readonly SellerService _sellerService;
    private readonly DepartmentService _departmentService;

    public SellersController(SellerService sellerService, DepartmentService departmentService)
    {
      _sellerService = sellerService;
      _departmentService = departmentService;
    }

    public IActionResult Index()
    {
      var sellersList = _sellerService.FindAll();
      return View(sellersList);
    }

    public IActionResult Create()
    {
      var departments = _departmentService.FindAll();
      var viewModel = new SellerFormViewModel() { Departments = departments };
      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Seller seller)
    {
      _sellerService.Insert(seller);
      return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int? id)
    {
      if (id == null) return RedirectToAction(nameof(Error), new { message = "Id not provided" });

      var seller = _sellerService.FindById(id.Value);

      if (seller == null) return RedirectToAction(nameof(Error), new { message = "Id not found" });

      return View(seller);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
      _sellerService.Remove(id);
      return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
      if(id == null) return RedirectToAction(nameof(Error), new { message = "Id not provided" });

      var seller = _sellerService.FindById(id);
      
      if(seller == null) return RedirectToAction(nameof(Error), new { message = "Id not found" });

      return View(seller);
    }

    public IActionResult Edit(int? id)
    {
      if (id == null) return RedirectToAction(nameof(Error), new { message = "Id not provided" });

      var seller = _sellerService.FindById(id.Value);

      if (seller == null) return RedirectToAction(nameof(Error), new { message = "Id not found" });

      List<Department> departments = _departmentService.FindAll();
      SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Seller seller)
    {
      if (id != seller.Id) return RedirectToAction(nameof(Error), new { message = "Id mismatch" });

      try
      {
        _sellerService.Update(seller);
        return RedirectToAction(nameof(Index));
      }
      catch (ApplicationException ex)
      {
        return RedirectToAction(nameof(Error), new { message = ex.Message });
      }
    }

    public IActionResult Error(string message)
    {
      var viewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };

      return View(viewModel);
    }
  }
}
