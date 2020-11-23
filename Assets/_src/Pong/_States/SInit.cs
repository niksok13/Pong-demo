using System;
using NSTools;
using UnityEngine;

namespace States
{
    public class SInit : FSM.IState
    {
        public string Enter()
        {
            Model.Set("ball_color", PrefsManager.BallColor); 
            return "menu";
        }

        public string Exit() => string.Empty;

        public string Signal(string name, object arg) => string.Empty;
    }
}