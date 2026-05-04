using WebApplication1.enums;

namespace WebApplication1.Models
{
    public class Subscription : BaseEntity
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public int PlanConfigId { get; set; }
        public SubscriptionPlanConfig PlanConfig { get; set; } = null!;

        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Trial;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsYearly { get; set; } = false;
        public decimal AmountPaid { get; set; }
        public DateTime? LastRenewalDate { get; set; }
        public DateTime? NextRenewalDate { get; set; }
        public string? Notes { get; set; }

        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}