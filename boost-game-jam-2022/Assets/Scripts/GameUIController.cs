using System.Collections.Generic;
using UnityEngine;

public class GameUIController : SingletonPattern<GameUIController>
{
    [SerializeField] 
    private List<UIElement_AwakenItem> m_AwakenItems = new List<UIElement_AwakenItem>();

    [SerializeField]
    private List<GameObject> m_Runes = new List<GameObject>();

    private UIElement_AwakenItem m_ActiveAwakenItem = null;

    protected override void Awake()
    {
        base.Awake();

        m_ActiveAwakenItem = m_AwakenItems[0];
        m_ActiveAwakenItem.SetActive(true);

        for (int i = 1; i < m_AwakenItems.Count; i++)
        {
            m_AwakenItems[i].SetActive(false);
        }
    }

    public static void OnPlayerScore(int i_CurrentAwaken)
    {
        Instance.Internal_OnPlayerWin(i_CurrentAwaken);
    }
    
    public static void OnAIScore()
    {
        Instance.Internal_OnPlayerLose();
    }

    private void Internal_OnPlayerWin(int i_CurrentAwaken)
    {
        if (i_CurrentAwaken >= m_AwakenItems.Count)
        {
            return;
        }
        m_ActiveAwakenItem.SetActive(false);
        m_ActiveAwakenItem = m_AwakenItems[i_CurrentAwaken];
        m_ActiveAwakenItem.SetActive(true);
    }
    
    private void Internal_OnPlayerLose()
    {
        m_Runes[0].SetActive(false);
        m_Runes[1].SetActive(true);
    }
    
}
