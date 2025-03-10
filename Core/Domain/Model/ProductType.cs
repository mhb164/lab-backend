namespace Backend.Model;

public class ProductType
{
    public int Id { get; set; }
    public required bool Activation { get; set; }  
    public required string Name { get; set; }
}
