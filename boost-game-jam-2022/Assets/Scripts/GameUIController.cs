using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : SingletonPattern<GameUIController>
{
    [SerializeField] 
    private List<UIElement_AwakenItem> m_AwakenItems = new List<UIElement_AwakenItem>();

    [SerializeField]
    private List<GameObject> m_Runes = new List<GameObject>();

    [SerializeField] 
    private Sprite m_InactiveRune = null;

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
    
    public static void OnAIScore(int i_CurrentRunes)
    {
        Instance.Internal_OnPlayerLose(i_CurrentRunes);
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
    
    private void Internal_OnPlayerLose(int i_CurrentRunes)
    {
        for (int i = i_CurrentRunes; i < m_Runes.Count; ++i)
        {
            m_Runes[i].GetComponent<Image>().sprite = m_InactiveRune;
        }
    }
    
}
