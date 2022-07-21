using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
     [SerializeField] GameObject[] Parts;
     public void ChangeColors(Material mat)
    {
        foreach (var item in Parts)
        {
            item.GetComponent<MeshRenderer>().material = mat;
        }
    }
}
