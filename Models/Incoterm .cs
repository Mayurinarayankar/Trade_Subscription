using WebApplication1.enums;

namespace WebApplication1.Models
{
  public class Incoterm : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IncotermsType Type { get; set; }
    public bool IsActive { get; set; } = true;
 
    public ICollection<Trade> Trades { get; set; } = new List<Trade>();
}
}