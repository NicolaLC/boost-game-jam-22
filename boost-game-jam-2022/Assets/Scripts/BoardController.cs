using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public enum CellValue
    {
        None,
        Player,
        AI
    }
    
    [Serializable]
    public class Row
    {
        public List<CellController> cells = new List<CellController>();
    }
    
    [SerializeField] 
    private List<Row> m_Rows = new List<Row>();
    
    private List<List<CellValue>> m_Grid = new List<List<CellValue>>();

    public void ForeachCell(Action<CellValue, int, int> i_Callback)
    {
        for (int row = 0; row < 3; ++row)
        {
            for (int col = 0; col < 3; ++col)
            {
                i_Callback(m_Grid[row][col], row, col);
            }
        }
    }

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        for (int row = 0; row < 3; ++row)
        {
            List<CellValue> columns = new List<CellValue>();
            
            for (int col = 0; col < 3; ++col)
            {
                columns.Add(CellValue.None);
                m_Rows[row].cells[col].Setup(row, col);
            }
            
            m_Grid.Add(columns);
        }
    }

    public void Apply(int i_Row, int i_Col, CellValue i_Value, out bool o_bWin)
    {
        o_bWin = false;
        
        if (m_Grid[i_Row][i_Col] != CellValue.None)
        {
            // Cannot change an existing vale
            return;
        }

        m_Grid[i_Row][i_Col] = i_Value;
        m_Rows[i_Row].cells[i_Col].SetValue(i_Value);
        
        //check col
        for (int i = 0; i < 3; i++){
            if(m_Grid[i_Row][i] != i_Value)
            {
                break;
            }
            if(i == 2)
            {
                o_bWin = true;
            }
        }

        if (o_bWin)
        {
            return;
        }

        // check row
        for (int i = 0; i < 3; i++){
            if(m_Grid[i][i_Col] != i_Value)
            {
                break;
            }
            if(i == 2)
            {
                o_bWin = true;
            }
        }
        
        if (o_bWin)
        {
            return;
        }
        
        // check diagonal
        
        //check diag
        if(i_Row == i_Col){
            //we're on a diagonal
            for(int i = 0; i < 3; i++){
                if(m_Grid[i][i] != i_Value)
                {
                    break;
                }
                if(i == 2)
                {
                    o_bWin = true;
                }
            }
        }
        
        if (o_bWin)
        {
            return;
        }

        //check anti diag
        if (i_Row + i_Col == 2){
            for(int i = 0; i < 3; i++){
                if(m_Grid[i][(2)-i] != i_Value)
                {
                    break;
                }
                if(i == 2){
                    o_bWin = true;
                }
            }
        }

    }

    public bool IsFilled()
    {
        for (int row = 0; row < 3; ++row)
        {
            for (int col = 0; col < 3; ++col)
            {
                if (m_Grid[row][col] == CellValue.None)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public CellValue GetValueAt(int i_Row, int i_Col)
    {
        return m_Grid[i_Row][i_Col];
    }
}
