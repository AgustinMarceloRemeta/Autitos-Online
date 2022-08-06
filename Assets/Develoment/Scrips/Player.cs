
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class Player : NetworkBehaviour
{
    [Header("Movement")]
    Rigidbody Rb;
    [SerializeField] WheelCollider WheelBl, WheelBr, WheelFr, WheelFl;
    [SerializeField] Transform TrWheelBl, TrWheelBr, TrWheelFr, TrWheelFl;
    [SerializeField] float Force, Velocity, VelocityMax, ActualVelocity, AnguledDirection, Turn, breakForce;
    [SerializeField] int PointControl, Laps;
    [SerializeField] GameObject NewCamera;
    [Networked(OnChanged = nameof (OnNickNameChanged))]
    public NetworkString<_16> NickName { get; set; }
    [SerializeField] Text NameText;

    public float currentBreakForce;

    GameManager manager;
    [SerializeField] Vector3 NewPosition;

    public PlayerData data;
    public bool End;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        manager = FindObjectOfType<GameManager>();

    }
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            this.gameObject.name = "LocalP";
            RpcSetNickName(PlayerPrefs.GetString("PlayerNickName"));
            print(PlayerPrefs.GetString("PlayerNickName"));
        }
    }
    private void Start()
    {
      
        ControlCamera.FollowEvent?.Invoke();
        Init(FindObjectOfType<BasicSpawner>().IdPlayer);
        manager.Players.Add(this);
    }

    void Init(int player)
    {
        manager.Race(this.GetComponent<Player>(), player);
    }

    void Win()
    {
        if (Laps == manager.LapsForWin)
        {
            //  manager.ListText(Name.ToString());
            End = true;
            if (Object.HasInputAuthority) GameObject.FindGameObjectWithTag("NewCamera").GetComponent<Camera>().enabled = true;
            // Runner.Despawn(GetComponent<NetworkObject>());
            this.transform.position = NewPosition;
            Laps = 0;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_InitGame(Player[] players)
    {
        StartCoroutine(FindObjectOfType<GameManager>().Countdown(players));
    }


    public void Update()
    {
        //   if(Laps == manager.Laps) 
        Win();
        if (End) manager.WinPlayer();

    }
    public override void FixedUpdateNetwork()
    {
        Movement();
        VisualWhels();
      
        //    if (Object.HasInputAuthority) Name = OldName; 
        //NameLocal = Name.ToString();
    }
    
    private void ApplyBreaking()
 {
    WheelFr.brakeTorque = currentBreakForce*3 * Runner.DeltaTime;
    WheelBr.brakeTorque = currentBreakForce*3 * Runner.DeltaTime;
    WheelBl.brakeTorque = currentBreakForce*3 * Runner.DeltaTime;
    WheelFl.brakeTorque = currentBreakForce*3 * Runner.DeltaTime;
 }
    #region Gameplay
    private void Movement()
    {
        if (GetInput(out NetworkInputData data))
        {

            /*
            data.direction.Normalize();
            transform.Translate(data.direction * Runner.DeltaTime);
            */
            ActualVelocity = 2 * Mathf.PI * WheelFl.radius * WheelFl.rpm * 60 / 1000;
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            Velocity = Rb.velocity.magnitude * 15;

            currentBreakForce = data.Break ? breakForce : 0f;
            ApplyBreaking();

            if (Velocity < VelocityMax)
            {
                WheelFl.motorTorque = Force * data.Force * Runner.DeltaTime;
                WheelFr.motorTorque = Force * data.Force * Runner.DeltaTime;
                //  WheelBl.motorTorque = Force * data.Force * Runner.DeltaTime;
                //  WheelBr.motorTorque = Force * data.Force * Runner.DeltaTime;
            }
            else
            {
                WheelFl.motorTorque = 0;
                WheelFr.motorTorque = 0;
                //  WheelBl.motorTorque = 0;
                //   WheelBr.motorTorque = 0;
            }

            Turn = AnguledDirection * data.turn;
            WheelFl.steerAngle = Turn;
            WheelFr.steerAngle = Turn;


        }
    }

    private void VisualWhels()
    {
        //  Vector3 DirectionWheel = TrWheelFl.localEulerAngles + new Vector3(0,0,180);
        //  DirectionWheel.y = Turn;


        TrWheelBl.Rotate(ActualVelocity, 0, 0);
        TrWheelFl.Rotate(ActualVelocity, 0, 0);
        TrWheelBr.Rotate(ActualVelocity, 0, 0);
        TrWheelFr.Rotate(ActualVelocity, 0, 0);
        TrWheelFl.localEulerAngles = new Vector3(TrWheelFl.localEulerAngles.x, Turn, 0);
        TrWheelFr.localEulerAngles = new Vector3(TrWheelFr.localEulerAngles.x, Turn, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        ControlPoint controlPoint = other.gameObject.GetComponent<ControlPoint>();
        if (controlPoint != null)
        {
            switch (controlPoint.Order)
            {
                case 1:
                    if (PointControl == 0) PointControl++;
                    else if (PointControl == 5) { Laps++; PointControl = 1; }
                    break;
                case 2:
                    if (PointControl == 1) PointControl++;
                    break;
                case 3:
                    if (PointControl == 2) PointControl++;
                    break;
                case 4:
                    if (PointControl == 3) PointControl++;
                    break;
                case 5:
                    if (PointControl == 4) PointControl++;
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    #region name 
    static void  OnNickNameChanged(Changed<Player> changed)
    {
        changed.Behaviour.OnNickNameChanged();
    }
    private void OnNickNameChanged()
    {
        NameText.text = NickName.ToString();
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RpcSetNickName(string NickName, RpcInfo info = default)
    {
        this.NickName = NickName;
    }
    #endregion
}