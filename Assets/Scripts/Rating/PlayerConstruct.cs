
using System.Collections.Generic;

public class PlayerConstruct
{
    public string Name;
    public long GC;

    public PlayerConstruct()
    {

    }

    public PlayerConstruct(string Name,long GC)
    {
        this.Name = Name;
        this.GC = GC;
       

    }
    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        result["Name"] = Name;
        result["GC"] = GC;
        return result;
    }
}
