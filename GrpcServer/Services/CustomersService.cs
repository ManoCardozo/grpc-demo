using Grpc.Core;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace GrpcServer.Services
{
    public class CustomersService : Customer.CustomerBase
    {
        private readonly ILogger<CustomersService> _logger;

        public CustomersService(ILogger<CustomersService> logger)
        {
            _logger = logger;
        }

        public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, ServerCallContext context)
        {
            var output = new CustomerModel();

            if (request.UserId == 1)
            {
                output.FirstName = "Jamie";
                output.LastName = "Smith";
            }
            else if (request.UserId == 2)
            {
                output.FirstName = "Jane";
                output.LastName = "Doe";
            }
            else
            {
                output.FirstName = "Greg";
                output.LastName = "Thomas";
            }

            return Task.FromResult(output);
        }

        public override async Task GetCustomers(CustomersLookupModel request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
        {
            var customers = new List<CustomerModel>
            {
                new CustomerModel
                {
                    FirstName = "Jamie",
                    LastName = "Smith",
                    EmailAddess = "jamie.smith@abc.com",
                    IsActive = true
                },
                new CustomerModel
                {
                    FirstName = "Greg",
                    LastName = "Thomas",
                    EmailAddess = "greg.thomas@abc.com",
                    IsActive = false
                },
                new CustomerModel
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    EmailAddess = "jane.doe@abc.com",
                    IsActive = true
                },
                new CustomerModel
                {
                    FirstName = "Mary",
                    LastName = "Jane",
                    EmailAddess = "mary.jane@abc.com",
                    IsActive = true
                },
                new CustomerModel
                {
                    FirstName = "John",
                    LastName = "Smith",
                    EmailAddess = "john.smith@abc.com",
                    IsActive = true
                }
            };

            foreach (var customer in customers)
            {
                await Task.Delay(1500);
                await responseStream.WriteAsync(customer);
            }
        }
    }
}
