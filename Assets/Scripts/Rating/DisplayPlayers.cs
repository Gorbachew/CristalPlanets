
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayers : MonoBehaviour
{
    private DatabaseReference reference;
    private Text FirstName, FirstScore, SecondName, SecondScore, ThirdName, ThirdScore;
    private SceneManage SM;

    private void Awake()
    {
        SM = GameObject.Find("MainCamera").GetComponent<SceneManage>();
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://crystal-planets-fe735.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirstName = transform.Find("1st/NameWind/Name").GetComponent<Text>();
        FirstScore = transform.Find("1st/ScoreWind/Score").GetComponent<Text>();
        SecondName = transform.Find("2nd/NameWind/Name").GetComponent<Text>();
        SecondScore = transform.Find("2nd/ScoreWind/Score").GetComponent<Text>();
        ThirdName = transform.Find("3rd/NameWind/Name").GetComponent<Text>();
        ThirdScore = transform.Find("3rd/ScoreWind/Score").GetComponent<Text>();

    }
  

    private void Display(DataSnapshot Data)
    {
        long count = Data.ChildrenCount;
        
        foreach (DataSnapshot Player in Data.Children)
        {
            if (count <= 3)
            {
                switch (count)
                {
                    case 1:
                        FirstName.text = Player.Child("Name").Value.ToString();
                        FirstScore.text = SM.Score.ConvertPrice(long.Parse(Player.Child("GC").Value.ToString())).ToString();
                        break;
                    case 2:
                        SecondName.text = Player.Child("Name").Value.ToString();
                        SecondScore.text = SM.Score.ConvertPrice(long.Parse(Player.Child("GC").Value.ToString())).ToString();
                        break;
                    case 3:
                        ThirdName.text = Player.Child("Name").Value.ToString();
                        ThirdScore.text = SM.Score.ConvertPrice(long.Parse(Player.Child("GC").Value.ToString())).ToString();
                        break;

                }
            }
            else
            {
                GameObject Panel = Instantiate(Resources.Load("Prefab/RatingPanel"), SM.ContentRating) as GameObject;
                Panel.transform.SetSiblingIndex(0);

                Panel.transform.Find("NameWind/Name").GetComponent<Text>().text = Player.Child("Name").Value.ToString();
                Panel.transform.Find("ScoreWind/Score").GetComponent<Text>().text = SM.Score.ConvertPrice(long.Parse(Player.Child("GC").Value.ToString())).ToString();
            }
            count -= 1;

        }
    }



    public void LoadPlayers()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("Users")
            .OrderByChild("GC")
            .ValueChanged += HandleValueChanged;
    }

    private void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (SM.Rating.activeSelf)
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }
            SM.ContentRating.DetachChildren();
            Display(args.Snapshot);
        }
    }
}
