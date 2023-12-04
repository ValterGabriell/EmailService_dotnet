namespace EmailService.Dto;

public class MessageInputEmail
{
    public string from { get; set; }
    public string to { get; set; }
    public string body { get; set; }
    public string subject { get; set; }
}