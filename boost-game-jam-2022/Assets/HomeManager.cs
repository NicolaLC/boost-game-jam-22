using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    public void GoToIntro()
    {
        SceneManager.LoadScene("Credits");
    }
}
