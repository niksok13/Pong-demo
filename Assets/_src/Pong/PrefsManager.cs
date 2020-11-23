using System;
using UnityEngine;

namespace NSTools
{
    public static class PrefsManager
    {
        public static Color BallColor
        {
            get
            {
                var str = PlayerPrefs.GetString("ball_color");
                
                var color = string.IsNullOrEmpty(str)?
                    Color.cyan:
                    JsonUtility.FromJson<Color>(str);
                
                return color;
            }
            set
            {
                var str = JsonUtility.ToJson(value);
                PlayerPrefs.SetString("ball_color",str);
                PlayerPrefs.Save();
            }
        }

        public static TimeSpan BestTime
        {
            get
            {
                var str = PlayerPrefs.GetString("best_time","0");
                return new TimeSpan(long.Parse(str));
            }
            set
            {
                PlayerPrefs.SetString("best_time", value.Ticks.ToString());
                PlayerPrefs.Save();
            }
        }
    }
}