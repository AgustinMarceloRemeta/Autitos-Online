using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
     [SerializeField] GameObject[] Parts;
     public void ChangeColors(Material mat)
    {
        for (int item = 0; item < Parts.Length; item++) 
        {
            if (item >= 3) Parts[item].GetComponent<MeshRenderer>().materials[3].color = mat.color;
            else  Parts[item].GetComponent<MeshRenderer>().materials[1].color = mat.color;
        }
    }
}
