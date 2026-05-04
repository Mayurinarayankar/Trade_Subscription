namespace WebApplication1.DTOs.Request
{
public record CreateIncotermRequest(
    string Code,
    string Name,
    string Description,
    int Type);
}