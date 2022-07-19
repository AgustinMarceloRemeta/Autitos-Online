using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] Vector3[] Positions;
    [SerializeField] Material NewMaterial;
    void Start()
    {
        
    }


    void Update()
    {
       
    }

    public void Race(Player[] players) 
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].gameObject.transform.position = Positions[i];
            players[i].gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            StartCoroutine(Countdown(players));
        }
    }
    IEnumerator Countdown(Player[] players)
    {
        yield return new WaitForSeconds(3);
        foreach (var item in players) item.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        this.GetComponent<MeshRenderer>().material = NewMaterial;
    }
 
}
