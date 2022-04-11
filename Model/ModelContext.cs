namespace Model;
using Microsoft.EntityFrameworkCore;

public class ModelContext : DbContext
{
    public DbSet<Address> Address { get; set; }
    public DbSet<Client> Client { get; set; }
    public DbSet<Owner> Owner { get; set; }
    public DbSet<Person> Person { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Purchase> Purchase { get; set; }
    public DbSet<Stocks> Stocks { get; set; }
    public DbSet<Store> Store { get; set; }
    public DbSet<WishList> WishList { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source = JVLPC0571;" + "Initial Catalog = db_projetoCSharp; Integrated Security=True");
    }
}