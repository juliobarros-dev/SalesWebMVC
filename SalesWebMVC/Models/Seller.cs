#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SalesWebMVC.Models
{
  public class Seller
  {
    public int Id { get; set; }
    [Required(ErrorMessage = "{0} required.")]
    public string Name { get; set; }
    [Display(Name = "E-mail")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "{0} required.")]
    public string Email { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Birth Date")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    [Required(ErrorMessage = "{0} required.")]
    public DateTime BirthDate { get; set; }
    [DataType(DataType.Currency)]
    [Display(Name = "Base Salary")]
    [Required(ErrorMessage = "{0} required.")]
    public double BaseSalary { get; set; }
    public Department Department { get; set; }
    [Required(ErrorMessage = "{0} required.")]
    public int DepartmentId { get; set; }
    public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

    public Seller()
    {
    }

    public Seller(int id, string name, string email, DateTime birthDate, double baseSalery, Department department)
    {
      Id = id;
      Name = name;
      Email = email;
      BirthDate = birthDate;
      BaseSalary = baseSalery;
      Department = department;
    }

    public void AddSale(SalesRecord sale)
    {
      Sales.Add(sale);
    }

    public void RemoveSale(SalesRecord sale)
    {
      Sales.Remove(sale);
    }

    public double TotalSales(DateTime initialDate, DateTime finalDate)
    {
      double totalSales = Sales
        .Where(sale => sale.Date >= initialDate && sale.Date <= finalDate)
        .Sum(sale => sale.Amount);

      return totalSales;
    }
  }
}
