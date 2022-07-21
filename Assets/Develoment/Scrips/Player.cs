using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Player : NetworkBehaviour
{
    [Header("Movement")]
    Rigidbody Rb;
    [SerializeField] WheelCollider WheelBl, WheelBr, WheelFr, WheelFl;
    [SerializeField] Transform TrWheelBl, TrWheelBr, TrWheelFr, TrWheelFl;
    [SerializeField] float Force, Velocity, VelocityMax, ActualVelocity, AnguledDirection, Turn;
    [SerializeField] int PointControl, Laps;
    [SerializeField] GameObject NewCamera;
    GameManager manager;
    [SerializeField] Vector3 NewPosition;
    [Networked] public string Name { get; set; }
   // public string OldName;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        manager = FindObjectOfType<GameManager>();

    }
    private void Start()
    {
        if (Object.HasInputAuthority)
        {
            ControlCamera.FollowEvent?.Invoke();
            Name = FindObjectOfType<BasicSpawner>().Name;
            if (Name == "") Name  = "Jugador sin nombre";
            this.gameObject.name = Name;
        }
        Init(FindObjectOfType<BasicSpawner>().IdPlayer);


    }
   
    void Init(int player)
    {
        FindObjectOfType<GameManager>().Race(this.GetComponent<Player>(),player);
    }

    void Win()
    {
        if (Laps == manager.LapsForWin)
        {
            manager.ListText(Name);
            if (Object.HasInputAuthority)  GameObject.FindGameObjectWithTag("NewCamera").GetComponent<Camera>().enabled = true;
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
    }
    public override void FixedUpdateNetwork()
    {
        Movement();
        VisualWhels();
    //    if (Object.HasInputAuthority) Name = OldName; 
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
        TrWheelFl.localEulerAngles = new Vector3(TrWheelFl.localEulerAngles.x,Turn, 0);
        TrWheelFr.localEulerAngles = new Vector3(TrWheelFr.localEulerAngles.x, Turn, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        ControlPoint controlPoint = other.gameObject.GetComponent<ControlPoint>();
        if (controlPoint != null)
        {
            switch (controlPoint.Order)
            {
                case 1: if (PointControl == 0) PointControl++;
                    else if (PointControl == 5) { Laps++; PointControl = 1; }
                    break;
                case 2: if (PointControl == 1) PointControl++;
                    break;
                case 3: if (PointControl == 2) PointControl++;
                    break;
                case 4: if (PointControl == 3) PointControl++;
                    break;
                case 5:if (PointControl == 4) PointControl++;
                    break;
                default:
                    break;
            }
        }
    }
}