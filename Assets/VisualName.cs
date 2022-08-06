using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualName : MonoBehaviour
{
    GameObject cam;
    [SerializeField] float distance;
    void Start()
    {
        cam = GameObject.Find("FollowText");
    }


    void Update()
    {
        if (Vector3.Distance(this.transform.position, cam.transform.position) > distance) this.GetComponent<Text>().enabled = false;
        else this.GetComponent<Text>().enabled = false;
        this.transform.LookAt(cam.transform);
    }
}
