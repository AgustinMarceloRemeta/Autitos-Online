using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualName : MonoBehaviour
{
    GameObject cam,Player;
    [SerializeField] float distance;
    Text text;
    void Start()
    {
        cam = GameObject.Find("FollowText");
        Player = GameObject.Find("LocalP");
        text = this.GetComponent<Text>();
    }


    void Update()
    {
        if (Vector3.Distance(this.transform.position, Player.transform.position) > distance) text.enabled = false;
        else text.enabled = true;
        this.transform.LookAt(cam.transform);
    }
}
