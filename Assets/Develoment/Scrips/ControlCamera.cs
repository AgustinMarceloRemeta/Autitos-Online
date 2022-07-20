using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ControlCamera : MonoBehaviour
{
    [SerializeField] Vector3 Distance;
    GameObject Player;
    public static Action FollowEvent;
    [SerializeField]float InitY,CameraY ,SpeedAnim;
    public bool Follow;
    void Start()
    {
        
    }


    void Update()
    {
        if (Follow) FollowCamera();
        

    }
    void FollowCamera()
    {
        if (Player != null)
        {
            this.gameObject.transform.LookAt(Player.transform);
            float Vel = Time.deltaTime * SpeedAnim;
            if (InitY > CameraY) InitY -= Vel;
            this.transform.position = new Vector3(this.transform.position.x,InitY, this.transform.position.z);
        }
    }
    void AsignPlayer()
    {
        Player = GameObject.Find("PlayerLocal");
    }
   public void SetCam()
    {
        this.transform.localPosition = Player.transform.localPosition + Distance;
        this.transform.SetParent(Player.transform);
    }
    private void OnEnable()
    {
        FollowEvent += AsignPlayer;
    }
    private void OnDisable()
    {
        FollowEvent -= AsignPlayer;
    }

}
