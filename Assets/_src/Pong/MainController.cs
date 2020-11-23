using NSTools;
using States;
using UnityEngine;

public class MainController : MonoBehaviour
{
    void Start()
    {

        FSM.Add<SInit>("init");
        FSM.Add<SMenu>("menu");
        FSM.Add<SSettings>("settings");
        
        FSM.Add<SReady>("ready");
        FSM.Add<SPlay>("play");
        FSM.Add<SPause>("pause");
        FSM.Add<SResult>("result");
        
        
        FSM.Add<SOnline>("online_join");
        FSM.Add<SOnlineReady>("online_ready");
        FSM.Add<SOnlinePlay>("online_play");
        FSM.Add<SOnlinePause>("online_pause");
        FSM.Add<SOnlineResult>("online_result");

        FSM.Go("init");
    }

}
