using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Game : SingletonPattern<Game>
{
    [SerializeField] 
    private BoardController m_Board = null;
    [SerializeField] 
    private CameraController m_PlayerCamera = null;
    [SerializeField]
    private AIController m_AI = null;

    private int m_GameAwakenCount = 0; // max 5
    private int m_PlayerActiveRunes = 2;
    private bool m_bGameStopped = false;
    
    public enum TurnTarget
    {
        Player,
        AI
    }

    private TurnTarget m_CurrentTarget;

    private Coroutine m_WaitForAIActionCoroutine = null;
    
    private Coroutine m_WaitForPlayerActionCoroutine = null;
    private bool m_bPlayerSelectedCell = false;

    private bool m_bGameStarted = false;

    public static TurnTarget CurrentTarget => Instance.m_CurrentTarget;
    public static bool GameStarted => Instance.m_bGameStarted;
    public static bool GameRunning => !Instance.m_bGameStopped;

    public static void CellSelected(int i_Row, int i_Col, TurnTarget i_Caller)
    {
        if (i_Caller != Instance.m_CurrentTarget)
        {
            return;
        }
        
        Instance.Internal_CellSelected(i_Row, i_Col);
    }

    protected override void Start()
    {
        base.Start();
        
        m_CurrentTarget = Random.value > 0.5f ? TurnTarget.AI : TurnTarget.Player;

        StartCoroutine(StartGame());
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Console.Clear();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_GameAwakenCount++;
            GameUIController.OnPlayerScore(m_GameAwakenCount);
            
            if (m_GameAwakenCount >= 5)
            {
                StartCoroutine(PlayerWin());
            }
            else
            {
                StartCoroutine(RestartGame(true));
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_PlayerActiveRunes--;
            GameUIController.OnAIScore(m_PlayerActiveRunes);
            
            if (m_PlayerActiveRunes <= 0)
            {
                StartCoroutine(PlayerLose());
            }
            else
            {
                StartCoroutine(RestartGame(false));
            }
        }
#endif
    }
    
    private IEnumerator StartGame()
    {
        if (PlayerPrefs.GetInt("first_play") == 0)
        {
            PlayerPrefs.SetInt("first_play", 1);
            GameUIController.ToggleTutorial(true);
        }
        GameMessage.OnGameStart();
        
        yield return new WaitForSeconds(3);

        m_bGameStarted = true;
        
        NextTurn();
    }

    private void NextTurn()
    {
        if (m_bGameStopped)
        {
            return;
        }

        if (m_CurrentTarget == TurnTarget.AI)
        {
            Debug.Log("[GAME] AI Turn.");

            if (m_WaitForAIActionCoroutine != null)
            {
                StopCoroutine(m_WaitForAIActionCoroutine);
            }
            
            m_WaitForAIActionCoroutine = StartCoroutine(WaitForAIAction());
        }
        else
        {
            Debug.Log("[GAME] Player Turn.");

            if (m_WaitForPlayerActionCoroutine != null)
            {
                StopCoroutine(m_WaitForPlayerActionCoroutine);
            }
            
            m_WaitForPlayerActionCoroutine = StartCoroutine(WaitForPlayerAction());
        }
    }
    
    private void ChangeTarget()
    {
        if (m_bGameStopped)
        {
            return;
        }
        m_CurrentTarget = m_CurrentTarget == TurnTarget.Player ? TurnTarget.AI : TurnTarget.Player;
    }

    private IEnumerator WaitForPlayerAction()
    {
        yield return new WaitForSeconds(1);
        
        GameMessage.OnNextTurn();
        m_PlayerCamera.SetBoardLookPosition();
        
        while (!m_bPlayerSelectedCell)
        {
            yield return new WaitForEndOfFrame();
        }
        
        GameSentences.ShowSentence(true);

        m_bPlayerSelectedCell = false;
    }

    private void Internal_CellSelected(int i_Row, int i_Col)
    {
        if (m_bGameStopped)
        {
            return;
        }

        m_bGameStopped = true;
        
        if (m_CurrentTarget == TurnTarget.Player)
        {
            if (m_bPlayerSelectedCell)
            {
                return;
            }
            
            m_bPlayerSelectedCell = true;
        }

        BoardController.CellValue cellValue = m_CurrentTarget == TurnTarget.Player
            ? BoardController.CellValue.Player
            : BoardController.CellValue.AI;
        
        m_Board.Apply(i_Row, i_Col, cellValue, out bool i_bWin);

        StartCoroutine(MoveToNextTurn(i_bWin));
    }

    private IEnumerator MoveToNextTurn(bool i_bWin)
    {
        yield return new WaitForSeconds(5);

        m_bGameStopped = false;

        if (i_bWin)
        {
            GameEnded();
        } else if (!i_bWin && m_Board.IsFilled())
        {
            GameEnded(true);
        }
        else
        {
            ChangeTarget();
            NextTurn();
        }
    }

    private IEnumerator WaitForAIAction()
    {
        CursorManager.SetDenied();
        CursorManager.Lock();

        yield return new WaitForSeconds(1);
        
        GameMessage.OnNextTurn();
        m_PlayerCamera.SetPlayerPosition();

        yield return new WaitForSeconds(UnityEngine.Random.Range(2, 5));
        
        bool canMove = m_AI.GetNextMove(m_Board, out int row, out int col);
            
        if (canMove)
        {
            GameSentences.ShowSentence(false);

            Internal_CellSelected(row, col);
        }
        
        CursorManager.Unlock();
        CursorManager.SetDenied();
    }
    
    private void GameEnded(bool i_bTie = false)
    {
        if (i_bTie)
        {
            print("Game ended - nobody wins");

            StartCoroutine(RestartGame(false, true));
        }
        else
        {
            print($"Game ended - {m_CurrentTarget} wins");

            if (m_CurrentTarget == TurnTarget.Player)
            {
                m_GameAwakenCount++;
                
                GameUIController.OnPlayerScore(m_GameAwakenCount);

                if (m_GameAwakenCount >= 5)
                {
                    StartCoroutine(PlayerWin());
                }
                else
                {
                    StartCoroutine(RestartGame(true));
                }
            }
            else
            {
                m_PlayerActiveRunes--;

                GameUIController.OnAIScore(m_PlayerActiveRunes);
                
                if (m_PlayerActiveRunes <= 0)
                {
                    StartCoroutine(PlayerLose());
                }
                else
                {
                    StartCoroutine(RestartGame(false));
                }
            }
        }
    }

    private IEnumerator RestartGame(bool i_bPlayerWin, bool i_bTie = false)
    {
        m_PlayerCamera.SetPlayerPosition();

        if(m_WaitForPlayerActionCoroutine != null) StopCoroutine(m_WaitForPlayerActionCoroutine);
        if(m_WaitForAIActionCoroutine != null) StopCoroutine(m_WaitForAIActionCoroutine);
        
        m_bGameStopped = true;

        if (i_bPlayerWin)
        {
            m_AI.DoDeathAnimation();
        }
        
        yield return new WaitForSeconds(0.5f);
        
        GameMessage.OnNewGame(m_PlayerActiveRunes, i_bPlayerWin, i_bTie);
        yield return new WaitForSeconds(2);
        
        
        SceneTransition.Show();
        yield return new WaitForSeconds(2);
        
        m_Board.Reset();
        
        SceneTransition.Hide();
        yield return new WaitForSeconds(2);

        m_bGameStopped = false;

        ChangeTarget();
        NextTurn();
    }

    private IEnumerator PlayerWin()
    {
        m_bGameStopped = true;

        CursorManager.SetDefault(true);
        
        Debug.Log("Player win - show win screen");
        
        m_AI.DoDeathAnimation();
                
        m_PlayerCamera.SetEndGamePosition();
        
        yield return new WaitForSeconds(2);

        CursorManager.SetDefault(true);
        
        EndGamePanels.OnWin();
    }
    
    private IEnumerator PlayerLose()
    {
        m_bGameStopped = true;
        
        CursorManager.SetDefault(true);
        
        Debug.Log("Player lose - show lose screen");
        
        m_PlayerCamera.SetEndGamePosition();
        
        yield return new WaitForSeconds(2);
        
        CursorManager.SetDefault(true);
        
        EndGamePanels.OnLose();
    }
}
