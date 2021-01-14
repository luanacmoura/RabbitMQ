using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        channel.BasicAck(ea.DeliveryTag, false); // se for true, então vai confirmar o recebimento de todas as mensagems com aquela deliverytag

                        //channel.BasicReject(ea.DeliveryTag, true);

                        //channel.BasicNack(ea.DeliveryTag, true, true);
                        Console.WriteLine(" [x] Mensagem recebida: {0}", message);
                    };

                    channel.BasicConsume(queue: "Fila1",
                                         autoAck: false,
                                         consumer: consumer);
                    
                    Console.WriteLine(" Pressione qualquer tecla para sair.");
                    Console.ReadLine();

                }
            }
        }
    }
}
