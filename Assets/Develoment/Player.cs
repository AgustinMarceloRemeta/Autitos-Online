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
    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        if (Object.HasInputAuthority)
        {
            this.gameObject.name = "PlayerLocal";
            ControlCamera.FollowEvent?.Invoke();
        } 
    }
    public override void FixedUpdateNetwork()
    {
        Movement();
        VisualWhels();
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
                WheelBl.motorTorque = Force * data.Force * Runner.DeltaTime;
                WheelBl.motorTorque = Force * data.Force * Runner.DeltaTime;
            }
            else
            {
                WheelFl.motorTorque = 0;
                WheelFr.motorTorque = 0;
                WheelBl.motorTorque = 0;
                WheelBl.motorTorque = 0;
            }

            Turn = AnguledDirection * data.turn;
            WheelFl.steerAngle = Turn;
            WheelFr.steerAngle = Turn;

        }
    }

    private void VisualWhels()
    {
        Vector3 DirectionWheel = TrWheelFl.localEulerAngles;
        DirectionWheel.y = Turn;
        TrWheelFl.localEulerAngles = DirectionWheel;
        TrWheelFr.localEulerAngles = DirectionWheel;

        TrWheelBl.Rotate(ActualVelocity, 0, 0);
        TrWheelFl.Rotate(ActualVelocity, 0, 0);
        TrWheelBr.Rotate(ActualVelocity, 0, 0);
        TrWheelFr.Rotate(ActualVelocity, 0, 0);
    }
}