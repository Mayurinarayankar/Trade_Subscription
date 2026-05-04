namespace WebApplication1.DTOs.Request
{
    public record CreateSubscriptionRequest(
        int CompanyId,
        int PlanConfigId,
        bool IsYearly,
        DateTime StartDate,
        string? Notes
    );

    public record RenewSubscriptionRequest(int SubscriptionId, bool IsYearly);

    public record UpgradeSubscriptionRequest(int SubscriptionId, int NewPlanConfigId, bool IsYearly);
}