using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using System.Collections.Generic;

namespace QuickShipParser
{
    public class QuickShipParse
    {
        private const string BlobContainerName = "quickship-az-fn-config";

        [FunctionName("QuickShipParse")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string model = req.Query["model"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            model = model ?? data?.model;

            // Create a Blob Service Client
            string connectionString = Environment.GetEnvironmentVariable("quickship-az-fn-config-token");
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Access the container
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(BlobContainerName);

            // Get all the *.json files content and store it in a list of strings
            List<string> jsonFileContents = new List<string>();
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                if (Path.GetExtension(blobItem.Name).Equals(".json", StringComparison.OrdinalIgnoreCase))
                {
                    BlobClient blobClient = containerClient.GetBlobClient(blobItem.Name);
                    BlobDownloadInfo download = await blobClient.DownloadAsync();
                    using (StreamReader reader = new StreamReader(download.Content))
                    {
                        jsonFileContents.Add(await reader.ReadToEndAsync());
                    }
                }
            }

            IMatch result = new FailedMatch("Base model not found.");
            foreach (var jsonContent in jsonFileContents)
            {
                var modelStructure = ModelStructure.FromJson(jsonContent);

                result = modelStructure.Match(model);
            }

            string responseMessage = string.IsNullOrEmpty(model)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Model string {model} is {result.Success()}. Error is in the following part of the string: {result.RemainingText()}";

            return new OkObjectResult(responseMessage);
        }
    }
}
