using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameData : MonoBehaviour
{
    [SerializeField] Text NameText;
 
    public void InitGame()
    {
        PlayerPrefs.SetString("PlayerNickName", NameText.text);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }
}
