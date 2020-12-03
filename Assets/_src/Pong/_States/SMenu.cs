using NSTools;

namespace States
{
    public class SMenu : FSM.IState
    {
        public FSM.IState Enter()
        {
            Model.Set("leaderboard_visible", false);
            Model.Set("window_menu_visible", true);
            return null;
        }

        
        public FSM.IState Signal(string name, object arg)
        {
            
            EventManager.Invoke("sound_play","pop");
            switch (name)
            {
                case "btn_play_pvp":
                    Model.Set("enemy_mode", PlayerMode.Player);
                    return new SReady();
                
                case "btn_play_ranked":
                    Model.Set("leaderboard_visible", true);
                    Model.Set("enemy_mode", PlayerMode.Ranked);
                    return new SReady();
                
                case "btn_play_pve":                     
                    Model.Set("enemy_mode", PlayerMode.Bot);
                    return new SReady();
                
                case "btn_play_mirror":                     
                    Model.Set("enemy_mode", PlayerMode.Mirror);
                    return new SReady();
                
                case "btn_settings": 
                    return new SSettings();
            }
            return null;
        }

        public FSM.IState Exit()
        {
            Model.Set("window_menu_visible", false);
            return null;
        }
    }
}