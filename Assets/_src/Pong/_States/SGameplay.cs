using System;
using NSTools;

namespace States
{
    /// <summary>
    /// Ready to play 
    /// </summary>
    public class SReady:FSM.IState
    {
        public string Enter()
        {
            Model.Set("gameplay_title","TOUCH TABLE TO START");
            Model.Set("label_time",$"Best time: {PrefsManager.BestTime}");
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
                    Model.Set("overlay_play_visible",false);
                    return "menu";
                
                case "btn_pause": 
                    Model.Set("match_time", 0);
                    return "play";
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
    public class SPlay:FSM.IState
    {
        private EZ ez;
        public string Enter()
        {
            Model.Set("game_playing",true);
            Model.Set("gameplay_title","TOUCH TABLE TO PAUSE");
            
            var match_time = Model.Get("match_time", 0);
            Model.Set("label_time",$"Current time: {TimeSpan.FromSeconds(match_time)}");
            ez = EZ.Spawn().Loop().Wait(1).Add(() =>
            {
                match_time++;
                Model.Set("match_time",match_time);
                Model.Set("label_time",$"Current time: {TimeSpan.FromSeconds(match_time)}");
            });

            return "";
        }

        public string Signal(string name, object arg)
        {
            switch (name)
            {
                case "player_win":
                    Model.Set("winner", arg);
                    return "result";
                
                case "btn_pause": 
                    return "pause";
            }

            return "";
        }

        public string Exit()
        {
            ez.Clear();
            Model.Set("game_playing",false);
            return "";
        }
    }
    
    /// <summary>
    /// Round result
    /// </summary>
    public class SResult: FSM.IState
    {
        public string Enter()
        {
            var winner = Model.Get("winner", -1);
            Model.Set("gameplay_title",$"PLAYER {winner} WIN!\n Touch table to continue");
            
            var match_time = TimeSpan.FromSeconds(Model.Get("match_time", 0));
            Model.Set("label_time",$"Current time: {match_time}");
            if (match_time > PrefsManager.BestTime)
                PrefsManager.BestTime = match_time;

            return "";
        }

        public string Signal(string name, object arg)
        {
            return name == "btn_pause" ? "ready" : "";
        }

        public string Exit() => "";
    }
    
    /// <summary>
    /// Round paused
    /// </summary>
    public class SPause:FSM.IState
    {
        public string Enter()
        {
            Model.Set("gameplay_title","PAUSE");
            return "";
        }
   
        public string Signal(string name, object arg)
        {
            return name == "btn_pause" ? "play" : "";
        }

        public string Exit() => "";
    }
}