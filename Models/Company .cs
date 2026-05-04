namespace WebApplication1.Models
{
    public class Company : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string TaxNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<Trade> Trades { get; set; } = new List<Trade>();
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}