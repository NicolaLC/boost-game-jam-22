using TMPro;
using UnityEngine;

public class GameMessage : SingletonPattern<GameMessage>
{

    [SerializeField] 
    private UIElement_GameMessage m_GameMessageText = null;

    public static void OnGameStart()
    {
        Instance.Internal_OnGameStart();
    }
    

    private void Internal_OnGameStart()
    {
        m_GameMessageText.Show($"AWAKE TORSTEN!");
    }    
    
    public static void OnNextTurn()
    {
        Instance.Internal_OnNextTurn();
    }

    private void Internal_OnNextTurn()
    {
        string name = Game.CurrentTarget == Game.TurnTarget.Player ? "YOUR" : "DEMONS";
        m_GameMessageText.Show($"{name} TURN");
    }
    
    public static void OnNewGame(int i_PlayerLives, bool i_bPlayerWins)
    {
        Instance.Internal_OnNewGame(i_PlayerLives, i_bPlayerWins);
    }
    
    private void Internal_OnNewGame(int i_PlayerLives, bool i_bPlayerWins)
    {
        if (!i_bPlayerWins)
        {
            m_GameMessageText.Show($"Demon wins this turn.\n{i_PlayerLives} lives remaining.");
        }
        else
        {
            m_GameMessageText.Show($"You win this turn.");
        }
    }

}
