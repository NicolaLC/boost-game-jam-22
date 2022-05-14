using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform m_PlayerPosition = null;
    [SerializeField] 
    private Transform m_BoardLookPosition = null;
    [SerializeField] 
    private Transform m_EndGamePosition = null;

    private Transform m_TargetPosition = null;

    public void SetPlayerPosition()
    {
        m_TargetPosition = m_PlayerPosition;
    }

    public void SetBoardLookPosition()
    {
        m_TargetPosition = m_BoardLookPosition;
    }
    
    public void SetEndGamePosition()
    {
        m_TargetPosition = m_EndGamePosition;
    }
    
    private void Awake()
    {
        m_TargetPosition = m_PlayerPosition;
    }

    private void Update()
    {
        if (transform.position != m_TargetPosition.position)
        {
            LerpToPosition();
        }

        if (transform.rotation != m_TargetPosition.rotation)
        {
            LerpToRotation();
        }
    }

    private void LerpToPosition()
    {
        transform.position = Vector3.Slerp(transform.position, m_TargetPosition.position, 0.01f);
    }
    
    private void LerpToRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, m_TargetPosition.rotation, 0.02f);
    }
}
