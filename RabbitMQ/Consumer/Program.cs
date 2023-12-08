using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

class Program
{
    static void Main()
    {
        // RabbitMQ bağlantısını oluşturmak için ConnectionFactory kullanılır.
        ConnectionFactory factory = new ConnectionFactory();

        // RabbitMQ sunucusunun URI'sini belirtir.
        factory.Uri = new Uri("amqps://npyxpnfc:ebpKf4tx9eGv91DG9EpPikemQPxnE8hX@porpoise.rmq.cloudamqp.com/npyxpnfc");



        using (IConnection connection = factory.CreateConnection())
        using (IModel channel = connection.CreateModel())
        {
            // Bir fanout tipinde değişim (exchange) tanımlanır.
            channel.ExchangeDeclare(
                exchange: "fanout-exchange-example",
                type: ExchangeType.Fanout);

            // Kullanıcıdan alınan isimle bir kuyruk oluşturulur.
            Console.Write("Queue Name: ");
            string queueName = Console.ReadLine();
            channel.QueueDeclare(
                queue: queueName,
                exclusive: false);

            // Oluşturulan kuyruğu fanout değişimine bağlar. Fanout, mesajları tüm kuyruklara gönderir.
            channel.QueueBind(
                queue: queueName,
                exchange: "fanout-exchange-example",
                routingKey: string.Empty);

            // EventingBasicConsumer kullanarak bir tüketici (consumer) oluşturulur.
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queueName, true, consumer);

            // Mesaj alındığında çalışacak olan olay dinleyicisi tanımlanır.
            consumer.Received += (sender, e) =>
            {
                // Gelen mesaj byte dizisinden string'e çevrilir ve ekrana yazdırılır.
                string message = Encoding.UTF8.GetString(e.Body.Span);
                Console.WriteLine(message);
            };

            
            Console.Read();
        }
    }
}