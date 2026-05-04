namespace WebApplication1.DTOs.Request
{
    public record CreateCompanyRequest(
    string Name,
    string Email,
    string Phone,
    string Address,
    string Country,
    string TaxNumber);
}