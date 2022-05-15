
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGamePanels : SingletonPattern<EndGamePanels>
{
    [SerializeField]
    private GameObject m_WinPanel = null;
    [SerializeField]
    private GameObject m_LosePanel = null;

    public static void OnWin()
    {
        Instance.m_WinPanel.SetActive(true);
    }
    
    public static void OnLose()
    {
        Instance.m_LosePanel.SetActive(true);
    }

    public static void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
