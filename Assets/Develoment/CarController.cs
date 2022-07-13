using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarController : MonoBehaviour
{
    [Header("Movement")]
    Rigidbody Rb;
    [SerializeField] WheelCollider WheelBl, WheelBr, WheelFr, WheelFl;
    [SerializeField] Transform TrWheelBl, TrWheelBr, TrWheelFr, TrWheelFl;
    [SerializeField] float Force, Velocity,VelocityMax, ActualVelocity, AnguledDirection, Turn;
    public bool VelocityPower;

    
    private void Start()
    {
        Rb = GetComponent<Rigidbody>();

    }
    private void FixedUpdate()
    {
        Wheels();
        
    }
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        VisualWhels();

    }

    private void Wheels()
    {
        ActualVelocity = 2 * Mathf.PI * WheelFl.radius * WheelFl.rpm * 60 / 1000;
      if(Velocity<VelocityMax)
        {
            if (VelocityPower)
            {
                WheelFl.motorTorque = Force * Input.GetAxis("Vertical")*2;
                WheelFr.motorTorque = Force * Input.GetAxis("Vertical") *2;
                WheelBl.motorTorque = Force * Input.GetAxis("Vertical") *2;
                WheelBl.motorTorque = Force * Input.GetAxis("Vertical") *2;
            }
            else
            {
                WheelFl.motorTorque = Force * Input.GetAxis("Vertical");
                WheelFr.motorTorque = Force * Input.GetAxis("Vertical");
                WheelBl.motorTorque = Force * Input.GetAxis("Vertical");
                WheelBl.motorTorque = Force * Input.GetAxis("Vertical");
            }
        }
        else
        {
            WheelFl.motorTorque = 0;
            WheelFr.motorTorque = 0;
            WheelBl.motorTorque = 0;
            WheelBl.motorTorque = 0;
        }

        Velocity = Rb.velocity.magnitude * 15;

        Turn = AnguledDirection * Input.GetAxis("Horizontal");
        WheelFl.steerAngle = Turn;
        WheelFr.steerAngle = Turn;
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

