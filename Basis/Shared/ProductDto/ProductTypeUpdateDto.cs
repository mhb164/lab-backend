namespace Laboratory.Backend.Dto;

public class ProductTypeUpdateDto
{
    public ProductTypeUpdateDto() { /* Method intentionally left empty.*/ }

    public ProductTypeUpdateDto(bool activation, string name)
    {
        Activation = activation;
        Name = name;
    }

    public bool Activation { get; set; }
    public string Name { get; set; }

    public ServiceResult Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            return ServiceResult.BadRequest($"{nameof(Name)} is required!");

        return ServiceResult.Success();
    }
}
