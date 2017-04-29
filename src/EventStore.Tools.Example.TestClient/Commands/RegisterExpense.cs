using System;
using EventStore.Tools.Infrastructure;

namespace EventStore.Tools.Example.TestClient.Commands
{
    public class RegisterExpense : Command
    {
        public Guid CorrelationId { get; }
        public decimal Value { get; }
        public string Description { get; }

        public RegisterExpense(Guid correlationId, decimal value, string description)
        {
            CorrelationId = correlationId;
            Value = value;
            Description = description;
        }
    }
}
