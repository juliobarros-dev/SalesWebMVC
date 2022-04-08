#nullable disable
namespace SalesWebMVC.Models
{
  public class Department
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

    public Department()
    {
    }

    public Department(int id, string name)
    {
      Id = id;
      Name = name;
    }

    public void AddSeller(Seller seller)
    {
      Sellers.Add(seller);
    }

    public double TotalSales(DateTime initialDate, DateTime finalDate)
    {
      double totalSales = Sellers.Sum(seller => seller.TotalSales(initialDate, finalDate));

      return totalSales;
    }
  }
}
