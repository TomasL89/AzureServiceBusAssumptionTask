using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace HeartbeatSender
{
    class Program
    {
        static readonly string connectionString = "";
        private static readonly string topicName = "";

        static async Task Main(string[] args)
        {
            var heartbeatCount = 0;

            while (true)
            {
                await SendHeartbeatAsync(heartbeatCount);
                await Task.Delay(TimeSpan.FromSeconds(30));
                heartbeatCount += 1;
            }
        }

        static async Task SendHeartbeatAsync(int heartBeat)
        {
            await using var client = new ServiceBusClient(connectionString);
            var sender = client.CreateSender(topicName);

            var message = new ServiceBusMessage($"{DateTime.UtcNow} Heartbeat {heartBeat}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"SENT {message.Body}");
            await sender.SendMessageAsync(message);
        }
    }
}
