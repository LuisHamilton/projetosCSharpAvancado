namespace DTO;
using Enums;
public class PurchaseDTO
{
    //Atributos
    public DateTime data_purchase;
    public int payment_type;
    public int purchase_status;
    public Double purchase_values;
    public String number_confirmation;
    public String number_nf;
    public ClientDTO client = new ClientDTO();
    public StoreDTO store = new StoreDTO();
    public List<ProductDTO> products = new List<ProductDTO>();
}
