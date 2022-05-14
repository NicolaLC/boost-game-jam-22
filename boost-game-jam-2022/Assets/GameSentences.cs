using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class GameSentences : SingletonPattern<GameSentences>
{

    [Serializable]
    public class Sentence
    {
        public string key = "";
        public string message = "";
        public AudioClip sound = null;
    }
    
    [SerializeField] 
    private GameObject m_PlayerSentenceRoot = null;
    [SerializeField] 
    private TextMeshProUGUI m_PlayerCardText = null;
    [SerializeField] 
    private Animator m_PlayerSentenceAnimator = null;
    
    [SerializeField] 
    private GameObject m_AISentenceRoot = null;
    [SerializeField] 
    private TextMeshProUGUI m_AICardText = null;
    [SerializeField] 
    private Animator m_AISentenceAnimator = null;

    [SerializeField] 
    private AudioSource m_LeftAudioSource;
    
    [SerializeField] 
    private AudioSource m_RightAudioSource;

    [SerializeField] 
    private List<Sentence> m_OriginalPositiveSentences = new List<Sentence>();
    private List<Sentence> m_UnusedPositiveSentences = new List<Sentence>();
    
    [SerializeField] 
    private List<Sentence> m_OriginalNegativeSentences = new List<Sentence>();
    private List<Sentence> m_UnusedNegativeSentences = new List<Sentence>();

    [SerializeField] 
    private AudioSource m_MasterGroup = null;

    public static void ShowSentence(bool i_bPositive)
    {
        Instance.Internal_ShowSentence(i_bPositive);
    }

    private void Internal_ShowSentence(bool i_bPositive)
    {
        if (i_bPositive)
        {
            StartCoroutine(ShowPositiveSentence());
        }
        else
        {
            StartCoroutine(ShowNegativeSentence());
        }
    }
    
    private IEnumerator ShowPositiveSentence()
    {
        if (m_UnusedPositiveSentences.Count == 0)
        {
            m_UnusedPositiveSentences = m_OriginalPositiveSentences;
        }
        
        // choose a random sentence
        Sentence sentence = m_UnusedPositiveSentences[UnityEngine.Random.Range(0, m_UnusedPositiveSentences.Count - 1)];
        
        m_UnusedPositiveSentences.Remove(sentence);
        
        m_LeftAudioSource.PlayOneShot(sentence.sound);
        
        m_PlayerSentenceRoot.SetActive(true);

        m_PlayerCardText.text = sentence.key;

        m_MasterGroup.volume = 0.5f;
        m_PlayerSentenceAnimator.Play("Sentence_In");

        yield return new WaitForSeconds(5);
        
        m_PlayerCardText.text = "";
        
        m_PlayerSentenceAnimator.Play("Sentence_Out");
        m_MasterGroup.volume = 1f;
        yield return new WaitForSeconds(1);
        
        m_PlayerSentenceRoot.SetActive(false);
    }
    
    private IEnumerator ShowNegativeSentence()
    {
        if (m_UnusedNegativeSentences.Count == 0)
        {
            m_UnusedNegativeSentences = m_OriginalNegativeSentences;
        }
        
        // choose a random sentence
        Sentence sentence = m_UnusedNegativeSentences[UnityEngine.Random.Range(0, m_UnusedNegativeSentences.Count - 1)];
        m_UnusedNegativeSentences.Remove(sentence);
        
        m_RightAudioSource.PlayOneShot(sentence.sound);
        
        m_AISentenceRoot.SetActive(true);
        m_AICardText.text = sentence.key;
        
        m_MasterGroup.volume = 0.5f;
        m_AISentenceAnimator.Play("Sentence_In");
        
        yield return new WaitForSeconds(5);
        
        m_AISentenceAnimator.Play("Sentence_Out");
        m_MasterGroup.volume = 1f;
        yield return new WaitForSeconds(1);
        
        m_AICardText.text = "";
        m_AISentenceRoot.SetActive(false);
    }
}
