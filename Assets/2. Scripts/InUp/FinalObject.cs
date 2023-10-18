using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //게임 클리어~
        }
    }
}
