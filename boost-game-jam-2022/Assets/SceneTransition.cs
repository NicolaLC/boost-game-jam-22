using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : SingletonPattern<SceneTransition>
{
    [SerializeField]
    private Animator m_Animator = null;

    public static void Show()
    {
        Instance.m_Animator.Play("Transition_Show");
    }
    
    public static void Hide()
    {
        Instance.m_Animator.Play("Transition_Hide");
    }
}
