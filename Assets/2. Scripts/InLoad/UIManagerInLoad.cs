using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerInLoad : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(ChangeMain());
    }

    IEnumerator ChangeMain()
    {
        yield return new WaitForSeconds(5.0f);

        SceneManager.LoadScene("Main");
    }
}
