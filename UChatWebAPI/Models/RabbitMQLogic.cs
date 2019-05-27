using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace UChatWebAPI.Models
{
    public class RabbitMQLogic
    {
        private string queue = "chatQueue";
        private string exchange = "chatExchange";
        private string routingKey = "routingKey";

        public IConnection GetConnection()
        {
            ConnectionFactory factory = new ConnectionFactory();
            //factory.UserName = "adam";
            //factory.Password = "1234";
            factory.Port = 5672;
            factory.HostName = "localhost";
            //factory.VirtualHost = "/";

            return factory.CreateConnection();
        }

        public bool Send(IConnection con, string message)
        {
            try
            {
                IModel channel = con.CreateModel();
                channel.ExchangeDeclare(exchange: "chatExchange", type: ExchangeType.Direct);
                channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queue: queue, exchange: exchange, routingKey: routingKey, arguments: null);
                var msg = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("chatExchange", "routingKey", null, msg);
            }
            catch (Exception)
            {
                return false;
            }

            return true;

        }
        public string Receive(IConnection con)
        {
            try
            {
                IModel channel = con.CreateModel();
                channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                BasicGetResult result = channel.BasicGet(queue: queue, autoAck: true);

                if (result != null)
                    return Encoding.UTF8.GetString(result.Body);
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}