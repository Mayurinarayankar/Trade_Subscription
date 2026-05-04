namespace WebApplication1.DTOs.Response
{
    public record CompanyResponse(
    int Id,
    string Name,
    string Email,
    string Phone,
    string Address,
    string Country,
    string TaxNumber,
    bool IsActive,
    DateTime CreatedAt,
    ActiveSubscriptionSummary? ActiveSubscription
);
 
public record ActiveSubscriptionSummary(
    int Id,
    string PlanName,
    string Status,
    DateTime EndDate,
    int DaysRemaining
);
 
}