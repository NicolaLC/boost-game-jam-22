using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GoToGame());
    }

    private IEnumerator GoToGame()
    {
        yield return new WaitForSeconds(43);

        SceneManager.LoadScene("Game");
    }
}
