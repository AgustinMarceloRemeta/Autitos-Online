using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ControlCamera : MonoBehaviour
{
    [SerializeField] Vector3 Distance;
    GameObject Player;
    public static Action FollowEvent;
    void Start()
    {
        
    }


    void Update()
    {
        FollowCamera();
    }
    void FollowCamera()
    {
        if (Player != null)
        {
            this.gameObject.transform.LookAt(Player.transform);
            this.transform.SetParent(Player.transform);
        }
    }
    void AsignPlayer()
    {
        Player = GameObject.Find("PlayerLocal");
        this.transform.position = Player.transform.position - Distance;
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
