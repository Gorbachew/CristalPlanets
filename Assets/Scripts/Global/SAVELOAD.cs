using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SAVELOAD : MonoBehaviour
{
    [SerializeField]
    private string GC, Name, Planet0;
    private Text GCtext,NameText;
    private SceneManage SM;
    DatabaseReference reference;

    private void Awake()
    {
        Debug.Log("Awake SAVELOAD");
        SM = GameObject.Find("MainCamera").GetComponent<SceneManage>();
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cristalplanets.firebaseio.com/");
        GCtext = GameObject.Find("/CanvasUI/CanvasSpace/GC/Canvas/Text").GetComponent<Text>();
        NameText = GameObject.Find("/CanvasUI/CanvasSpace/Name/Canvas/Text").GetComponent<Text>();
    }
    public void Start()
    {
        LoadPlayer();
        LoadInfo();
    }
    public void LoadInfo()
    {
        GCtext.text = SM.Score.ConvertPrice(long.Parse(GC));
        NameText.text = Name;
    }
    public void LoadPlayer()
    {
        
        PlayerData data = SaveSystem.LoadPlayer();
        if (data == null)
        {
            //Если файл удален, или игрок заходит в 1-й раз, то это изначальные статы
            GC = "0";
            Name = "Anonim";
            Planet0 = "15/0/1/1/1.0/1/2.1.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0/0.0.0.0.0.0";
            Debug.Log("Загрузка заново");
        }
        else
        {
            GC = data.GC;
            Name = data.Name;
            Planet0 = data.Planet0;
            Debug.Log("Загрузка продолжения");
        }
    }

    public void RefreshPlayer()
    {
        string path = Application.persistentDataPath + "/Player.Save";
        string pathOld = Application.persistentDataPath + "/PlayerOLD.Save";
        string pathOld2 = Application.persistentDataPath + "/PlayerOLD2.Save";
        if (File.Exists(pathOld2)) File.Delete(pathOld2);
        if (File.Exists(pathOld)) File.Delete(pathOld);
        if (File.Exists(path)) File.Delete(path);

        LoadPlayer();

    }

    public void RollBackPlayer()
    {
        string path = Application.persistentDataPath + "/Player.Save";
        string pathOld = Application.persistentDataPath + "/PlayerOLD.Save";
        string pathOld2 = Application.persistentDataPath + "/PlayerOLD2.Save";
        if (File.Exists(pathOld2))
        {
            //При проверке, юзер может перезаписать 2-й файл и в итоге будет 2 ошибочных сохранения
            if (File.Exists(path)) File.Delete(path);
            if (File.Exists(pathOld)) File.Delete(pathOld);
            File.Move(pathOld2, path);

            LoadPlayer();

        }
        else SM.ErrorSource.Play();
    }
    private void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void ChangeGC(char Sign, long value)
    {
        long gc = 0;
        switch (Sign)
        {
            case '+':
                gc = long.Parse(GC) + value;
                break;
            case '-':
                gc = long.Parse(GC) - value;
                break;

        }
        GC = gc.ToString();
        GCtext.text = SM.Score.ConvertPrice(gc);
        SavePlayer();
    }
    public string ShowInfo(string value)
    {
        switch (value)
        {
            case "GC":
                return GC;
            case "Name":
                return Name;
            case "Planet0":
                return Planet0;
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
            case "Planet0":
                Planet0 = value;
                break;
        }
        SavePlayer();
    }
    
    public void RegScore()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        string time = DateTime.UtcNow.AddHours(3).ToString();
        PlayerConstruct PC = new PlayerConstruct(Name, long.Parse(GC), time, time);
        Dictionary<string, object> Values = PC.ToDictionary();
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates["/Users/" + Name] = Values;
        reference.UpdateChildrenAsync(childUpdates);
    }
    
    public void UpdateScore()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("Users").Child(SM.SL.Name).Child("GC").SetValueAsync(long.Parse(SM.SL.GC));
        reference.Child("Users").Child(SM.SL.Name).Child("DateUpdate").SetValueAsync(DateTime.UtcNow.AddHours(3).ToString());

    }

}
