namespace EmailService.Dto;

public class EmailDto
{
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;

    public void ValidatingFields()
    {
        if (this.Body == string.Empty)
        {
            throw new HttpRequestException("Mensagem incompleta. Body faltando");
        }

        if (this.Subject == string.Empty)
        {
            throw new HttpRequestException("Mensagem incompleta. Subject faltando");
        }

        if (this.To == string.Empty)
        {
            throw new HttpRequestException("Mensagem incompleta. To faltando");
        }
    }
}