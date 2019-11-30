
[System.Serializable]
public class PlayerData
{
    public string Name;
    public string GC;
    public string Planet0;



    public PlayerData(SAVELOAD saveload)
    {
        GC = saveload.ShowInfo("GC");
        Name = saveload.ShowInfo("Name");
        Planet0 = saveload.ShowInfo("Planet0");
    }
}
    