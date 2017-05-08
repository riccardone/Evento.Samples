using Evento;

namespace Domain.Events
{
    public class ExpenseRegistered : Event
    {
        public decimal Value { get; }
        public string Description { get; }
        public decimal Balance { get; }

        public ExpenseRegistered(decimal value, string description, decimal balance)
        {
            Value = value;
            Description = description;
            Balance = balance;
        }
    }
}
