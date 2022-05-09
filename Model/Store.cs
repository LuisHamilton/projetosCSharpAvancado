using Microsoft.EntityFrameworkCore;
using Interfaces;
using DAO;
using DTO;
namespace Model;
public class Store : IValidateDataObject, IDataController<StoreDTO, Store>
{
    //Atributos
    private String name;
    private String CNPJ;
    private Owner owner; //Dependência
    private List<Purchase> purchases=new List<Purchase>();
    public List<StoreDTO>storeDTO = new List<StoreDTO>();

    //Construtor
    public Store(Owner owner)
    {
        this.owner = owner;
    }
    //Métodos

    private Store(){

    }
    public static Store convertDTOToModel(StoreDTO obj)
    {
        //Owner.convertDTOToModel(obj.owner)
        var store = new Store();
        
        store.setName(obj.name);
        store.setCNPJ(obj.CNPJ);
        foreach(var purch in obj.purchases)
        {
            store.addNewPurchase(Purchase.convertDTOToModel(purch));
        }
        return store;
    }

    public void delete(StoreDTO obj)
    {

    }
    public int save(int owner)
    {       
        var id = 0;

        using(var context = new DaoContext())
        {
            var ownerDAO = context.Owner.Where(c => c.id == owner).Single();

            var storeDAO = new DAO.Store{
                name = this.name,
                CNPJ= this.CNPJ,
                owner = ownerDAO
            };

            context.Store.Add(storeDAO);
            context.Entry(storeDAO.owner).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            context.SaveChanges();

            id = storeDAO.id;

        }
         return id;
    }
    public void update(StoreDTO obj)
    {

    }
    public StoreDTO findById(int id)
    {
        return new StoreDTO();
    }
    public static object findByCNPJ(String cnpj)
    {
        using(var context = new DaoContext())
        {
            var storeInstance = context.Store.Include(s => s.owner).Include(s => s.owner.address).Where(s => s.CNPJ == cnpj).Single();
            return storeInstance;
        }
    }
    public int findId(){
        using(var context = new DaoContext())
        {
            var storeDAO = context.Store.Where(c => c.CNPJ == this.CNPJ).Single();
            return storeDAO.id;
        }
    }
    public List<StoreDTO> getAll()
    {        
        return this.storeDTO;      
    }
    public static List<object> getAllStores()
    {
        List<object> stores = new List<object>();

        using(var context = new DaoContext())
        {
            var lojas = context.Store.Where(p => true);
            foreach(var loja in lojas)
            {
                stores.Add(loja);
            }
            return stores;
        }
    }
    public StoreDTO convertModelToDTO()
    {
        var storeDTO = new StoreDTO();

        storeDTO.name = this.name;
        storeDTO.CNPJ = this.CNPJ;
        storeDTO.owner = this.owner.convertModelToDTO();
        foreach(var purch in purchases)
        {
            storeDTO.purchases.Add(purch.convertModelToDTO());
        }
        return storeDTO;
    }
    public Boolean validateObject()
    {
        if(this.getName()==null){return false;}
        if(this.getCNPJ() == null) { return false; }

        return true;
    }  
    public List<Purchase> getPurchases(){return purchases;}
    public void addNewPurchase(Purchase purchase)
    {
         if (!getPurchases().Contains(purchase))
         {
             this.purchases.Add(purchase);
         }
    }
    public Owner getOwner(){return owner;}
    public void setOwner(Owner owner){this.owner=owner;}
    public String getName(){return name;}
    public void setName(String name){this.name=name;}
    public String getCNPJ(){return CNPJ;}
    public void setCNPJ(String CNPJ){this.CNPJ=CNPJ;}


}
