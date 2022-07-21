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
        if (Follow)
        {
            this.GetComponent<FollowCamera>().target = Player.transform;
            Follow = false;
        }


    }

    void AsignPlayer()
    {
        Player = GameObject.Find("LocalP");
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
