using System;
using System.Collections.Generic;
using System.Text;
using EventStore.ClientAPI;
using EventStore.Tools.Example.TestClient.ReadModel;
using Newtonsoft.Json;

namespace EventStore.Tools.Example.TestClient
{
    /// <summary>
    /// This is a simple Read Model Synchoniser. It listen for internal Domain Events ExpenseRegistered and IncomeRegistered
    /// In a real world scenario I would probably listen for some "external" events that contains what we need 
    /// and are detached from the internal Domain Events.
    /// Using EventStore it is easy create a special projection that provide the events to keep synchronised this read model
    /// </summary>
    class ReadModelSynchroniser
    {
        private readonly IEventStoreConnection _connection;

        public ReadModelSynchroniser(IEventStoreConnection connection)
        {
            _connection = connection;
        }

        public void Start()
        {
            _connection.SubscribeToAllFrom(Position.Start, CatchUpSubscriptionSettings.Default, EventAppeared);
            Console.WriteLine("ReadModelSynchroniser started");
        }

        private static void EventAppeared(EventStoreCatchUpSubscription eventStoreCatchUpSubscription, ResolvedEvent resolvedEvent)
        {
            if (!resolvedEvent.Event.EventType.Equals("ExpenseRegistered") &&
                !resolvedEvent.Event.EventType.Equals("IncomeRegistered")) return;

            var evt =
                JsonConvert.DeserializeObject<CurrentBalanceDto>(Encoding.UTF8.GetString(resolvedEvent.Event.Data));

            var metadata =
                JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(
                    Encoding.UTF8.GetString(resolvedEvent.Event.Metadata));

            Console.WriteLine($"The account with correlatioId {metadata["$correlationId"]} has a current balance of £{evt.Balance}");
            // TODO add more required info to the dto and synchronise a database as a read model
        }
    }
}
