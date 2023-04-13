namespace WebApplication1.Models.Web;

public class RealmsModel
{
    public bool RealmsAreSet { get; set; }
    public Dictionary<string, string> Realms { get; set; } = new Dictionary<string, string>();
}