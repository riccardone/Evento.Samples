using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Commands;
using Evento;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Newtonsoft.Json;

namespace TestClient
{
    /// <summary>
    /// This test client interact with the AssociateAccount Micro-Service
    /// The AssociateAccountEndPoint on the other end listen the 'input-account' stream for input commands
    /// In a real world scenarion this could be a WebUi or and Http Api or a FromFileAdapter or a FromAmqpAdapter or whatever
    /// </summary>
    class Program
    {
        private static IEventStoreConnection _connection;

        static void Main(string[] args)
        {
            Console.WriteLine("Loading EventStore");
            if (Process.GetProcessesByName("EventStore.ClusterNode").Length == 0)
            {
                Console.WriteLine("To run this sample program you need to start EventStore on this machine.");
                Console.WriteLine("If you don't have it you can download from https://geteventstore.com/downloads/");
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
                return;
            }
            Connect();
            var readModelSynchroniser = new ReadModelSynchroniser(_connection);
            readModelSynchroniser.Start();
            var repeat = true;
            var correlationId = Guid.Empty;
            Console.WriteLine("Press 1 to send a Create command");
            Console.WriteLine("Press 2 to send a Register 10£ RegisterExpense command");
            Console.WriteLine("Press 3 to send a Register 5£ RegisterIncome command");
            Console.WriteLine("Press 5 to exit the program");
            do
            {
                var key = Convert.ToString(Console.ReadKey().KeyChar);
                int option;
                if (int.TryParse(key, out option))
                {
                    switch (option)
                    {
                        case 1:
                            correlationId = Guid.NewGuid();
                            SendCommand(new CreateAssociateAccount(correlationId, Guid.NewGuid()));
                            Console.WriteLine($"CreateAssociateAccount sent with correlationId {correlationId}");
                            break;
                        case 2:
                            if (!Guid.Empty.Equals(correlationId))
                            {
                                SendCommand(new RegisterExpense(correlationId, 10, "test expense"));
                                Console.WriteLine($"RegisterExpense sent with correlationId {correlationId}");
                            }
                            else
                                ShowNotValidMessage();
                            break;
                        case 3:
                            if (!Guid.Empty.Equals(correlationId))
                            {
                                SendCommand(new RegisterIncome(correlationId, 5, "test income"));
                                Console.WriteLine($"RegisterIncome sent with correlationId {correlationId}");
                            }
                            else
                                ShowNotValidMessage();
                            break;
                        case 5:
                            repeat = false;
                            break;
                        default:
                            Console.WriteLine("Unrecognised option");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Illegal input");
                }
            } while (repeat);
            _connection.Close();
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }

        private static void ShowNotValidMessage()
        {
            Console.WriteLine("You have to create an Account first");
        }

        private static void SendCommand(Command command)
        {
            var eventData = new EventData(Guid.NewGuid(), command.GetType().Name, true, SerializeObject(command), null);
            _connection.AppendToStreamAsync("input-account", ExpectedVersion.Any, eventData).Wait();
        }

        private static void Connect()
        {
            var connSettings = ConnectionSettings.Create().SetDefaultUserCredentials(new UserCredentials("admin", "changeit"))
                .KeepReconnecting().KeepRetrying().Build();
            _connection = EventStoreConnection.Create(connSettings,
                new IPEndPoint(IPAddress.Loopback, 1113), "ES-TestClientSender");
            _connection.ConnectAsync().Wait();
        }

        private static byte[] SerializeObject(object obj)
        {
            var jsonObj = JsonConvert.SerializeObject(obj);
            var data = Encoding.UTF8.GetBytes(jsonObj);
            return data;
        }
    }
}
