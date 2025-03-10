namespace Backend.Dto;

public class ProductTypeDto
{
    public int Id { get; set; }
    public required bool Activation { get; set; }  
    public required string Name { get; set; }
}
