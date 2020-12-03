using System;
using NSTools;

namespace States
{
    /// <summary>
    /// Ready to play 
    /// </summary>
    public class SReady:FSM.IState
    {
        public FSM.IState Enter()
        {
            Model.Set("gameplay_hint","TOUCH TABLE TO START");
            var gamemode = Model.Get("enemy_mode", PlayerMode.Player);
            switch (gamemode)
            {
                case PlayerMode.Ranked:
                    Model.Set("gameplay_title", "SURVIVE FOR A WHILE\nJOIN TOP 100");
                    break;
    
                case PlayerMode.Bot:
                    Model.Set("gameplay_title", "SHARPEN YOUR SKILLS WITH BOT");
                    break;

                case PlayerMode.Mirror:
                    Model.Set("gameplay_title", "CONTROL BOTH BARS\nWITH ONE THUMB");
                    break;
                
                case PlayerMode.Player:
                    Model.Set("gameplay_title", "PLAY WITH YOUR FRIEND\n(best on tablet)");
                    break;
                
                default:
                    break;
            }

            Model.Set("btn_back_active", true);
            Model.Set("overlay_play_visible",true); 
            EventManager.Invoke("reset_game");
            return null;
        }

        
        public FSM.IState Signal(string name, object arg)
        {
            switch (name)
            {
                case "btn_back": 
                    Model.Set("overlay_play_visible",false);
                    EventManager.Invoke("sound_play","pop");
                    return new SMenu();
                
                case "btn_pause": 
                    return new SPlay();
            }

            return null;
        }
        
        public FSM.IState Exit()
        {
            Model.Set("btn_back_active", false);
            return null;
        }
    }

    
    /// <summary>
    /// Playing
    /// </summary>
    public class SPlay:FSM.IState
    {
        public FSM.IState Enter()
        {
            Model.Set("game_playing",true);
            Model.Set("gameplay_hint","TOUCH TABLE TO PAUSE");
            Model.Set("gameplay_title","");
            return null;
        }

        public FSM.IState Signal(string name, object arg)
        {
            switch (name)
            {
                case "player_win":
                    Model.Set("winner", arg);
                    return new SResult();
                
                case "btn_pause": 
                    return new SPause();
            }

            return null;
        }

        public FSM.IState Exit()
        {
            Model.Set("game_playing",false);
            return null;
        }
    }
    
    /// <summary>
    /// Round result
    /// </summary>
    public class SResult: FSM.IState
    {
        public FSM.IState Enter()
        {

            if (Model.Get("enemy_mode", PlayerMode.Player) == PlayerMode.Ranked){
                var time = TimeSpan.FromSeconds(Model.Get("timer_seconds", 0f));
                if (time.TotalSeconds>PrefsManager.BestTime){
                    PrefsManager.BestTime = (float)time.TotalSeconds;
                    LeaderboardInterface.SubmitScore(time);
                }
            }
            EventManager.Invoke("sound_play","pong");
            var winner = Model.Get("winner", -1)==1?
                Model.Get("player_name","YOU"):
                Model.Get("enemy_name","ENEMY");
            Model.Set("gameplay_title",$"{winner} WIN!");
            Model.Set("gameplay_hint",$"TOUCH TABLE TO CONTINUE");
            return null;
        }

        public FSM.IState Signal(string name, object arg)
        {
            return name == "btn_pause" ? new SReady():null;
        }

        public FSM.IState Exit() => null;
    }
    
    /// <summary>
    /// Round paused
    /// </summary>
    public class SPause:FSM.IState
    {
        public FSM.IState Enter()
        {
            Model.Set("gameplay_title","PAUSED");
            Model.Set("gameplay_hint",$"TOUCH TABLE TO CONTINUE");
            return null;
        }
   
        public FSM.IState Signal(string name, object arg)
        {
            return name == "btn_pause" ? new SPlay():null;
        }

        public FSM.IState Exit() => null;
    }
}