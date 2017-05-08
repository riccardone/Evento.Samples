using System;
using Evento;

namespace Domain.Commands
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
