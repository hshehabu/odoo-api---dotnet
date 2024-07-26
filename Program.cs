using System;
using System.Threading.Tasks;
using PortaCapena.OdooJsonRpcClient;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;

namespace OdooIntegration
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Configuration
            var config = new OdooConfig(
                apiUrl: "https://odoo-api-url.com", // Replace with your Odoo URL
                dbName: "odoo-db-name",             // Replace with your database name
                userName: "admin",                  // Replace with your username
                password: "admin"                   // Replace with your password
            );

            var odooClient = new OdooClient(config);

            // Check Odoo version
            var versionResult = await odooClient.GetVersionAsync();
            Console.WriteLine($"Odoo Version: {versionResult}");

            // Login (optional, handled by OdooClient automatically)
            var loginResult = await odooClient.LoginAsync();
            Console.WriteLine($"Login Success: {loginResult}");

            // Get model
            var tableName = "product.product";
            var modelResult = await odooClient.GetModelAsync(tableName);
            Console.WriteLine($"Model Result: {modelResult}");

            var model = OdooModelMapper.GetDotNetModel(tableName, modelResult.Value);
            Console.WriteLine($"DotNet Model: {model}");

            // CRUD Operations
            var repository = new OdooRepository<OdooProductProduct>(config);

             // Read
            var result = await repository.Query().ToListAsync();
            if (result != null)
            {
                // Extracting the data from OdooResult
                var products = result.Value; // Adjust this based on the actual property name

                Console.WriteLine("Products:");
                foreach (var product in products)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(product, Formatting.Indented));
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
            // Create
            var createModel = OdooDictionaryModel.Create(() => new OdooProductProduct
            {
                DisplayName = "New Product",
                Price = 99.99
            });
            var createResult = await repository.CreateAsync(createModel);
            Console.WriteLine($"Create Result: {createResult}");

            // Update
            var updateModel = OdooDictionaryModel.Create(() => new OdooProductProduct
            {
                DisplayName = "Updated Product"
            });
            var updateResult = await repository.UpdateAsync(updateModel, 1); // Replace with actual ID
            Console.WriteLine($"Update Result: {updateResult}");

            // Delete
            var deleteResult = await repository.DeleteAsync(1); // Replace with actual ID
            Console.WriteLine($"Delete Result: {deleteResult}");
        }
    }

    [OdooTableName("product.product")]
    public class OdooProductProduct : IOdooModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("price")]
        public double? Price { get; set; }
    }
}
