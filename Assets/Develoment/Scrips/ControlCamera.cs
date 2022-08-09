using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ControlCamera : MonoBehaviour
{
    GameObject Player;
    public static Action FollowEvent;
    public bool Follow;

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
