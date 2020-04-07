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
using System.ComponentModel;
using System.Threading;
using System.Linq;

namespace Monitor
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

            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += Worker_DoWork;
        }

        private readonly CloudQueue _queue;

        private readonly BackgroundWorker worker;

        public void Start()
        {
            if (!worker.IsBusy)
            {

                worker.RunWorkerAsync();
                IsRunning?.Invoke(true);
            }
        }

        public delegate void MessageArrivedHandler(DemoMessage[] message);
        public event MessageArrivedHandler MessagesArrived;

        public delegate void IsRunningHandler(bool IsRunning);
        public event IsRunningHandler IsRunning;

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                if (worker.CancellationPending)
                {
                    return;
                }

                var messages = _queue.PeekMessages(10, new QueueRequestOptions
                {
                    ServerTimeout = new TimeSpan(0, 0, 2)
                });

                var demoMessages = messages.Select(r =>
                    JsonSerializer.Deserialize<DemoMessage>(r.AsString)).ToArray();

                MessagesArrived?.Invoke(demoMessages);



                Thread.Sleep(1000);
            }
            
        }

        public void Stop()
        {
            if(worker.IsBusy)
            {
                worker.CancelAsync();
                IsRunning?.Invoke(false);
            }
        }

    }
}
