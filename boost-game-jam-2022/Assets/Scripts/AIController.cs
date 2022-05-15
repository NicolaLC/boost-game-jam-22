using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    [Serializable]
    private class AIStep
    {
        public int weight = 0;
        public int row = 0;
        public int column = 0;
    }
    
    [SerializeField]
    private Animator m_Animator = null;

    [SerializeField] 
    private SpriteRenderer m_Sprite = null;

    [SerializeField] 
    private List<Sprite> m_Bosses = new List<Sprite>();

    private int m_CurrentBossIndex = 0;

    private bool m_bFirstChoice = true;
    private readonly List<AIStep> m_PossibleMoves = new List<AIStep>();

    private void Awake()
    {
        m_Sprite.sprite = m_Bosses[m_CurrentBossIndex];
    }

    public bool GetNextMove(BoardController i_Board, out int o_Row, out int o_Col)
    {
        // for now, get a random empty cell
        m_PossibleMoves.Clear();
        
        o_Row = -1;
        o_Col = -1;


        void CheckCell(BoardController.CellValue i_Value, int i_Row, int i_Col)
        {
            AIStep step = new AIStep();
            
            if (i_Value == BoardController.CellValue.None)
            {
                step.row = i_Row;
                step.column = i_Col;
                
                m_PossibleMoves.Add(step);
            }
        }

        i_Board.ForeachCell(CheckCell);
        
        if (m_PossibleMoves.Count == 0)
        {
            // no moves to do
            return false;
        }

        AIStep step = new AIStep();
        
        if (m_bFirstChoice)
        {
            step = m_PossibleMoves[Random.Range(0, m_PossibleMoves.Count)];
            
            o_Row = step.row;
            o_Col = step.column;
            
            m_bFirstChoice = false;
            
            return true;
        }

        SetPossibleMovesWeight(i_Board);

        List<AIStep> stepsToStopPlayer = CanWin(i_Board, BoardController.CellValue.Player);
        List<AIStep> stepsToWin = CanWin(i_Board, BoardController.CellValue.AI);

        if (stepsToStopPlayer.Count > 0)
        {
            step = stepsToStopPlayer[0];
            print("Player can win, stop it.");
        } 
        else if (stepsToWin.Count > 0)
        {
            step = stepsToWin[0];
            print("I can win!");
        }
        else
        {
            step = m_PossibleMoves[^1];
        }

        // take the best or the worst decision, randomly
        
        o_Row = step.row;
        o_Col = step.column;
        
        return true;
    }

    private List<AIStep> CanWin(BoardController i_Board, BoardController.CellValue i_Value)
    {
        List<AIStep> result = new List<AIStep>();
        foreach (AIStep possibleMove in m_PossibleMoves)
        {
            // Check row
            int count = 0;
            for (int i = 0; i < 3; ++i)
            {
                if (i_Board.GetValueAt(possibleMove.row, i) == i_Value)
                {
                    count++;
                }
            }

            if (count > 1)
            {
                result.Add(possibleMove);
            }
            
            // Check col
            count = 0;
            for (int i = 0; i < 3; ++i)
            {
                if (i_Board.GetValueAt(i, possibleMove.column) == i_Value)
                {
                    count++;
                }
            }

            if (count > 1)
            {
                result.Add(possibleMove);
            }

            count = 0;
            
            if (possibleMove.row == possibleMove.column)
            {
                //we're on a diagonal
                for (int i = 0; i < 3; i++)
                {
                    if (i_Board.GetValueAt(i, i) == i_Value)
                    {
                        count++;
                    }
                }
                if (count > 1)
                {
                    result.Add(possibleMove);
                }
            }
            
            count = 0;
            
            if (possibleMove.row + possibleMove.column == 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (i_Board.GetValueAt(i, (2) - i) == i_Value)
                    {
                        count++;
                    }
                }
                
                if (count > 1)
                {
                    result.Add(possibleMove);
                }
            }

            count = 0;
        }
        
        return result;
    }

    private void SetPossibleMovesWeight(BoardController i_Board)
    {
        for (var index = 0; index < m_PossibleMoves.Count; index++)
        {
            var step = m_PossibleMoves[index];
            BoardController.CellValue value = i_Board.GetValueAt(step.row, step.column);
            
            step.weight += GetWeight(BoardController.CellValue.AI, i_Board, m_PossibleMoves[index].row, m_PossibleMoves[index].column);
            step.weight += GetWeight(BoardController.CellValue.Player, i_Board, m_PossibleMoves[index].row, m_PossibleMoves[index].column, 10);
        }

        m_PossibleMoves.Sort((x, y) => x.weight.CompareTo(y.weight));
    }

    private int GetWeight(BoardController.CellValue i_Value, BoardController i_Board, int i_Row, int i_Col, int i_Multiplier = 1)
    {
        int weight = 0;

        for (int i = 0; i < 3; ++i)
        {
            if (i_Board.GetValueAt(i, i_Col) == i_Value)
            {
                weight += 1;
            }
        }

        for (int i = 0; i < 3; ++i)
        {
            if (i_Board.GetValueAt(i_Row, i) == i_Value)
            {
                weight += 1;
            }
        }

        if (i_Row == i_Col)
        {
            //we're on a diagonal
            for (int i = 0; i < 3; i++)
            {
                if (i_Board.GetValueAt(i, i) == i_Value)
                {
                    weight += 1;
                }
            }
        }

        if (i_Row + i_Col == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i_Board.GetValueAt(i, (2) - i) == i_Value)
                {
                    weight += 1;
                }
            }
        }

        return weight * i_Multiplier;
    }

    public void DoDeathAnimation()
    {
        m_Animator.Play("Boss_Death");
        StartCoroutine(ChangeBossGraphics());
    }

    private IEnumerator ChangeBossGraphics()
    {
        yield return new WaitForSeconds(1);
        if (m_CurrentBossIndex >= m_Bosses.Count - 1)
        {
            yield break;
        }
        m_Sprite.sprite = m_Bosses[++m_CurrentBossIndex];
    }
}
