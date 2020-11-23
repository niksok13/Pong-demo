using NSTools;

namespace States
{
    public class SOnline : FSM.IState
    {
        public string Enter()
        {
            EventManager.Invoke("join_session");
            Model.Set("btn_back_active", true);
            Model.Set("overlay_play_visible",true);
            Model.Set("gameplay_title","FINDING LOBBY");
            Model.Set("label_time","");
            return ""; 
        }


        public string Signal(string name, object arg)
        {
            switch (name)
            {
                case "match_found":
                    EventManager.Invoke("run_session");
                    return "online_ready";
                
                case "btn_back": 
                    EventManager.Invoke("online_state",NetworkController.GameState.Exit);
                    Model.Set("overlay_play_visible",false);
                    return "menu";
            }

            return "";
        }

        public string Exit()
        {
            Model.Set("btn_back_active", false);
            return "";
        }
    }
    
    /// <summary>
    /// Ready to play 
    /// </summary>
    public class SOnlineReady:FSM.IState
    {
        public string Enter()
        {
            Model.Set("gameplay_title","TOUCH TABLE TO START");
            Model.Set("btn_back_active", true);
            Model.Set("overlay_play_visible",true);
            EventManager.Invoke("reset_game");
            return "";
        }

        
        public string Signal(string name, object arg)
        {
            switch (name)
            {
                case "btn_back": 
                    EventManager.Invoke("online_state",NetworkController.GameState.Exit);
                    Model.Set("overlay_play_visible",false);
                    return "menu";
                
                case "btn_pause": 
                    EventManager.Invoke("online_state",NetworkController.GameState.Play);
                    return "";
                
                case "set_state":
                    return arg.ToString();
            }

            return "";
        }
        
        public string Exit()
        {
            Model.Set("btn_back_active", false);
            return "";
        }
    }

    
    /// <summary>
    /// Playing
    /// </summary>
    public class SOnlinePlay:FSM.IState
    {
        public string Enter()
        {
            Model.Set("game_playing",true);
            Model.Set("gameplay_title","TOUCH TABLE TO PAUSE");
            return "";
        }

        public string Signal(string name, object arg)
        {
            switch (name)
            {
                case "player_win":
                    Model.Set("winner", arg);
                    EventManager.Invoke("online_state",NetworkController.GameState.Result);
                    return "";
                
                case "btn_pause": 
                    EventManager.Invoke("online_state",NetworkController.GameState.Pause);
                    return "";
                
                case "set_state":
                    return arg.ToString();
            }

            return "";
        }

        public string Exit()
        {
            Model.Set("game_playing",false);
            return "";
        }
    }
    
    /// <summary>
    /// Round result
    /// </summary>
    public class SOnlineResult: FSM.IState
    {
        public string Enter()
        {
            Model.Set("btn_back_active",true);
            var winner = Model.Get("winner", false) ? "WIN" : "LOSE";
            Model.Set("gameplay_title",$"YOU {winner}!\n Touch table to continue");
            return "";
        }

        public string Signal(string name, object arg)
        {
            switch (name)
            {
                case "btn_back": 
                    EventManager.Invoke("online_state",NetworkController.GameState.Exit);
                    Model.Set("overlay_play_visible",false);
                    return "";
                
                case "btn_pause":
                    EventManager.Invoke("online_state",NetworkController.GameState.Ready);
                    return "";
                case "set_state":
                    return arg.ToString();
            }
            return "";
        }
        
        public string Exit()
        {
            Model.Set("btn_back_active",false);
            return "";
        }
    }
    
    /// <summary>
    /// Round paused
    /// </summary>
    public class SOnlinePause:FSM.IState
    {
        public string Enter()
        {
            Model.Set("btn_back_active",true);
            Model.Set("gameplay_title","PAUSE");
            return "";
        }
   
        public string Signal(string name, object arg)
        {
            switch (name)
            {
                case "btn_back": 
                    EventManager.Invoke("online_state",NetworkController.GameState.Exit);
                    Model.Set("overlay_play_visible",false);
                    return "menu";
                
                case "btn_pause": 
                    EventManager.Invoke("online_state",NetworkController.GameState.Play);
                    return "";
                
                case "set_state":
                    return arg.ToString();
            }
            
            return "";
        }

        public string Exit()
        {
            Model.Set("btn_back_active",false);
            return "";
        }
    }
    
}