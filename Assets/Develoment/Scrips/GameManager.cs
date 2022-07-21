using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using Fusion;

public class GameManager : NetworkBehaviour
{
    [SerializeField] Vector3[] Positions;
    [SerializeField] Material NewMaterial;
    public int LapsForWin;
    [SerializeField] Text TextWin;
    string TextWinSt;
    public List<Player> Players;
    int Order;
    [SerializeField] Material[] material;

    void Start()
    {
        TextWinSt = "Ganadores:" + "\n";


    }

    public override void FixedUpdateNetwork()
    {
        WinPlayer();
        TextWin.text = TextWinSt;
    }

    public void WinPlayer()
    {
        for (int item = 0; item < Players.Count; item++)        
        {
            if (Players[item].End)
            {
                Order++;
                ListText("Puesto " + Order + ": Player " + (item + 1));
                Players[item].End = false;
            }
        }
    }

    void Update()
    {
     
    }

    public void ListText(string Name)
    {
   TextWinSt += Name + "\n";
    }

    public void Race(Player player, int id) 
    {
        for (int i = 0; i < Positions.Length; i++)        
            if (id == i)
            {
                player.gameObject.GetComponent<ChangeColor>().ChangeColors(material[i]);
                player.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                player.gameObject.transform.position = Positions[i];
                player.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
    }
   public IEnumerator Countdown(Player[] player)
    {
        FindObjectOfType<ControlCamera>().Follow = true;

        yield return new WaitForSeconds(3);
        for (int i = 0; i < player.Length; i++)
        player[i].gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        this.GetComponent<MeshRenderer>().material = NewMaterial;
     
     //   Destroy(FindObjectOfType<BasicSpawner>().gameObject);
     //   SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
 
}
