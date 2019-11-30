
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{ 
   public static void SavePlayer(SAVELOAD saveload)
    {
        Debug.Log("Saved");
        string path = Application.persistentDataPath + "/Player.Save";
        
        string pathOld = Application.persistentDataPath + "/PlayerOLD.Save";
        string pathOld2 = Application.persistentDataPath + "/PlayerOLD2.Save";
        //Если находит сохраненный файл, то переименовывает его в old версию, для отката сохранения
        //Удаляет старый сейв, если он есть

        if (File.Exists(pathOld2)) File.Delete(pathOld2);
        if (File.Exists(pathOld))File.Move(pathOld, pathOld2);
        if (File.Exists(path)) File.Move(path, pathOld);
        
        //Генерит новый актуальный сейвфайл
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path,FileMode.Create);
        PlayerData data = new PlayerData(saveload);
        formatter.Serialize(stream, data);
        stream.Close();

    }
  
    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/Player.Save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
           // Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
