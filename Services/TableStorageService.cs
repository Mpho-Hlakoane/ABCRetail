using Azure;
using Azure.Data.Tables;
using ABCRetail.Models;
using ABCRetailWebApp.Models;
using Microsoft.Extensions.Configuration;

namespace ABCRetail.Services
{
    public class TableStorageService
    {
        private readonly TableClient _customerTableClient;
        private readonly TableClient _adminTableClient;

        public TableStorageService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            var customerTableName = configuration["AzureStorage:TableName"]; // "Customers"
            var adminTableName = "Admins"; // Separate table for admin accounts

            // Initialize Customer TableClient
            _customerTableClient = new TableClient(connectionString, customerTableName);
            _customerTableClient.CreateIfNotExists();

            // Initialize Admin TableClient
            _adminTableClient = new TableClient(connectionString, adminTableName);
            _adminTableClient.CreateIfNotExists();
        }

        // ========================
        // Customer / Product Methods
        // ========================

        public async Task UpsertCustomerAsync(CustomerProductEntity entity)
        {
            await _customerTableClient.UpsertEntityAsync(entity, TableUpdateMode.Replace);
        }

        public async Task AddCustomerAsync(CustomerProductEntity entity)
        {
            await _customerTableClient.AddEntityAsync(entity);
        }

        public async Task<CustomerProductEntity?> GetCustomerAsync(string partitionKey, string rowKey)
        {
            try
            {
                var response = await _customerTableClient.GetEntityAsync<CustomerProductEntity>(partitionKey, rowKey);
                return response.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return null;
            }
        }

        public async Task<List<CustomerProductEntity>> GetAllCustomersAsync()
        {
            var customers = new List<CustomerProductEntity>();
            await foreach (var entity in _customerTableClient.QueryAsync<CustomerProductEntity>())
            {
                customers.Add(entity);
            }
            return customers;
        }

        public async Task DeleteCustomerAsync(string partitionKey, string rowKey)
        {
            await _customerTableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        // ========================
        // Admin Methods
        // ========================

        public async Task<AdminEntity?> GetAdminAsync(string partitionKey, string rowKey)
        {
            try
            {
                var response = await _adminTableClient.GetEntityAsync<AdminEntity>(partitionKey, rowKey);
                return response.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return null;
            }
        }

        public async Task AddAdminAsync(AdminEntity entity)
        {
            await _adminTableClient.AddEntityAsync(entity);
        }

        public async Task UpsertAdminAsync(AdminEntity entity)
        {
            await _adminTableClient.UpsertEntityAsync(entity, TableUpdateMode.Replace);
        }
    }
}
