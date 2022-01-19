using System;
using System.Text;
using System.Threading;
using NRH_UserAdress.Rabbit;
using RabbitMQ.Client;

namespace NRH_UserAdress.Rabbit
{
    public class RabbitSend
    {
        //public static void Run(string integrationEvent, string eventData )
        //{
        //for (var i = 0; i < 5; i++)
        //{
        //   Publish(i);
        //   Thread.Sleep(500);
        //}
        //Publish(integrationEvent, eventData);
        // }

        public static void Publish(string integrationEvent, string eventData)
        {
            RabbitChannel.Channel.QueueDeclare(queue: "UserAdress.user",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            var body = Encoding.UTF8.GetBytes(eventData);
            RabbitChannel.Channel.BasicPublish(exchange: "UserAdress",
                                             routingKey: integrationEvent,
                                             basicProperties: null,
                                             body: body);
            Console.WriteLine("sent!");
            RabbitChannel.CloseConnection();
        }
    }
}