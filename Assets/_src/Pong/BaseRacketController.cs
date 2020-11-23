using System.Collections;
using System.Collections.Generic;
using NSTools;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseRacketController : MonoBehaviour
{
    public Transform panel, ball;

    void Awake()
    {
        EventManager.Bind("reset_game",ResetGame);
    }

    private void ResetGame(object obj)
    {
        panel.localPosition = Vector3.zero;
    }

    protected void ProcessDrag(PointerEventData eventData)
    {
        if (!Model.Get("game_playing",false)) return;
        float panel_pos = panel.localPosition.x + eventData.delta.x / 100;
        panel_pos = Mathf.Min(panel_pos, 2);
        panel_pos = Mathf.Max(panel_pos, -2);
        panel.localPosition = new Vector2(panel_pos,0);

    }
    
}
