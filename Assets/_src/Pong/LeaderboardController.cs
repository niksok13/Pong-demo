using System;
using NSTools;
using UnityEngine;
using static LeaderboardInterface;

public class LeaderboardController : MonoBehaviour
{
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        Model.Bind("leaderboard_data", RefreshLeaderboard);        
    }

    private void RefreshLeaderboard(object obj)
    {
        foreach (Transform item in transform)
            Destroy(item.gameObject);

        var data = Model.Get("leaderboard_data",new PlayerInfo[0]);
        for (int i = 0; i < data.Length; i++)
        {
            var item = data[i];
            var entry = Instantiate(prefab,transform);
            entry.GetComponent<LeaderboardItem>().Init(i+1,item.name, new TimeSpan(item.score));            
        }
    }
}
