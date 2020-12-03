using NSTools;
using States;
using UnityEngine;

public class MainController : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
        FSM.Go(new SInit());
    }

}
