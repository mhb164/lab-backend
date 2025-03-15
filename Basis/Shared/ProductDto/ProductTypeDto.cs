namespace Laboratory.Backend.Dto;

public class ProductTypeDto
{
    public ProductTypeDto() { /* Method intentionally left empty.*/ }

    public ProductTypeDto(int id, bool activation, string name)
    {
        Id = id;
        Activation = activation;
        Name = name;
    }

    public int Id { get; set; }
    public bool Activation { get; set; }
    public string Name { get; set; }

    public ServiceResult Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            return ServiceResult.BadRequest($"{nameof(Name)} is required!");

        return ServiceResult.Success();
    }
}
