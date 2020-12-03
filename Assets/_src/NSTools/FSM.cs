using UnityEngine;

namespace NSTools
{
    public static class FSM
    {
        public interface IState
        {
            IState Enter();
            IState Exit();
            IState Signal(string name, object arg);
        }

        private static IState currentState;


        public static void Go(IState newState)
        {
            if (newState == null) return;
            Log.Info($"FSM:Go({newState})");
            var result = currentState?.Exit();
            if (result != null)
            {
                Go(result);    
                return;
            }
            currentState = newState;
            result = currentState.Enter();
            Go(result);
        }

        public static void Signal(string name, object arg = null)
        {
            Log.Trace($"FSM.Signal({name}, {arg}",Camera.main);
            var result = currentState.Signal(name, arg);
            Go(result);
        }
    }
}