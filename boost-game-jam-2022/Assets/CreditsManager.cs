using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(GoToIntro());
    }

    private IEnumerator GoToIntro()
    {
        yield return new WaitForSeconds(12);
        SceneManager.LoadScene("Intro");
    }
}
