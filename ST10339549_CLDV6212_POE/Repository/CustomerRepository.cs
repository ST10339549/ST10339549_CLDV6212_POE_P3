using Dapper;
using ST10339549_CLDV6212_POE.Helpers;
using ST10339549_CLDV6212_POE.Interfaces;
using ST10339549_CLDV6212_POE.Models;
using System.Data;

namespace ST10339549_CLDV6212_POE.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public CustomerRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            if (string.IsNullOrEmpty(customer.CustomerId))
            {
                customer.CustomerId = await GenerateCustomerIdAsync();
            }

            using IDbConnection connection = _dbConnection.CreateConnection();
            var query = "INSERT INTO Customers (CustomerId, CustomerName, CustomerSurname, CustomerEmail, CustomerAddress) VALUES (@CustomerId, @CustomerName, @CustomerSurname, @CustomerEmail, @CustomerAddress)";
            await connection.ExecuteAsync(query, customer);
        }


        public async Task<Customer> GetCustomerByIdAsync(string customerId)
        {
            using IDbConnection connection = _dbConnection.CreateConnection();
            var query = "SELECT * FROM Customers WHERE CustomerId = @CustomerId";
            return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { CustomerId = customerId });
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            using IDbConnection connection = _dbConnection.CreateConnection();
            var query = "SELECT * FROM Customers";
            return await connection.QueryAsync<Customer>(query);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            using IDbConnection connection = _dbConnection.CreateConnection();
            var query = "UPDATE Customers SET CustomerName = @CustomerName, CustomerSurname = @CustomerSurname, CustomerEmail = @CustomerEmail, CustomerAddress = @CustomerAddress WHERE CustomerId = @CustomerId";
            await connection.ExecuteAsync(query, customer);
        }

        public async Task DeleteCustomerAsync(string customerId)
        {
            using IDbConnection connection = _dbConnection.CreateConnection();
            var query = "DELETE FROM Customers WHERE CustomerId = @CustomerId";
            await connection.ExecuteAsync(query, new { CustomerId = customerId });
        }

        public async Task<string> GenerateCustomerIdAsync()
        {
            using IDbConnection connection = _dbConnection.CreateConnection();

            var query = "SELECT TOP 1 CustomerId FROM Customers ORDER BY CustomerId DESC";
            var lastCustomerId = await connection.QueryFirstOrDefaultAsync<string>(query);

            if (string.IsNullOrEmpty(lastCustomerId))
            {
                return "C001";
            }

            int numericPart = int.Parse(lastCustomerId.Substring(1));
            numericPart++;

            return $"C{numericPart.ToString("D3")}";
        }
    }
}
