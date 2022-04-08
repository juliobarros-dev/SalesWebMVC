#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SalesWebMVC.Models
{
  public class Seller
  {
    public int Id { get; set; }
    [Required(ErrorMessage = "Mensagem")]
    [MinLength(3)]
    [MaxLength(50)]
    public string Name { get; set; }

    public string Email { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime BirthDate { get; set; }
    [DataType(DataType.Currency)]
    public double BaseSalary { get; set; }
    public Department Department { get; set; }
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
