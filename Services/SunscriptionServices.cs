using WebApplication1.Application.Interfaces;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using WebApplication1.DTOs.Response.Common;
using WebApplication1.enums;
using WebApplication1.Models;
using WebApplication1.Interfaces.Common;
using System.Text.Json;

namespace  WebApplication1.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IUnitOfWork _uow;

    public SubscriptionService(IUnitOfWork uow) => _uow = uow;

    public async Task<ApiResponse<IEnumerable<SubscriptionPlanResponse>>> GetPlansAsync()
    {
        var plans = await _uow.SubscriptionPlans.GetActiveAsync();
        return new ApiResponse<IEnumerable<SubscriptionPlanResponse>>(true, "Success.", plans.Select(MapPlanResponse));
    }

    public async Task<ApiResponse<SubscriptionResponse>> GetByIdAsync(int id)
    {
        var sub = await _uow.Subscriptions.GetByIdAsync(id);
        if (sub == null) return new ApiResponse<SubscriptionResponse>(false, "Subscription not found.", null);
        return new ApiResponse<SubscriptionResponse>(true, "Success.", MapResponse(sub));
    }

    public async Task<ApiResponse<IEnumerable<SubscriptionResponse>>> GetByCompanyAsync(int companyId)
    {
        var subs = await _uow.Subscriptions.GetByCompanyAsync(companyId);
        return new ApiResponse<IEnumerable<SubscriptionResponse>>(true, "Success.", subs.Select(MapResponse));
    }

    public async Task<ApiResponse<SubscriptionResponse>> GetActiveSubscriptionAsync(int companyId)
    {
        var sub = await _uow.Subscriptions.GetActiveSubscriptionAsync(companyId);
        if (sub == null) return new ApiResponse<SubscriptionResponse>(false, "No active subscription found.", null);
        return new ApiResponse<SubscriptionResponse>(true, "Success.", MapResponse(sub));
    }

    public async Task<ApiResponse<SubscriptionResponse>> CreateAsync(CreateSubscriptionRequest request)
    {
        var plan = await _uow.SubscriptionPlans.GetByIdAsync(request.PlanConfigId);
        if (plan == null) return new ApiResponse<SubscriptionResponse>(false, "Plan not found.", null);

        var company = await _uow.Companies.GetByIdAsync(request.CompanyId);
        if (company == null) return new ApiResponse<SubscriptionResponse>(false, "Company not found.", null);

        var price = request.IsYearly ? plan.YearlyPrice : plan.MonthlyPrice;
        var endDate = request.IsYearly
            ? request.StartDate.AddYears(1)
            : request.StartDate.AddMonths(1);

        var sub = new Subscription
        {
            CompanyId = request.CompanyId,
            PlanConfigId = request.PlanConfigId,
            Status = SubscriptionStatus.Active,
            StartDate = request.StartDate,
            EndDate = endDate,
            IsYearly = request.IsYearly,
            AmountPaid = price,
            LastRenewalDate = request.StartDate,
            NextRenewalDate = endDate,
            Notes = request.Notes
        };

        await _uow.Subscriptions.AddAsync(sub);
        await _uow.SaveChangesAsync();

        sub.Company = company;
        sub.PlanConfig = plan;

        return new ApiResponse<SubscriptionResponse>(true, "Subscription created successfully.", MapResponse(sub));
    }

    public async Task<ApiResponse<SubscriptionResponse>> RenewAsync(RenewSubscriptionRequest request)
    {
        var sub = await _uow.Subscriptions.GetByIdAsync(request.SubscriptionId);
        if (sub == null) return new ApiResponse<SubscriptionResponse>(false, "Subscription not found.", null);

        var price = request.IsYearly ? sub.PlanConfig.YearlyPrice : sub.PlanConfig.MonthlyPrice;
        var newEnd = request.IsYearly ? sub.EndDate.AddYears(1) : sub.EndDate.AddMonths(1);

        sub.Status = SubscriptionStatus.Active;
        sub.LastRenewalDate = DateTime.UtcNow;
        sub.EndDate = newEnd;
        sub.NextRenewalDate = newEnd;
        sub.IsYearly = request.IsYearly;
        sub.AmountPaid += price;
        sub.UpdatedAt = DateTime.UtcNow;

        await _uow.Subscriptions.UpdateAsync(sub);
        await _uow.SaveChangesAsync();

        return new ApiResponse<SubscriptionResponse>(true, "Subscription renewed successfully.", MapResponse(sub));
    }

    public async Task<ApiResponse<SubscriptionResponse>> UpgradeAsync(UpgradeSubscriptionRequest request)
    {
        var sub = await _uow.Subscriptions.GetByIdAsync(request.SubscriptionId);
        if (sub == null) return new ApiResponse<SubscriptionResponse>(false, "Subscription not found.", null);

        var newPlan = await _uow.SubscriptionPlans.GetByIdAsync(request.NewPlanConfigId);
        if (newPlan == null) return new ApiResponse<SubscriptionResponse>(false, "Plan not found.", null);

        sub.PlanConfigId = request.NewPlanConfigId;
        sub.IsYearly = request.IsYearly;
        sub.AmountPaid += request.IsYearly ? newPlan.YearlyPrice : newPlan.MonthlyPrice;
        sub.UpdatedAt = DateTime.UtcNow;

        await _uow.Subscriptions.UpdateAsync(sub);
        await _uow.SaveChangesAsync();

        return new ApiResponse<SubscriptionResponse>(true, "Subscription upgraded successfully.", MapResponse(sub));
    }

    public async Task<ApiResponse<string>> CancelAsync(int id)
    {
        var sub = await _uow.Subscriptions.GetByIdAsync(id);
        if (sub == null) return new ApiResponse<string>(false, "Subscription not found.", null);

        sub.Status = SubscriptionStatus.Cancelled;
        sub.UpdatedAt = DateTime.UtcNow;
        await _uow.Subscriptions.UpdateAsync(sub);
        await _uow.SaveChangesAsync();

        return new ApiResponse<string>(true, "Subscription cancelled.", null);
    }

    private SubscriptionResponse MapResponse(Subscription s) => new(
        s.Id, s.CompanyId, s.Company?.Name ?? "",
        s.PlanConfigId, s.PlanConfig?.Name ?? "",
        s.Status.ToString(), s.StartDate, s.EndDate,
        s.IsYearly, s.AmountPaid, s.LastRenewalDate,
        s.NextRenewalDate, s.Notes, s.CreatedAt
    );

    private SubscriptionPlanResponse MapPlanResponse(SubscriptionPlanConfig p)
    {
        var features = new List<string>();
        try { features = JsonSerializer.Deserialize<List<string>>(p.Features) ?? new(); } catch { }
        return new(p.Id, p.Name, p.Plan.ToString(), p.MonthlyPrice, p.YearlyPrice,
                   p.MaxUsers, p.MaxTrades, features, p.IsActive);
    }
}