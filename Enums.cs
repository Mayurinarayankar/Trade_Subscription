namespace WebApplication1.enums
{
    public enum SubscriptionStatus
    {
        Active = 1,
        Inactive = 2,
        Expired = 3,
        Cancelled = 4,
        Trial = 5
    }

    public enum SubscriptionPlan
    {
        Basic = 1,
        Standard = 2,
        Premium = 3,
        Enterprise = 4
    }

    public enum InvoiceStatus
    {
        Draft = 1,
        Sent = 2,
        Paid = 3,
        Overdue = 4,
        Cancelled = 5
    }

    public enum TradeStatus
    {
        Pending = 1,
        InProgress = 2,
        Completed = 3,
        Cancelled = 4,
        Disputed = 5
    }

    public enum IncotermsType
    {
        EXW = 1,   // Ex Works
        FCA = 2,   // Free Carrier
        CPT = 3,   // Carriage Paid To
        CIP = 4,   // Carriage and Insurance Paid To
        DAP = 5,   // Delivered at Place
        DPU = 6,   // Delivered at Place Unloaded
        DDP = 7,   // Delivered Duty Paid
        FAS = 8,   // Free Alongside Ship
        FOB = 9,   // Free on Board
        CFR = 10,  // Cost and Freight
        CIF = 11   // Cost Insurance and Freight
    }

    public enum UserRole
    {
        SuperAdmin = 1,
        Admin = 2,
        Manager = 3,
        User = 4
    }
}