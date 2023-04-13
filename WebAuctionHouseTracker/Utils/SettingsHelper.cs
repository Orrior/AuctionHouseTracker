namespace WebApplication1.Utils;

public class SettingsHelper
{
    public static List<string> parseRealms(string s)
    {
        var sarr = s.Trim().Split(",");
        List<string> result = new List<string>();

        long ignore = 0;
        
        foreach (var i in sarr)
        {
            if (i != "" && Int64.TryParse(i, out ignore))
            {
                result.Add(i);
            }
        }

        return result;
    }
}