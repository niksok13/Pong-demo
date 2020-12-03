using System.Collections;
using System.Collections.Generic;
using NSTools;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseRacketController : MonoBehaviour
{
    public Transform panel;
    public BallController ball;

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
        var panelPos = panel.localPosition.x + eventData.delta.x / 100;
        panelPos = Mathf.Min(panelPos, 1.9f);
        panelPos = Mathf.Max(panelPos, -1.9f);
        panel.localPosition = new Vector2(panelPos,0);

    }
    
}
