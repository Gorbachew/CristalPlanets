using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddUser : MonoBehaviour
{
    SAVELOAD SL;
    InputField inputName;
    DatabaseReference reference;
    GameObject Busy;
    private string Check;
    private void Awake()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cristalplanets.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        inputName = gameObject.GetComponentInChildren<InputField>();
        SL = GameObject.Find("SL").GetComponent<SAVELOAD>(); 
        Busy = gameObject.transform.Find("Busy").gameObject;
    }
    private void Start()
    {
        Busy.SetActive(false);
        if (SL.ShowInfo("Name") != "Anonim") gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }
    


    private void FixedUpdate()
    {
        switch (Check)
        {
            case "Busy":
                Busy.SetActive(true);
                break;
            case "Free":
                string name = inputName.text;
                SL.CollectionData("Name", name);
                //Добавлят в firebase базу
                SL.UpdateScore();

                gameObject.SetActive(false);
                break;
        }
        Check = null;
    }

    public void CheckName()
    {
        gameObject.GetComponent<AudioSource>().Play();
        FirebaseDatabase.DefaultInstance
        .GetReference("Users")
        .OrderByKey()
        .EqualTo(inputName.text)
        .LimitToFirst(53)
        .GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("Error");
            }
            else if (task.IsCompleted)
            {
                
                DataSnapshot snapshot = task.Result;
                if (snapshot.ChildrenCount == 1 || inputName.text == "" || inputName.text == null)
                {
                    Check = "Busy";
                   
                }
                else if (snapshot.ChildrenCount == 0)
                {
                    Check = "Free";
                }
                //AddNewUser(snapshot);
            }

        });
    }

}
