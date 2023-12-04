using System.Text;
using System.Text.Json.Serialization;
using EmailService.Dto;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Tls;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EmailService.Domain;

public class RabbitMqConsumer : BackgroundService
{
    private readonly RabbitMqConfiguration _configuration;
    private readonly IModel _model;

    public RabbitMqConsumer(IOptions<RabbitMqConfiguration> options)
    {
        _configuration = options.Value;
        var factory = new ConnectionFactory
        {
            Uri = new Uri("amqps://izxkxvtx:SAPzlALKx32sg-P9adRMEJ8tcWkloUfC@toad.rmq.cloudamqp.com/izxkxvtx")
        };
      
        var connection = factory.CreateConnection();
        _model = connection.CreateModel();
        _model.QueueDeclare(
            queue: _configuration.Queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_model);
        consumer.Received += (sender, eventArgs) =>
        {
            var contentArray = eventArgs.Body.ToArray();
            var contentString = Encoding.UTF8.GetString(contentArray);
            Console.Write(contentString);
            _model.BasicAck(eventArgs.DeliveryTag, false);
        };
        _model.BasicConsume(_configuration.Queue, false, consumer);
        return Task.CompletedTask;
    }
}