using Microsoft.EntityFrameworkCore;
using Interfaces;
using DAO;
using DTO;
namespace Model;
public class Purchase : IValidateDataObject, IDataController<PurchaseDTO, Purchase>
{
    //Atributos
    private DateTime data_purchase;
    private int payment_type;
    private int purchase_status;
    public double purchase_values;
    private String number_confirmation;
    private String number_nf;
    private List<Product> products = new List<Product>();
    private Client client; //Dependência
    private Store store; //Dependência
    public List<PurchaseDTO>purchaseDTO = new List<PurchaseDTO>();

    //Construtor
    public Purchase(){}
    private Purchase (Client client)
    {
        this.client=client;
    }
    //Métodos
    public static Purchase convertDTOToModel(PurchaseDTO obj)
    {
        var purchase=new Purchase(Client.convertDTOToModel(obj.client));

        purchase.payment_type = obj.payment_type;
        purchase.purchase_status = obj.purchase_status;
        purchase.data_purchase=obj.data_purchase;
        purchase.purchase_values=obj.purchase_values;
        purchase.number_confirmation=obj.number_confirmation;
        purchase.number_nf=obj.number_nf;
        List<Product> produtos = new List<Product>();
        foreach (var prod in obj.products){
            produtos.Add(Product.convertDTOToModel(prod));
        }
        purchase.products = produtos;
        purchase.store = Store.convertDTOToModel(obj.store);

        return purchase;
    }

    public Boolean validateObject()
    {
        if(this.getDataPurchase() == null) { return false;}
        if(this.getPurchaseValues() == 0.0) { return false;}
        if(this.getNumberConfirmation() == null) { return false;}
        if(this.getNumberNf() == null) { return false; }
        return true;
    }

    public void delete(PurchaseDTO obj)
    {

    }
    public int save(int clientID, int storeID, int productID)
    {
        var id = 0;
        using(var context = new DaoContext())
        {

            var clientDAO = context.Client.Where(c => c.id == clientID).Single();
            var storeDAO = context.Store.Where(c => c.id == storeID).Single();
            var productsDAO = context.Product.Where(c => c.id == productID).Single();

            var purchaseDAO = new DAO.Purchase{
                client = clientDAO,
                store = storeDAO,
                products = productsDAO,
                data_purchase = this.data_purchase,
                purchase_values= this.purchase_values,
                purchase_status = this.purchase_status,
                payment_type = this.payment_type,
                number_confirmation = this.number_confirmation,
                number_nf = this.number_nf
            };

            context.Entry(purchaseDAO.client).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            context.Entry(purchaseDAO.store).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            context.Entry(purchaseDAO.products).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            context.Purchase.Add(purchaseDAO);
            context.SaveChanges();
            id = purchaseDAO.id;
        }
        return id;
    }
    public void update(PurchaseDTO obj)
    {

    }


    public PurchaseDTO findById(int id)
    {

        return new PurchaseDTO();
    }

    public List<PurchaseDTO> getAll()
    {        
        return this.purchaseDTO;      
    }

    public static List<object> getAllPurchases(int clientId){
        List<object> compras = new List<object>();

        using(var context = new DaoContext()){
            var purchases = context.Purchase.Include(c => c.client).Include(s => s.store).Include(p => p.products).Where(c => c.client.id == clientId).ToList();
            foreach(var purchase in purchases){
                compras.Add(new{
                    id = purchase.id,
                    productId = purchase.products.id,
                    productName = purchase.products.name,
                    productImg = purchase.products.image,
                    storeId = purchase.store.id,
                    storeName = purchase.store.name,
                    clientId = purchase.client.id,
                    clientName = purchase.client.name,
                    purchaseDate = purchase.data_purchase.ToString("dd/MM/yyyy"),
                    purchaseAmount = purchase.purchase_values
                });
            }
        }
        return compras;
    }
    public static List<object> getAllSales(int storeId){
        List<object> vendas = new List<object>();

        using(var context = new DaoContext()){
            var sales = context.Purchase.Include(c => c.client).Include(s => s.store).Include(p => p.products).Where(c => c.store.id == storeId).ToList();
            foreach(var sale in sales){
                vendas.Add(new{
                    id = sale.id,
                    productId = sale.products.id,
                    productName = sale.products.name,
                    productImg = sale.products.image,
                    storeId = sale.store.id,
                    storeName = sale.store.name,
                    clientId = sale.client.id,
                    clientName = sale.client.name,
                    purchaseDate = sale.data_purchase.ToString("dd/MM/yyyy"),
                    purchaseAmount = sale.purchase_values
                });
            }
        }
        return vendas;
    }
    public static object getPurchaseDetail(int clientID, int productID, int storeID){
        using(var context = new DaoContext()){
            var compra = context.Purchase.Include(c => c.client).Include(p => p.products).Include(s => s.store)
            .Where(cp => cp.client.id == clientID && cp.products.id == productID && cp.store.id == storeID).Single();

            return new{
                id = compra.id,
                purchaseData = compra.data_purchase.ToString("dd/MM/yyyy"),
                purchaseAmount = compra.purchase_values,
                purchaseNF = compra.number_nf,
                purchaseNC = compra.number_confirmation,
                purchasePayment = compra.payment_type,
                storeName = compra.store.name,
                productImg = compra.products.image,
                productName = compra.products.name,
                clientName = compra.client.name,
                clientDocument = compra.client.document,
                clientPhone = compra.client.phone
            };
        }
    }
   
    public PurchaseDTO convertModelToDTO()
    {
        var purchaseDTO = new PurchaseDTO();

        purchaseDTO.data_purchase = this.data_purchase;

        purchaseDTO.payment_type = this.payment_type;

        purchaseDTO.purchase_values = this.purchase_values;

        purchaseDTO.purchase_status = this.purchase_status;

        purchaseDTO.number_confirmation = this.number_confirmation;

        purchaseDTO.number_nf = this.number_nf;

        return purchaseDTO;
    }
    public void updateStatus(int PurchaseStatusEnum)
    {
        this.purchase_status = PurchaseStatusEnum;
    }
    public static List<object> findByDocument(String doc)
    {
        using(var context = new DaoContext())
        {
            var purchaseInstance = context.Purchase.Include(c=>c.client).Include(c=> c.client.address).Include(c=>c.products).Include(c=>c.store).Include(c=>c.store.owner).Include(c=>c.store.owner.address).Where(c => c.client.document == doc);
            
            List<object> purchases = new List<object>();

            foreach(object purchase in purchaseInstance){
                purchases.Add(purchase);
            }
            return purchases;
        }
    }
    public static object findByCNPJ(String cnpj)
    {
        using(var context = new DaoContext())
        {
            var purchaseInstance = context.Purchase.Include(c=>c.store).Include(c=>c.store.owner).Include(c=>c.store.owner.address).Include(c=>c.client).Include(c=> c.client.address).Include(c=>c.products).Where(c => c.store.CNPJ == cnpj).Single();
            return purchaseInstance;
        }
    }


    //GETs e SETs
    public Store getStore(){return this.store;}
    public void setStore(Store store){this.store=store;}
    public Client getClient(){return this.client;}
    public void setClient(Client client){this.client=client;}
    public List<Product> getProducts(){return this.products;}
    public void setProducts(List<Product>products){this.products=products;}
    public DateTime getDataPurchase(){return this.data_purchase;}
    public void setDataPurchase(DateTime data_purchase){this.data_purchase=data_purchase;}
    public int getPaymentType(){return this.payment_type;}
    public void setPaymentType(int payment_type){this.payment_type=payment_type;}
    public String getNumberConfirmation(){return this.number_confirmation;}
    public void setNumberConfirmation(String number_confirmation){this.number_confirmation=number_confirmation;}
    public String getNumberNf(){return this.number_nf;}
    public void setNumberNf(String number_nf){this.number_nf=number_nf;}
    public int getPurchaseStatus() => purchase_status;
    public void setPurchaseStatus(int purchase_status){this.purchase_status=purchase_status;}
    public double getPurchaseValues() => purchase_values;
    public void setPurchaseValues(double purchase_values){this.purchase_values=purchase_values;}
}
