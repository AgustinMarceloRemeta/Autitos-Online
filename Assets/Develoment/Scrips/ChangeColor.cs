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
            Parts[item].GetComponent<MeshRenderer>().materials[1].color = mat.color;
           //  Parts[item].GetComponent<MeshRenderer>().material = mat;
        }
    }
}
