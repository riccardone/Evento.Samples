using System;
using EventStore.Tools.Infrastructure;

namespace EventStore.Tools.Example.Domain.Commands
{
    public class CreateAssociateAccount : Command
    {
        public Guid CorrelationId { get; }
        public Guid AssociateId { get; }

        public CreateAssociateAccount(Guid correlationId, Guid associateId)
        {
            CorrelationId = correlationId;
            AssociateId = associateId;
        }
    }
}
