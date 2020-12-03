using NSTools;
using UnityEngine;

namespace States
{
    public class SSettings: FSM.IState
    {
        public FSM.IState Enter()
        {
            Model.Set("window_settings_visible", true);
            return null;
        }
        
        public FSM.IState Signal(string name, object arg)
        {
            switch (name)
            {
                case "btn_back": 
                    EventManager.Invoke("sound_play","pop");
                    return new SMenu();
            }
            
            return null;
        }



        public FSM.IState Exit()
        {
            //Apply settings
            PrefsManager.Difficulty = Model.Get("difficulty", 0.2f);
            PrefsManager.Volume = Model.Get("sound_volume", 1f);
            PrefsManager.PlayerName = Model.Get("player_name", "ABC");
            //Close window
            Model.Set("window_settings_visible", false);
            return null;
        }
    }
}