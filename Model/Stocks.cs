using Enums;
using Interfaces;
using DAO;
using DTO;
namespace Model;
public class Stocks : IValidateDataObject, IDataController<StocksDTO, Stocks>
{
    //Atributos
    private int quantity;
    private Double unit_price;
    private Store store; //Dependência
    private Product product; //Dependência
    public List<StocksDTO>stocksDTO = new List<StocksDTO>();

    //Construtor
    public Stocks(){}
    private Stocks (Store store, Product product)
    {
        this.store = store;
        this.product = product;
    }

    //Métodos
    public static Stocks convertDTOToModel(StocksDTO obj)
    {
        var stocks=new Stocks(Store.convertDTOToModel(obj.store), Product.convertDTOToModel(obj.product));
        stocks.setQuantity(obj.quantity);
        stocks.setUnitPrice(obj.unit_price);
        return stocks;
    }
    public Boolean validateObject()
    {
        if(this.getQuantity()==0){return false;}
        if(this.getUnitPrice()==0.0){return false;}
        if(this.getStore() == null) { return false; }
        if(this.getProduct() == null) { return false; }
        return true;
    }  
    public void delete(StocksDTO obj)
    {

    }
    public int save(int storeID, int productID, int quantity, double unit_price)
    {
        var id = 0;

        using(var context = new DaoContext())
        {
            var storeDAO = context.Store.FirstOrDefault(s => s.id == storeID);
            var productDAO = context.Product.FirstOrDefault(p => p.id == productID);
            var stocksDAO = new DAO.Stocks{
                quantity = quantity,
                unit_price= unit_price,
                product = productDAO,
                store = storeDAO
            };
            context.Stocks.Add(stocksDAO);
            context.SaveChanges();
            id = stocksDAO.id;
        }
         return id;
    }
    public void update(StocksDTO obj)
    {

    }
    public StocksDTO findById(int id)
    {

        return new StocksDTO();
    }
    public List<StocksDTO> getAll()
    {        
        return this.stocksDTO;      
    }
    public StocksDTO convertModelToDTO()
    {
        var stocksDTO = new StocksDTO();

        stocksDTO.quantity = this.quantity;

        stocksDTO.unit_price = this.unit_price;

        return stocksDTO;
    }

    //GETs e SETs
    public Store getStore(){return store;}
    public void setStore(Store store){this.store=store;}
    public Product getProduct(){return product;}
    public void setProduct(Product product){this.product=product;}
    public int getQuantity(){return quantity;}
    public void setQuantity(int quantity){this.quantity=quantity;}
    public double getUnitPrice(){return unit_price;}
    public void setUnitPrice(double unit_price){this.unit_price=unit_price;}

}