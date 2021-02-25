using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace HeartbeatListenerOne
{
    class Program
    {
        static readonly string connectionString = "";
        private static readonly string topicName = "";
        private static readonly string subscriptionName = "";

        static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine("HEARTBEAT LISTENER ONE");
            await ReceiveHeartbeatFromSubscriptionAsync();

            while (true)
            {
                await ReceiveHeartbeatFromSubscriptionAsync();
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }

        static async Task ReceiveHeartbeatFromSubscriptionAsync()
        {
            await using var client = new ServiceBusClient(connectionString);
            var processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions());

            processor.ProcessMessageAsync += MessageHandler;

            processor.ProcessErrorAsync += ErrorHandler;

            await processor.StartProcessingAsync();

            var waitTime = 1;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Waiting for a {waitTime} minute(s) delay before stopping the processor");

            await Task.Delay(TimeSpan.FromMinutes(waitTime));

            await processor.StopProcessingAsync();
            Console.WriteLine("Stopped receiving messages");
        }

        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            var body = args.Message.Body.ToString();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Received: {body}");

            await args.CompleteMessageAsync(args.Message);
        }

        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            return Task.CompletedTask;
        }
    }
}
