using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGround : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Clay")
        {
            other.transform.gameObject.SetActive(false);
        }
    }
}
