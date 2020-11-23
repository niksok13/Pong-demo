using System;
using System.Text;
using System.Threading.Tasks;
using NSTools;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : MonoBehaviour
{
    private enum GameAction
    {
        Join = 1,
        Wait = 2,
        Update = 3,
        SetState = 4
    }

    public enum GameState
    {
        Ready = 1, 
        Play = 2, 
        Result = 3, 
        Pause = 4, 
        Exit = 5
    }
    private struct Payload
    {
        public GameAction command;
        public GameState state;
        public bool host;
        public Vector2 host_position, guest_position, ball_position, ball_direction;
        public float ball_radius, ball_speed;
    }

    private string url = "http://192.168.1.1:3000";
    
    public Transform player1, player2, ball;
    
    
    // Start is called before the first frame update
    async void Start()
    {
        EventManager.Bind("join_session",JoinSession);        
        EventManager.Bind("run_session",UpdateState);        
        EventManager.Bind("online_state",SetState);        
    }

    private bool isHost = false;
    private GameState curState;
    private void JoinSession(object obj)
    {
        var payload = new Payload {command = GameAction.Join};
        
        SendRequest(payload, data =>
        {
            isHost = data.host;
            print("sendJoin");
            WaitLobby();            
        });
    }
    
    private async void WaitLobby()
    {
        var isWaiting = true;
        while (isWaiting)
        {
            var payload = new Payload {                
                host = isHost,
                command = GameAction.Wait
            };
            
            SendRequest(payload,data =>
            {
                if (data.state == GameState.Ready)
                {
                    isWaiting = false;
                    FSM.Signal("match_found");
                }

                if (data.state == GameState.Exit)
                {
                    isWaiting = false;
                }
                print($"isWaiting: {isWaiting}");
            });
            await Task.Delay(TimeSpan.FromSeconds(1f));
        }
    }

    
    private async void UpdateState(object obj)
    {
        var isPlaying = true;
        var ballController = ball.GetComponent<BallController>();
        while (isPlaying)
        {
            var payload = new Payload
            {
                host = isHost,
                command = GameAction.Update
            };

            if (isHost)
            {
                payload.ball_speed = ballController.speed;
                payload.ball_direction = ballController.direction;
                payload.ball_radius = ballController.ball_radius;

                payload.ball_position = ball.localPosition;
                payload.host_position = player1.localPosition;
            }
            else
            {
                payload.guest_position = player1.localPosition;
            }
            SendRequest(payload,data => {
                if (!isHost) {
                    data.ball_position = Vector3.Reflect(data.ball_position, Vector3.down);
                    data.ball_direction = Vector3.Reflect(data.ball_direction, Vector3.down);
                    ballController.speed = data.ball_speed;
                    ballController.ball_radius = data.ball_radius;
                    ballController.direction = data.ball_direction;
                    ball.localPosition = data.ball_position;
                }
                var player2from = player2.localPosition;
                var player2to = isHost ?  data.guest_position: data.host_position;
                EZ.Spawn().Add(0.1f, t => {
                    player2.localPosition = Vector3.Lerp(player2from, player2to, EZ.QuadOut(t));
                });

                if (data.state == GameState.Exit)
                {
                    isPlaying = false;
                }

                if (data.state!=curState)
                {
                    curState = data.state;
                    var strState = "";
                    switch (curState)
                    {
                        case GameState.Ready:
                            strState = "online_ready";
                            break;

                        case GameState.Play:
                            strState = "online_play";
                            break;
                        
                        case GameState.Pause:
                            strState = "online_pause";
                            break;

                        case GameState.Result:
                            Model.Set("winner", ball.position.y>0);
                            strState = "online_result";
                            break;
                        
                        case GameState.Exit:
                            Model.Set("message_text","Multiplayer game finished");
                            Model.Set("window_message_visible",true);
                            strState = "menu";
                            break;
                    }
                    FSM.Signal("set_state",strState);
                }
                print($"isHost: {isHost}");
            });
            await Task.Delay(TimeSpan.FromSeconds(0.1f));
        }
    }

    private void SendRequest(Payload payload,Action<Payload> cb)
    {
        var body = JsonUtility.ToJson(payload);
        var bytes = Encoding.UTF8.GetBytes(body);
        var uwr = UnityWebRequest.Put(url, bytes);
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SendWebRequest().completed += r =>
        {
            var data = JsonUtility.FromJson<Payload>(uwr.downloadHandler.text);
            cb(data);
        };  
    }

    private void SetState(object state)
    {
        var payload = new Payload
        {
            command = GameAction.SetState,
            state = (GameState) state
        };
        
        SendRequest(payload, data => { });
        
    }

    private void OnDestroy()
    {
        SetState(GameState.Exit);
    }
}
