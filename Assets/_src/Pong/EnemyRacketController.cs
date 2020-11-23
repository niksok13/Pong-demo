using System.Collections;
using System.Collections.Generic;
using NSTools;
using UnityEngine;
using UnityEngine.EventSystems;


public enum PlayerMode
{
    Player, Bot, Mirror, Online
}

public class EnemyRacketController : BaseRacketController, IDragHandler
{
    public Transform player1;
    
    private PlayerMode mode;

    private void Awake()
    {
        Model.Bind("enemy_mode", SetMode);
    }

    private void SetMode(object obj)
    {
        mode = Model.Get("enemy_mode", PlayerMode.Bot);

        foreach (Transform child in transform)
            child.gameObject.SetActive(mode == PlayerMode.Player);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (mode == PlayerMode.Player) ProcessDrag(eventData);
    }


    public void Update()
    {
        if (!Model.Get("game_playing",false)) return;
        switch (mode)
        {
            case PlayerMode.Bot:
            {
                var panel_pos = panel.position;
                panel_pos.x = Mathf.Lerp(panel_pos.x,ball.position.x,0.3f);
                panel_pos.x = Mathf.Min(panel_pos.x, 2);
                panel_pos.x = Mathf.Max(panel_pos.x, -2);
                panel.position = panel_pos;
                break;
            }
            case PlayerMode.Mirror:
            {
                panel.localPosition = player1.localPosition;
                break;
            }
        }
    }
}
