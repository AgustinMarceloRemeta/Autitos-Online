using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using Fusion;

public class GameManager : NetworkBehaviour
{
    #region Parameters
    [SerializeField] Vector3[] Positions;
    public List<Player> Players;
    [SerializeField] Material NewMaterial, PreMaterial;
    public int LapsForWin;
    [SerializeField] Text TextWin;
    string TextWinSt; 
    int Order;
    [SerializeField] GameObject ListWin, BoxColor;
    #endregion

    #region Fuctions
    void Start()
    {
        TextWinSt = "Ganadores:" + "\n";
    }

    public override void FixedUpdateNetwork()
    {
        TextWin.text = TextWinSt;
    }

    public void WinPlayer()
    {
        if (!ListWin.activeSelf) ListWin.SetActive(true); 
        for (int item = 0; item < Players.Count; item++)        
        {
            if (Players[item].End)
            {
                Order++;
                ListText("Puesto " + Order + ":" + Players[item].NickName.ToString());
                Players[item].End = false;
            }
        }
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
                player.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                player.gameObject.transform.position = Positions[i];
                player.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
    }

   public IEnumerator Countdown(Player[] player)
    {
        FindObjectOfType<ControlCamera>().Follow = true;
        for (int i = 0; i < player.Length; i++)
        yield return new WaitForSeconds(3);
        for (int i = 0; i < player.Length; i++)
        player[i].gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        BoxColor.GetComponent<MeshRenderer>().material = NewMaterial;
    }

    public void Respawn()
    {
        Player.RespawnEvent?.Invoke();
        GameObject.FindGameObjectWithTag("NewCamera").GetComponent<Camera>().enabled = false;
        BoxColor.GetComponent<MeshRenderer>().material = PreMaterial;
        BasicSpawner Spawner = FindObjectOfType<BasicSpawner>();
        if (Spawner.IdPlayer== Spawner.MaxPlayersRoom - 1)
        Spawner.InitRace();
        ListWin.SetActive(false);
        TextWinSt = "Posiciones:";
    }
    #endregion
}
