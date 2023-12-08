using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    class Program
    {
        static async Task Main(string[] args)
        {
           
            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://npyxpnfc:ebpKf4tx9eGv91DG9EpPikemQPxnE8hX@porpoise.rmq.cloudamqp.com/npyxpnfc");



            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                // Bir fanout tipinde değişim (exchange) tanımlanır.
                channel.ExchangeDeclare(
                    exchange: "fanout-exchange-example",
                    type: ExchangeType.Fanout);

                
                for (int i = 450; i <= 501; i++)
                {
                   
                    await Task.Delay(200);

                    // Mesaj oluşturulur ve UTF-8'e çevrilerek byte dizisine dönüştürülür.
                    byte[] message = Encoding.UTF8.GetBytes($"MTH{i}");

                    // Oluşturulan mesaj, fanout değişimine gönderilir.
                    // Routing key belirtilmemiştir çünkü fanout, mesajları tüm kuyruklara gönderir.
                    channel.BasicPublish(
                        exchange: "fanout-exchange-example",
                        routingKey: "",
                        body: message);
                }
            }
        }
    }
}
