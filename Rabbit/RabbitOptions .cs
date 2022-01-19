using RabbitMQ.Client;

namespace NRH_UserAdress.Rabbit
{
    public class RabbitChannel
    {
        public static IModel Channel;
        private static IConnection _connection;
        public static IConnection Connection => _connection;

        public static RabbitMQ.Client.IModel Init()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = "NRHRabbit";
            factory.Password = "qsDf58965!";
            factory.VirtualHost = "/";
            factory.HostName = "35.183.1.211";
            factory.Port = 5672;
            IConnection conn = factory.CreateConnection();

            Channel = conn.CreateModel();
            return Channel;
        }

        public static void CloseConnection()
        {
            if (Channel != null)
            {
                Channel.Close();
                Channel.Dispose();
            }

            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }
    }
}


