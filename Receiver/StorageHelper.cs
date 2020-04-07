using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.Json;

namespace Receiver
{
    public class StorageHelper
    {
        public StorageHelper()
        {
            var secretClient = new SecretClient(
                new System.Uri(ConfigurationManager.AppSettings["KeyVaultName"]),
                new DefaultAzureCredential()
                );

            var storageAccount = CloudStorageAccount.Parse(secretClient.GetSecret("StorageConnectionString").Value.Value);
            var queueClient = storageAccount.CreateCloudQueueClient();
            _queue = queueClient.GetQueueReference(secretClient.GetSecret("QueueName").Value.Value);
        }

        private readonly CloudQueue _queue;

        public DemoMessage GetMessage()
        {            
            var cloudQueueMessage = _queue.GetMessage();
            _queue.DeleteMessage(cloudQueueMessage);

            return JsonSerializer.Deserialize<DemoMessage>(cloudQueueMessage.AsString);

        }
    }
}
