using NSTools;
using UnityEngine;

namespace States
{
    public class SSettings: FSM.IState
    {
        public string Enter()
        {
            Model.Set("window_settings_visible", true);
            return "";
        }
        
        public string Signal(string name, object arg)
        {
            switch (name)
            {
                case "btn_back": return "menu";
                case "set_ball_color": return SetBallColor((Color)arg);
            }
            
            return "";
        }

        private string SetBallColor(Color col)
        {
            Model.Set("ball_color",col);
            return "";
        }


        public string Exit()
        {
            //Apply settings
            PrefsManager.BallColor = Model.Get("ball_color", Color.white);

            //Close window
            Model.Set("window_settings_visible", false);
            return "";
        }
    }
}