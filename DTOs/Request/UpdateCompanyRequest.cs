namespace WebApplication1.DTOs.Request
{
    public record UpdateCompanyRequest(
    string Name,
    string Phone,
    string Address,
    string Country,
    string TaxNumber);
}   