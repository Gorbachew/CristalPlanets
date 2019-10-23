
[System.Serializable]
public class PlayerData
{
    public string Name;
    public string GC;
    public string Planet1;



    public PlayerData(SAVELOAD saveload)
    {
        GC = saveload.ShowInfo("GC");
        Name = saveload.ShowInfo("Name");
        Planet1 = saveload.ShowInfo("Planet1");
    }
}
    