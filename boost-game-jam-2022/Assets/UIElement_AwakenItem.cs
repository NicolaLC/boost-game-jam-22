using UnityEngine;

public class UIElement_AwakenItem : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Selector = null;

    public void SetActive(bool i_bActive)
    {
        if (m_Selector != null)
        {
            m_Selector.SetActive(i_bActive);
        }
    }
}
