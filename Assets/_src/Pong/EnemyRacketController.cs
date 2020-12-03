using System;
using NSTools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public enum PlayerMode
{
    Player, Bot, Mirror, Ranked
}

public class EnemyRacketController : BaseRacketController, IDragHandler
{
    public Transform player1;
    
    private PlayerMode mode;
    private float playTime;

    private void Awake()
    {
        Model.Bind("enemy_mode", SetMode);
        EventManager.Bind("reset_game", ResetTimer);
    }

    private void ResetTimer(object obj)
    {
        playTime = 0;
        Model.Set("timer_seconds",playTime);
        Model.Set("timer_value", TimeSpan.FromSeconds(playTime).ToString(@"m\:ss\:fff"));
        panel.localPosition = Vector3.zero;
    }

    private void SetMode(object obj)
    {
        mode = Model.Get("enemy_mode", PlayerMode.Bot);
        var label = GetComponent<Text>();
        var lbl = "ALPHA-PONG";
        switch (mode) {
            
            case PlayerMode.Bot:
                lbl = "ALPHA-PONG";
                break;

            case PlayerMode.Ranked:                
                lbl = "GOD OF PONG";
                break;
            
            case PlayerMode.Mirror:
                lbl = Model.Get("player_name","");
                break;
            
            case PlayerMode.Player:
                lbl = "PLAYER 2";
                break;
        }

        Model.Set("timer_visible", mode == PlayerMode.Ranked);
        Model.Set("player2_input", mode == PlayerMode.Player);
        Model.Set("enemy_name", lbl);
        label.text = lbl;
    }

    public void Update()
    {
        if (!Model.Get("game_playing",false)) return;
        var diff = Model.Get("difficulty", 1f);
        var panel_pos = panel.position;
        switch (mode)
        {
            case PlayerMode.Ranked:
                playTime += Time.deltaTime;
                Model.Set("timer_seconds",playTime);
                Model.Set("timer_value", TimeSpan.FromSeconds(playTime).ToString(@"m\:ss\:fff"));
                panel_pos.x = ball.transform.position.x;
                panel_pos.x = Mathf.Min(panel_pos.x, 1.9f);
                panel_pos.x = Mathf.Max(panel_pos.x, -1.9f);
                panel.position = panel_pos;
                break;

            case PlayerMode.Bot:
                if (ball.direction.y < 0) return;
                panel_pos.x = Mathf.Lerp(panel_pos.x,ball.transform.position.x,diff/2);
                panel_pos.x = Mathf.Min(panel_pos.x, 1.9f);
                panel_pos.x = Mathf.Max(panel_pos.x, -1.9f);
                panel.position = panel_pos;
                break;

            case PlayerMode.Mirror:
                panel.localPosition = player1.localPosition;
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (mode == PlayerMode.Player) ProcessDrag(eventData);
    }


}
