namespace DTO;
public class PurchaseDTO
{
    //Atributos
    public DateTime dataPurchase;
    public int paymentType;
    public int purchaseStatus;
    public double purchaseValues;
    public String numberConfirmation;
    public String numberNf;
    public List<ProductDTO> products = new List<ProductDTO>();
}