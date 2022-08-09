
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class Player : NetworkBehaviour
{
    #region Parameters
        [Header("Movement")]
    Rigidbody Rb;
    [SerializeField] WheelCollider WheelBl, WheelBr, WheelFr, WheelFl;
    [SerializeField] Transform TrWheelBl, TrWheelBr, TrWheelFr, TrWheelFl;
    [SerializeField] float Force, Velocity, VelocityMax, ActualVelocity, AnguledDirection, Turn, breakForce;
    public float currentBreakForce;

    [Header("Gameplay")]
    [SerializeField] GameObject NewCamera;
    [SerializeField] int PointControl, Laps;
    GameManager manager;
    [SerializeField] Vector3 NewPosition;
    [SerializeField] Material[] material;
    public bool End;
    public static Action RespawnEvent;

    [Header("Name")]
    [SerializeField] Text NameText;
    [Networked(OnChanged = nameof (OnNickNameChanged))]
    public NetworkString<_16> NickName { get; set; }
    [Networked] public int NumberPlayer { get; set; }
    #endregion

    #region Gameplay
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
        }

    }

    public override void FixedUpdateNetwork()
    {
        Movement();
        VisualWhels();
        if (End) manager.WinPlayer();
    }

    private void Start()
    {
        ControlCamera.FollowEvent?.Invoke();
        Init(FindObjectOfType<BasicSpawner>().IdPlayer);
        manager.Players.Add(this);
        ColorPlayer();
    }

    public void Update()
    {
        Win();
    }

    private void ColorPlayer()
    {
        if (Object.HasInputAuthority) NumberPlayer = FindObjectOfType<BasicSpawner>().IdPlayer;
        this.GetComponent<ChangeColor>().ChangeColors(material[NumberPlayer]);
    }

    void Init(int player)
    {
        manager.Race(this.GetComponent<Player>(), player);
    }

    void Win()
    {
        if (Laps == manager.LapsForWin)
        {
            End = true;
            LeaveCarrer();
            Laps = 0;
        }
    }

    private void LeaveCarrer()
    {
        if (Object.HasInputAuthority) GameObject.FindGameObjectWithTag("NewCamera").GetComponent<Camera>().enabled = true;
        this.transform.position = NewPosition;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_InitGame(Player[] players)
    {
        StartCoroutine(FindObjectOfType<GameManager>().Countdown(players));
    }

    #endregion

    #region Movement
    private void ApplyBreaking()
    {
        WheelFr.brakeTorque = currentBreakForce * 3 * Runner.DeltaTime;
        WheelBr.brakeTorque = currentBreakForce * 3 * Runner.DeltaTime;
        WheelBl.brakeTorque = currentBreakForce * 3 * Runner.DeltaTime;
        WheelFl.brakeTorque = currentBreakForce * 3 * Runner.DeltaTime;
    }

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


        TrWheelBl.Rotate(ActualVelocity*10, 0, 0);
        TrWheelFl.Rotate(ActualVelocity * 10, 0, 0);
        TrWheelBr.Rotate(ActualVelocity * 10, 0, 0);
        TrWheelFr.Rotate(ActualVelocity * 10, 0, 0);
        TrWheelFl.localEulerAngles = new Vector3(TrWheelFl.localEulerAngles.x, Turn*2, 0);
        TrWheelFr.localEulerAngles = new Vector3(TrWheelFr.localEulerAngles.x, Turn*2, 0);
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

    public void Respawn()
    {
        if(Object.InputAuthority) Init(FindObjectOfType<BasicSpawner>().IdPlayer);
    }

    private void OnEnable()
    {
        RespawnEvent += Respawn;
    }
    private void OnDisable()
    {
        RespawnEvent -= Respawn;
    }

}