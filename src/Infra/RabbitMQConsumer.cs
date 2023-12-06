using System.Text;
using EmailService.Dto;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EmailService.Domain;

public class RabbitMqConsumer : BackgroundService
{
    private readonly RabbitMqConfiguration _configuration;
    private readonly IModel _model;

    private readonly IEmailInfraPort _emailInfraPort;

    public RabbitMqConsumer(IOptions<RabbitMqConfiguration> options, IEmailInfraPort emailInfraPort)
    {
        _emailInfraPort = emailInfraPort;
        _configuration = options.Value;
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_configuration.URI)
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

            var emailDto = JsonConvert.DeserializeObject<EmailDto>(contentString);
            _emailInfraPort.sendEmail(emailDto);

            _model.BasicAck(eventArgs.DeliveryTag, false);
        };
        _model.BasicConsume(_configuration.Queue, false, consumer);
        return Task.CompletedTask;
    }
}