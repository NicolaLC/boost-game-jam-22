using TMPro;
using UnityEngine;

public class UIElement_GameMessage : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI m_GameMessageText = null;

    [SerializeField] 
    private Animator m_Animator = null;

    public void Show(string i_Message)
    {
        m_GameMessageText.text = i_Message;
        
        m_Animator.Play("Text_Show");
    }
}
