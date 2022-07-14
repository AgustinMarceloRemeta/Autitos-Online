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
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            /*
            data.direction.Normalize();
            transform.Translate(data.direction * Runner.DeltaTime);
            */
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

}