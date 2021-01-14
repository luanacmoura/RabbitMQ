using RabbitMQ.Client;
using System;
using System.Text;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Insira a mensagem que deseja enviar:");
                var message = Console.ReadLine();
                Send(message);
            }
        }

        public static void Send(string message)
        {
            //Criando a conexão com o servidor
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                //Criando o nosso canal
                using (var channel = connection.CreateModel())
                {
                    //channel.ExchangeDeclarePassive;
                    channel.ExchangeDeclare(exchange: "Exchange1", type: ExchangeType.Fanout);

                    //Declarando a fila
                    //channel.QueueDeclarePassive
                    channel.QueueDeclare(queue: "Fila1",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        null);

                    channel.QueueBind(queue: "Fila1",
                              exchange: "Exchange1",
                              routingKey: "Chave1"); //Também chamada de binding key
                    //se o exchange for do tipo fanout ele vai ignorar a routing key


                    var body = Encoding.UTF8.GetBytes(message); 

                    //Enviando uma mensagem pra fila que criamos (subscribe)
                    channel.BasicPublish(exchange: "Exchange1", //se estiver vazio, por padrão utiliza uma direct exchange
                        routingKey: "Chave1", // se o exchange estiver vazio, precisa ser igual ao nome da fila para que ele a encontre
                        basicProperties: null,
                        body: body);

                    Console.WriteLine(" [x] Mensagem enviada: {0}", message);
                    Console.ReadLine();

                }
            }
        }
    }
}
