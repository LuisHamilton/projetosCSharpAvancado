namespace DAO;
public class Stocks
{
    //Atributos
    public int id;
    public int quantity;
    public Double unit_price;
    public Store store; //DependĂȘncia
    public Product product; //DependĂȘncia
    public Stocks stocks;

}