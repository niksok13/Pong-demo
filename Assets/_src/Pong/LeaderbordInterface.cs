using System;
using System.Collections;
using System.Collections.Generic;
using NSTools;
using UnityEngine;
using UnityEngine.Networking;

public static class LeaderboardInterface {
    
    [Serializable]
    private class UserData {
        public string uuid;
        public PlayerInfo info;
    }
    
    [Serializable]
    public struct PlayerInfo {
        public string name;
        public long score;
    }

    [Serializable]
    private struct LeaderboardPayload{
        public UserData data;

    }

    [Serializable]
    struct LeaderboardResponse{
        public PlayerInfo[] list;

    }
    private static string url = "https://us-central1-com-niks-common.cloudfunctions.net/{0}";

    [RuntimeInitializeOnLoadMethod]
    public static void Init(){
        EventManager.Bind("FetchLeaderboard",FetchLeaderboard);
    }
    public static void SubmitScore(TimeSpan newScore){
        var payload = new LeaderboardPayload{
            data = new UserData{
                uuid = PrefsManager.UUID,
                info = new PlayerInfo{
                    name = PrefsManager.PlayerName,
                    score = newScore.Ticks
                }
            }
        };
        var request = UnityWebRequest.Put(url.Format("updateUserdata"), JsonUtility.ToJson(payload));
        request.SetRequestHeader("Content-Type","application/json");
        request.SendWebRequest().completed += resp=>{        
            var response = JsonUtility.FromJson<LeaderboardResponse>(request.downloadHandler.text);
            Model.Set("leaderboard_data",response.list);
        };
    }

    public static void FetchLeaderboard(object arg = null){
        var request = UnityWebRequest.Get(url.Format("getLeaderboard"));
        request.SetRequestHeader("Content-Type","application/json");
        request.SendWebRequest().completed += resp=>{
            var response = JsonUtility.FromJson<LeaderboardResponse>(request.downloadHandler.text);
            Model.Set("leaderboard_data",response.list);
        };
    }

}