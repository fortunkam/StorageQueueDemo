using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using SharedModels;

namespace Client
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

        public void SendMessage(DemoMessage message)
        {
            var serializedMesage = JsonSerializer.Serialize(message);
            var cloudQueueMessage = new CloudQueueMessage(serializedMesage);
            _queue.AddMessage(cloudQueueMessage);
        }
    }
}
