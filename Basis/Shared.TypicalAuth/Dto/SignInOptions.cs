﻿namespace Shared.Dto;

public class SignInOptions
{
    public SignInOptions() { /* Method intentionally left empty.*/ }
    public SignInOptions(IEnumerable<string> types)
    {
        Types = types.ToList() ?? new List<string>();
    }

    public List<string> Types { get; set; }


}
