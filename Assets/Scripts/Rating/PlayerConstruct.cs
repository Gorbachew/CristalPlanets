using System;
using System.Collections.Generic;

public class PlayerConstruct
{
    public string Name,DateReg,DateUpdate;
    public long GC;
    public PlayerConstruct()
    {

    }

    public PlayerConstruct(string Name,long GC, string DateReg, string DateUpdate)
    {
        this.Name = Name;
        this.GC = GC;
        this.DateReg = DateReg;
        this.DateUpdate = DateUpdate;

    }
    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        result["Name"] = Name;
        result["GC"] = GC;
        result["DateReg"] = DateReg;
        result["DateUpdate"] = DateUpdate;
     
        return result;
    }

}
