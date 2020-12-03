using System;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardItem : MonoBehaviour
{
    public Text labelPos, labelName, labelScore;
    // Start is called before the first frame update
    public void Init(int index, string name, TimeSpan score)
    {
        labelPos.text = index.ToString();
        if (name.Length>15){
            name = name.Substring(1,15)+("...");
        }
        labelName.text = name;
        labelScore.text = score.ToString(@"m\:ss\:fff");
    }
}
