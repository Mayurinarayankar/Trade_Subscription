namespace WebApplication1.DTOs.Response
{
   public record IncotermResponse(
    int Id,
    string Code,
    string Name,
    string Description,
    string Type,
    bool IsActive
);
 
}