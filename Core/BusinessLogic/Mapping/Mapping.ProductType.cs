﻿using Backend.Dto;
using Backend.Model;

namespace Backend;

public static partial class MappingExtensions
{
    public static ProductTypeDto ToDto(this ProductType modelItem)
        => new ProductTypeDto
        {
            Id = modelItem.Id,
            Activation = modelItem.Activation,
            Name = modelItem.Name,
        };

    public static IEnumerable<ProductTypeDto> ToDto(this IEnumerable<ProductType> modelItems)
    {
        foreach (var modelItem in modelItems)
            yield return ToDto(modelItem);
    }

    public static ProductType ToModel(this ProductTypeDto dto)
        => new ProductType
        {
            Id = dto.Id,
            Activation = dto.Activation,
            Name = dto.Name,
        };

    public static IEnumerable<ProductType> ToModel(this IEnumerable<ProductTypeDto> dtos)
    {
        foreach (var dto in dtos)
            yield return ToModel(dto);
    }
}
