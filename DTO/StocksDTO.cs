namespace DTO;
public class StocksDTO
{
    //Atributos
    public int quantity;
    public Double unitPrice;
    public StoreDTO store; //Dependência
    public ProductDTO product; //Dependência
}