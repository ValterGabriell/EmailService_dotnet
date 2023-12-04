# Documentação do Serviço de Envio de Emails via RabbitMQ

## Introdução

Bem-vindo ao repositório do serviço de envio de emails utilizando RabbitMQ. Este serviço foi desenvolvido para facilitar o envio de emails em suas aplicações, oferecendo uma integração eficiente e escalável através do RabbitMQ, um sistema de mensagens robusto.

## Objetivo

O objetivo principal deste serviço é fornecer uma solução simplificada e confiável para o envio de emails em suas aplicações, utilizando a arquitetura de mensagens assíncronas proporcionada pelo RabbitMQ.

## Funcionalidades

### 1. Envio de Emails

O serviço permite que suas aplicações enviem emails de forma assíncrona, garantindo um desempenho eficiente e evitando possíveis gargalos no processo.

### 2. Integração com RabbitMQ

A integração com o RabbitMQ permite que suas aplicações enviem mensagens de forma assíncrona para o serviço de envio de emails. Isso possibilita uma melhor escalabilidade e desacoplação entre os componentes do sistema.

### 3. Personalização de Mensagens

Você pode personalizar facilmente as mensagens de email, incluindo assunto, corpo e destinatários. O serviço suporta a inclusão de variáveis dinâmicas para personalizar ainda mais o conteúdo dos emails.

## Configuração

### 1. Configuração do RabbitMQ

Antes de começar, certifique-se de ter um servidor RabbitMQ configurado e acessível para suas aplicações. Configure as credenciais de conexão no arquivo de configuração do serviço.

```yaml
rabbitmq:
  host: <endereco_do_servidor>
  porta: <porta>
  usuario: <usuario>
  senha: <senha>
  fila: <nome_da_fila>
```

### 2. Configuração de Email

Configure as credenciais do serviço de envio de emails no mesmo arquivo de configuração.

```yaml
email:
  host: <servidor_smtp>
  porta: <porta>
  usuario: <usuario>
  senha: <senha>
  remetente: <email_remetente>
```

## Uso

## Exemplo de Uso em Java com Spring Boot

A seguir, apresento um exemplo básico de como você pode integrar o serviço de envio de emails via RabbitMQ em uma aplicação Java usando Spring Boot. Certifique-se de ter o RabbitMQ instalado e em execução.

### 1. Configuração do RabbitMQ

Adicione as configurações do RabbitMQ no arquivo `application.properties`:

```properties
spring.rabbitmq.host=<endereco_do_servidor>
spring.rabbitmq.port=<porta>
spring.rabbitmq.username=<usuario>
spring.rabbitmq.password=<senha>
```

### 2. Configuração de Email

Adicione as configurações do serviço de email no mesmo arquivo `application.properties`:

```properties
email.host=<servidor_smtp>
email.port=<porta>
email.username=<usuario>
email.password=<senha>
email.sender=<email_remetente>
```

### 3. Dependências Maven

Certifique-se de ter as dependências adequadas em seu arquivo `pom.xml`:

```xml
<!-- Dependências do Spring Boot -->
<dependencies>
    <dependency>
        <groupId>org.springframework.boot</groupId>
        <artifactId>spring-boot-starter-amqp</artifactId>
    </dependency>
    <dependency>
        <groupId>org.springframework.boot</groupId>
        <artifactId>spring-boot-starter-mail</artifactId>
    </dependency>
</dependencies>
```

### 4. Código Java

Crie uma classe para representar a mensagem de email:

```java
public class EmailMessage {
    private String to;
    private String subject;
    private String body;

    // getters e setters
}
```

Crie um serviço para lidar com o envio de emails:

```java
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

@Service
public class EmailService {

    @Value("${spring.rabbitmq.exchange}")
    private String exchange;

    @Value("${spring.rabbitmq.routing-key}")
    private String routingKey;

    private final RabbitTemplate rabbitTemplate;

    public EmailService(RabbitTemplate rabbitTemplate) {
        this.rabbitTemplate = rabbitTemplate;
    }

    public void enviarEmail(EmailMessage emailMessage) {
        rabbitTemplate.convertAndSend(exchange, routingKey, emailMessage);
        System.out.println("Email enviado com sucesso!");
    }
}
```

Configure o serviço em sua classe principal (por exemplo, `Application.java`):

```java
import org.springframework.amqp.core.TopicExchange;
import org.springframework.amqp.rabbit.annotation.EnableRabbit;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.amqp.support.converter.Jackson2JsonMessageConverter;
import org.springframework.amqp.support.converter.MessageConverter;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@EnableRabbit
@Configuration
public class RabbitMQConfig {

    @Bean
    public MessageConverter jsonMessageConverter() {
        return new Jackson2JsonMessageConverter();
    }

    @Bean
    public TopicExchange exchange(@Value("${spring.rabbitmq.exchange}") final String exchangeName) {
        return new TopicExchange(exchangeName);
    }

    @Bean
    public RabbitTemplate rabbitTemplate(final RabbitTemplate rabbitTemplate,
                                         final MessageConverter messageConverter,
                                         @Value("${spring.rabbitmq.exchange}") final String exchangeName,
                                         @Value("${spring.rabbitmq.routing-key}") final String routingKey) {
        rabbitTemplate.setMessageConverter(messageConverter);
        rabbitTemplate.setExchange(exchangeName);
        rabbitTemplate.setRoutingKey(routingKey);
        return rabbitTemplate;
    }
}
```

Agora, você pode usar o serviço em seus controladores ou serviços onde precisar enviar emails:

```java
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class EmailController {

    private final EmailService emailService;

    public EmailController(EmailService emailService) {
        this.emailService = emailService;
    }

    @PostMapping("/enviar-email")
    public void enviarEmail(@RequestBody EmailMessage emailMessage) {
        emailService.enviarEmail(emailMessage);
    }
}
```

Lembre-se de ajustar os valores de configuração conforme necessário e adicionar a lógica de tratamento de exceções, logs, etc., conforme a complexidade do seu aplicativo. Este é um exemplo básico para ajudar a começar.

## Contribuições

Contribuições são bem-vindas! Sinta-se à vontade para abrir problemas (issues) ou enviar pull requests para melhorar este serviço de envio de emails.

Esperamos que este serviço facilite o envio de emails em suas aplicações. Se precisar de ajuda ou tiver sugestões, não hesite em entrar em contato.
vgabrielbri@gmail.com
