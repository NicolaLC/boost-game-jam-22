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
        string name = Game.CurrentTarget == Game.TurnTarget.Player ? "TORSTEN" : "DEMON";
        m_GameMessageText.Show($"{name} TURN");
    }
}
