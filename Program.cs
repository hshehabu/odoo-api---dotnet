
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace OdooRestApiExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new OdooConfig
            {
                ApiUrl = "http://localhost:8069",
                DbName = "dsp",
                UserName = "admin@example.com",
                Password = "admin1234"
            };

            var odooClient = new OdooApiClient(config);

            // Login
            var userId = await odooClient.LoginAsync();
            if (userId == null)
            {
                Console.WriteLine("Login failed.");
                return;
            }
            Console.WriteLine("Login successful. User ID: " + userId);

            // Create       
            // var createValues = new Dictionary<string, object>
            // {
            //     { "name", "Company X" },
            //     { "description", "this is a test" },
            // };
            // var createResult = await odooClient.CreateAsync("dsp.company", createValues);
            // Console.WriteLine("Create result: " + createResult);

            var createValues = new Dictionary<string, object>
            {
                { "name", "MR X" },
                { "position", "Accountant" },
                {"company_id" , 1}
            };
            var createResult = await odooClient.CreateAsync("dsp.employee", createValues);
            Console.WriteLine("Create result: " + createResult);

            // // Read
            // var readFields = new[] { "name", "description" };
            // var readResult = await odooClient.ReadAsync("dsp.company", createResult.Value<int>(), readFields);
            // Console.WriteLine("Read result: " + readResult);

            // Update
            // var updateValues = new Dictionary<string, object>
            // {
            //     { "name", "esmael"}
            // };
            // var updateResult = await odooClient.UpdateAsync("dsp.company", 2, updateValues);
            // Console.WriteLine("Update result: " + updateResult);

            // // Delete
            // var deleteResult = await odooClient.DeleteAsync("dsp.company", 3);
            // Console.WriteLine("Delete result: " + deleteResult);
        }
    }
}
