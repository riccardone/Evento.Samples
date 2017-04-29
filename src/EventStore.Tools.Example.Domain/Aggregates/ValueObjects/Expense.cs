namespace EventStore.Tools.Example.Domain.Aggregates.ValueObjects
{
    internal class Expense
    {
        public decimal Value { get; }
        public string Description { get; }

        public Expense(decimal value, string description)
        {
            Value = value;
            Description = description;
        }
    }
}
