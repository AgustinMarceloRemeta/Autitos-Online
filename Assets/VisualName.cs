using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualName : MonoBehaviour
{
    GameObject cam;
    void Start()
    {
        cam = GameObject.Find("FollowText");
    }


    void Update()
    {
        this.transform.LookAt(cam.transform);
    }
}
