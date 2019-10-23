using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections.Generic;
using UnityEngine;

public class SAVELOAD : MonoBehaviour
{
    
    private string GC, Name, Planet1;
    DatabaseReference reference;
    
    private void Awake()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cristalplanets.firebaseio.com/");
        DontDestroyOnLoad(this);
        if (FindObjectsOfType(GetType()).Length > 1) Destroy(gameObject);
        LoadPlayer();
    }
    public void ChangeGC(char Sign, long value)
    {
        switch (Sign)
        {
            case '+':
                GC = (long.Parse(GC) + value).ToString();
                break;
            case '-':
                GC = (long.Parse(GC) - value).ToString();
                break;
        }
    }
    public string ShowInfo(string value)
    {
        switch (value)
        {
            case "GC":
                return GC;
            case "Name":
                return Name;
            case "Planet1":
                return Planet1;
        }
        return "Error";
    }
    public void CollectionData(string Var,string value)
    {
        switch (Var)
        {
            case "Name":
                Name = value;
                break;
            case "Planet1":
                Planet1 = value;
                break;
        }
        SavePlayer();
    }
    private void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }
    public void LoadPlayer()
    {
        
        PlayerData data = SaveSystem.LoadPlayer();
        if(data == null)
        {
            //Если файл удален, или игрок заходит в 1-й раз, то это изначальные статы
            GC = "0";
            Name = "Anonim";
            Planet1 = "15/0/1/1/1/1/1.1.0.0.0.0";  
        }
        else
        {
            GC = data.GC;
            Name = data.Name;
            Planet1 = data.Planet1;
        }
    }
    public void UpdateScore()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        PlayerConstruct PC = new PlayerConstruct(Name,long.Parse(GC));
        Dictionary<string, object> Values = PC.ToDictionary();
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates["/Users/" + Name] = Values;
        reference.UpdateChildrenAsync(childUpdates);
    }


}
