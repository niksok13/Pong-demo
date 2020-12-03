using System;
using UnityEngine;

namespace NSTools
{
    public static class PrefsManager
    {
        public static float Difficulty
        {
            get => PlayerPrefs.GetFloat("difficulty",0.2f);
            set
            {
                PlayerPrefs.SetFloat("difficulty", value);
                PlayerPrefs.Save();
            }
        }
        public static float Volume
        {
            get => PlayerPrefs.GetFloat("sound_volume",1f);
            set
            {
                PlayerPrefs.SetFloat("sound_volume", value);
                PlayerPrefs.Save();
            }
        }
        public static string PlayerName
        {
            get => PlayerPrefs.GetString("player_name","ABC");
            set
            {
                PlayerPrefs.SetString("player_name", value);
                PlayerPrefs.Save();
            }
        }
        public static string UUID
        {
            get
            {
                string result = PlayerPrefs.GetString("uuid","");
                if(string.IsNullOrEmpty(result)){
                    result = Guid.NewGuid().ToString();
                    Log.Debug(result);
                    PlayerPrefs.SetString("uuid", result);
                    PlayerPrefs.Save();
                }
                return result;
            }
        }
        public static float BestTime
        {
            get => PlayerPrefs.GetFloat("best_time", 0f);
            set
            {
                PlayerPrefs.SetFloat("best_time", value);
                PlayerPrefs.Save();
            }
        }
    }
}