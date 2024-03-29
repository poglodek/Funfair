﻿using Newtonsoft.Json;
using Users.Core.Exceptions;

namespace Users.Core.ValueObjects;

public record Name
{
    [JsonProperty("value")]
    public string Value { get;}

    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidNameException(value);
        }
        
        Value = value;
    }
    public static implicit operator Name(string date) => new (date);
};