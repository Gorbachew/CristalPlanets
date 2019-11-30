using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AddUser : MonoBehaviour
{
   
    InputField inputName;
    GameObject Busy;
    SceneManage SM;
    private Button btnOpenRegPanel;
    [SerializeField]
    private bool Check,NameIsBusy;

    private void Awake()
    {
        SM = GameObject.Find("MainCamera").GetComponent<SceneManage>();
    }

    public void Load()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://crystal-planets-fe735.firebaseio.com/");
        btnOpenRegPanel = SM.CanvasRating.transform.Find("Top/Reg").GetComponent<Button>();
        Busy = SM.objRegTableCont.transform.Find("Busy").gameObject;
        inputName = SM.objRegTableCont.GetComponentInChildren<InputField>();


        Busy.SetActive(false);
        if (SM.SL.ShowInfo("Name") == "Anonim") btnOpenRegPanel.interactable = true;
        else btnOpenRegPanel.interactable = false;
        //SM.table.Reg();
        
    }
    private void Add(string name)
    {       
        SM.SL.CollectionData("Name", name);
        //Добавлят в firebase базу
        SM.SL.RegScore();
        SM.objTable.SetActive(false);
    }
   public IEnumerator WaitCheck()
    {
        string name = inputName.text;
        CheckName(name);
        Busy.SetActive(false);
        yield return new WaitUntil(() => Check);
        if (NameIsBusy)
        {
            Debug.Log("Занято");
            Busy.SetActive(true);
        }
        else
        {
            Debug.Log("Свободно");
            Add(name);
        }
        Check = false;
        NameIsBusy = false;

    }
    public void CheckName(string name)
    {

        FirebaseDatabase.DefaultInstance
        .GetReference("Users")
        .OrderByKey()
        .EqualTo(name)
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
                    Check = true;
                    NameIsBusy = true;
                }
                else if (snapshot.ChildrenCount == 0)
                {
                    Check = true;
                    NameIsBusy = false;
                }
            }

        });
    }
}
