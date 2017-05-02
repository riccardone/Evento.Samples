using System;

namespace Evento.Domain.Events
{
    public class AssociateAccountCreated : Event
    {
        public Guid CorrelationId { get; }
        public Guid AssociateId { get; }

        public AssociateAccountCreated(Guid correlationId, Guid associateId)
        {
            CorrelationId = correlationId;
            AssociateId = associateId;
        }
    }
}
