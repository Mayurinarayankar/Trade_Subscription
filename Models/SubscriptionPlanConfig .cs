using WebApplication1.enums;

namespace WebApplication1.Models
{
    public class SubscriptionPlanConfig : BaseEntity
    {public string Name { get; set; } = string.Empty;
    public SubscriptionPlan Plan { get; set; }
    public decimal MonthlyPrice { get; set; }
    public decimal YearlyPrice { get; set; }
    public int MaxUsers { get; set; }
    public int MaxTrades { get; set; }
    public string Features { get; set; } = string.Empty; // JSON list of features
    public bool IsActive { get; set; } = true;
 
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}