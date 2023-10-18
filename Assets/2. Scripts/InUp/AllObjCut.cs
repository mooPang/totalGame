using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllObjCut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Object")
        {
            other.gameObject.SetActive(false);
        }
    }
}
