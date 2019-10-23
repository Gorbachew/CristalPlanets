using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisplayPlayers : MonoBehaviour
{
    static string[] namesCicle = { "", "K", "M", "B", "T", "q", "Q", "s", "S", "O" };
    private DatabaseReference reference;
    private Text FirstName, FirstScore, SecondName, SecondScore, ThirdName, ThirdScore;
    private Transform Content;
    private void Awake()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cristalplanets.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirstName = transform.Find("1st/NameWind/Name").GetComponent<Text>();
        FirstScore = transform.Find("1st/ScoreWind/Score").GetComponent<Text>();
        SecondName = transform.Find("2nd/NameWind/Name").GetComponent<Text>();
        SecondScore = transform.Find("2nd/ScoreWind/Score").GetComponent<Text>();
        ThirdName = transform.Find("3rd/NameWind/Name").GetComponent<Text>();
        ThirdScore = transform.Find("3rd/ScoreWind/Score").GetComponent<Text>();
        Content = GameObject.Find("Content").transform;
    }
    void Start()
    {
        LoadPlayers();
        
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
                        FirstScore.text = ConvertPrice(long.Parse(Player.Child("GC").Value.ToString())).ToString();
                        break;
                    case 2:
                        SecondName.text = Player.Child("Name").Value.ToString();
                        SecondScore.text = ConvertPrice(long.Parse(Player.Child("GC").Value.ToString())).ToString();
                        break;
                    case 3:
                        ThirdName.text = Player.Child("Name").Value.ToString();
                        ThirdScore.text = ConvertPrice(long.Parse(Player.Child("GC").Value.ToString())).ToString();
                        break;

                }
            }
            else
            {
                GameObject Panel = Instantiate(Resources.Load("Prefab/RatingPanel"), Content) as GameObject;
                Panel.transform.SetSiblingIndex(0);

                Panel.transform.Find("NameWind/Name").GetComponent<Text>().text = Player.Child("Name").Value.ToString();
                Panel.transform.Find("ScoreWind/Score").GetComponent<Text>().text = ConvertPrice(long.Parse(Player.Child("GC").Value.ToString())).ToString();
            }
            count -= 1;

        }
    }



    private void LoadPlayers()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("Users")
            .OrderByChild("GC")
            .ValueChanged += HandleValueChanged;
        
        
    }

    private void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (SceneManager.GetActiveScene().name == "Rating")
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }
            Content.DetachChildren();
            Display(args.Snapshot);
        }
    }

    public string ConvertPrice(long value)
    {
        string price = value.ToString();
        int cycles = 0;
        if (price.Length > 4)
        {
            while (price.Length > 6)
            {
                cycles += 1;
                price = price.Remove(price.Length - 3);
            }
            string balance = price.Substring(price.Length - 3);
            price = price.Remove(price.Length - 3) + "." + balance.Remove(balance.Length - 2) + namesCicle[cycles + 1];
        }
        return price;
    }
}
