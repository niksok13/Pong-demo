using System;
using NSTools;
using UnityEngine;

namespace States
{
    public class SInit : FSM.IState
    {
        public FSM.IState Enter()
        {
            Model.Set("difficulty",PrefsManager.Difficulty);
            Model.Set("sound_volume",PrefsManager.Volume);
            Model.Set("player_name",PrefsManager.PlayerName);
            LeaderboardInterface.FetchLeaderboard();
            return new SMenu();
        }

        public FSM.IState Exit() => null;

        public FSM.IState Signal(string name, object arg) => null;
    }
}