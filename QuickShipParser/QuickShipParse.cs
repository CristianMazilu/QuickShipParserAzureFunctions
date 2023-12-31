﻿using System;
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
using Azure;

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
            Exception exception = null;
            string? model = req.Query["model"];

            if (model == null)
            {
                var errorResponse = new ReturnStructure(null, false, null, "Model parameter is missing.");
                return new BadRequestObjectResult(
                    new ReturnStructure(null, false, null, "Model parameter is missing.").JsonSerialized());
            }

            IMatch result = new FailedMatch("Base model not found.");
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

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

                foreach (var jsonContent in jsonFileContents)
                {
                    var modelStructure = ModelStructure.FromJson(jsonContent);

                    result = modelStructure.Match(model);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (exception != null)
            {
                return new BadRequestObjectResult(
                        new ReturnStructure(model, result?.Success(), result?.RemainingText(), exception.Message).JsonSerialized());
            }

            return new OkObjectResult(
                    new ReturnStructure(model, result?.Success(), result?.RemainingText()).JsonSerialized());
        }

        public class ReturnStructure
        {
            public string? ModelString { get; set; }
            public bool? QuickShipValid { get; set; }
            public string? QuickShipInvalidPart { get; set; }
            public string ExceptionMessage { get; set; }

            public ReturnStructure(string? modelString, bool? quickShipValid, string? quickShipInvalidPart, string exceptionMessage = "None")
            {
                ModelString = modelString;
                QuickShipValid = quickShipValid;
                QuickShipInvalidPart = quickShipInvalidPart;
                ExceptionMessage = exceptionMessage;
            }

            public string JsonSerialized()
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented, // For pretty printing
                    NullValueHandling = NullValueHandling.Ignore // To ignore null values
                };

                return JsonConvert.SerializeObject(this, serializerSettings);

            }
        }
    }
}
