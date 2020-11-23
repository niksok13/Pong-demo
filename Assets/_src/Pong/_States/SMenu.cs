using NSTools;

namespace States
{
    public class SMenu : FSM.IState
    {
        public string Enter()
        {
            Model.Set("window_menu_visible", true);
            return "";
        }

        
        public string Signal(string name, object arg)
        {
            switch (name)
            {
                case "btn_play_pvp":
                    Model.Set("enemy_mode", PlayerMode.Player);
                    return "ready";
                
                case "btn_play_pve":                     
                    Model.Set("enemy_mode", PlayerMode.Bot);
                    return "ready";
                
                case "btn_play_mirror":                     
                    Model.Set("enemy_mode", PlayerMode.Mirror);
                    return "ready";
                
                case "btn_play_online":                     
                    Model.Set("enemy_mode", PlayerMode.Online);
                    return "online_join";
                
                case "btn_settings": return "settings";
            }
            return "";
        }

        public string Exit()
        {
            Model.Set("window_menu_visible", false);
            return "";
        }
    }
}