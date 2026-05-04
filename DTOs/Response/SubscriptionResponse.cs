namespace WebApplication1.DTOs.Response
{
    public record SubscriptionResponse(
    int Id,
    int CompanyId,
    string CompanyName,
    int PlanConfigId,
    string PlanName,
    string Status,
    DateTime StartDate,
    DateTime EndDate,
    bool IsYearly,
    decimal AmountPaid,
    DateTime? LastRenewalDate,
    DateTime? NextRenewalDate,
    string? Notes,
    DateTime CreatedAt
);
 
public record SubscriptionPlanResponse(
    int Id,
    string Name,
    string Plan,
    decimal MonthlyPrice,
    decimal YearlyPrice,
    int MaxUsers,
    int MaxTrades,
    List<string> Features,
    bool IsActive
);
 
}