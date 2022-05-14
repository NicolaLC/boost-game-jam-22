using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CellController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_GlowEffect = null;
    [SerializeField]
    private GameObject m_AIGraphics = null;
    [SerializeField]
    private GameObject m_PlayerGraphics = null;
    
    private int m_row = 0;
    private int m_col = 0;

    private BoardController.CellValue m_Value = BoardController.CellValue.None;

    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Console.Clear();
        }
        #endif
    }

    public void Setup(int i_Row, int i_Col)
    {
        m_row = i_Row;
        m_col = i_Col;
        
        if (m_AIGraphics != null)
        {
            m_AIGraphics.SetActive(false);
        }

        if (m_PlayerGraphics != null)
        {
            m_PlayerGraphics.SetActive(false);
        }
        
        if (m_GlowEffect != null)
        {
            m_GlowEffect.SetActive(false);
        }
    }

    private void OnMouseEnter()
    {
        if (m_Value != BoardController.CellValue.None)
        {
            return;
        }

        if (Game.CurrentTarget != Game.TurnTarget.Player)
        {
            return;
        }
        
        StartGlow();
    }

    private void OnMouseDown()
    {
        if (m_Value != BoardController.CellValue.None)
        {
            return;
        }
        
        if (Game.CurrentTarget != Game.TurnTarget.Player)
        {
            return;
        }

        StopGlow();
        Game.CellSelected(m_row, m_col, Game.TurnTarget.Player);
    }

    private void OnMouseExit()
    {
        StopGlow();
    }
    
    private void StartGlow()
    {
        if (m_GlowEffect == null)
        {
            return;
        }
        
        m_GlowEffect.SetActive(true);
        CursorManager.SetInteract();
    }
    
    private void StopGlow()
    {
        if (m_GlowEffect == null)
        {
            return;
        } 
        
        m_GlowEffect.SetActive(false);
        CursorManager.SetDefault();
    }

    public void SetValue(BoardController.CellValue i_Value)
    {
        if (i_Value == BoardController.CellValue.None)
        {
            return;
        }

        m_Value = i_Value;

        SetGraphics();
    }

    private void SetGraphics()
    {
        if (m_AIGraphics != null)
        {
            m_AIGraphics.SetActive(m_Value == BoardController.CellValue.AI);
        }

        if (m_PlayerGraphics != null)
        {
            m_PlayerGraphics.SetActive(m_Value == BoardController.CellValue.Player);
        }
    }

    public void Reset()
    {
        m_Value = BoardController.CellValue.None;
        SetGraphics();
    }
}
