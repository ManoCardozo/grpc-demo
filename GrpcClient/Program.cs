using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");

            await GreetGrpc(channel);

            await GetCustomer(channel);

            await GetCustomers(channel);

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static async Task GreetGrpc(GrpcChannel channel)
        {
            var client = new Greeter.GreeterClient(channel);
            var input = new HelloRequest
            {
                Name = "gRPC"
            };

            var reply = await client.SayHelloAsync(input);

            Console.WriteLine("Greeting");
            Console.WriteLine($"- {reply.Message}");
        }

        private static async Task GetCustomer(GrpcChannel channel)
        {
            var client = new Customer.CustomerClient(channel);
            var input = new CustomerLookupModel
            {
                UserId = 2
            };

            var customer = await client.GetCustomerInfoAsync(input);

            Console.WriteLine();
            Console.WriteLine("Customer Requested");
            Console.WriteLine($"- {customer.FirstName} {customer.LastName}");
        }

        private static async Task GetCustomers(GrpcChannel channel)
        {
            var client = new Customer.CustomerClient(channel);
            var input = new CustomersLookupModel();

            Console.WriteLine();
            Console.WriteLine("Customer List");
            using (var call = client.GetCustomers(input))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var currentCustomer = call.ResponseStream.Current;
                    Console.WriteLine($"- {currentCustomer.FirstName} {currentCustomer.LastName}: {currentCustomer.EmailAddess}");
                }
            }
        }
    }
}
