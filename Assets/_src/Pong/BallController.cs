using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NSTools;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallController : MonoBehaviour
{
    public Transform player1, player2;
    // Start is called before the first frame update
    private bool isRunning = false;
    
    public Vector3 direction;
    public float speed;
    public float ball_radius;
    
    private float wallDistance;
    private float panelDistance;

    private float _wallDistance = 2.4f;
    private float _panelDistance = 2.4f;
    private const float playerWidth = 1f;
    private ParticleSystem ps;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        Model.Bind("game_playing", ProcessGameplayState);
        EventManager.Bind("reset_game",ResetGame);
    }

    private void ProcessGameplayState(object obj)
    {
        isRunning = Model.Get("game_playing", false);
        if (isRunning)
            ps.Play();
        else
            ps.Stop();
    }

    void ResetGame(object obj)
    {
        var diff = Model.Get("difficulty", 1f);
        
        if (Model.Get("enemy_mode", PlayerMode.Player) == PlayerMode.Ranked)
            diff = 0.5f;
        
        ball_radius = 0.3f - diff / 4;
        wallDistance = _wallDistance - ball_radius;
        panelDistance = _panelDistance - ball_radius;
        transform.localScale = Vector2.one*ball_radius*2;
        direction = Random.insideUnitCircle.normalized;
        while (direction.y < 0.83)
            direction = Random.insideUnitCircle.normalized;

        speed = 5+10f * diff;
        transform.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            transform.localPosition += Time.deltaTime * speed * direction;
            CheckWalls();
            CheckPlayerCollision();
        }
    }


    /// <summary>
    /// Check side wall collision with between frames
    /// </summary>
    void CheckWalls()
    {
        var cur_x = transform.localPosition.x;
        var pos_fix = 0f;
        
        if (cur_x > wallDistance)
            pos_fix = -(cur_x - wallDistance) * 2;

        if (cur_x < -wallDistance)
            pos_fix = -(cur_x + wallDistance) * 2;

        if (pos_fix != 0f)
        {
            
            direction = Vector3.Reflect(direction, Vector2.right);
            transform.Translate(pos_fix,0,0);
            ps.Emit(5);
        }
    }
  
    
    /// <summary>
    /// Check player collision
    /// </summary>
    private void CheckPlayerCollision()
    {
        var cur_y = transform.position.y;
        var pos_fix = 0f;
        
        if (cur_y > panelDistance)
            if (Mathf.Abs(transform.position.x - player2.position.x) < playerWidth / 2+ball_radius)
            {
                EventManager.Invoke("sound_play","ping");
                var dx = transform.position.x - player2.position.x;
                direction = new Vector2(dx*3,-playerWidth).normalized;
                pos_fix = -(cur_y - panelDistance) * 2;
            }
            else
                FSM.Signal("player_win", 1);
            

        if (cur_y < -panelDistance)
            if (Mathf.Abs(transform.position.x - player1.position.x) < playerWidth / 2+ball_radius)
            {
                EventManager.Invoke("sound_play","ping");
                var dx = transform.position.x - player1.position.x;
                direction = new Vector2(dx*3,playerWidth).normalized;
                pos_fix = -(cur_y + panelDistance) * 2;
            }
            else
                FSM.Signal("player_win", 2);


        if (pos_fix != 0f)
        {
            transform.Translate(0,pos_fix,0);
            ps.Emit(5);
        }
    }
    
}
