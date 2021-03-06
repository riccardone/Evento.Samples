﻿namespace Domain.Aggregates.ValueObjects
{
    internal class Income
    {
        public decimal Value { get; }
        public string Description { get; }

        public Income(decimal value, string description)
        {
            Value = value;
            Description = description;
        }
    }
}
