using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private int[,] gridArray;
    private float cellSize;
    private float cellSize2;

    private int m_Counter;

    private Sprite m_NoteSprite;
    private SpriteRenderer m_sr;
    // Start is called before the first frame update
    public Grid(int width, int height, float cellSize, Camera Kamera, Vector3 Position, float cellSize2)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.cellSize2 = cellSize2;
        m_Counter = 0;
        gridArray = new int [width,height];
        for (float x = 0; x < gridArray.GetLength(0); x++)
        {
            for (float y= 0; y < gridArray.GetLength(1); y++)
            {
                //Vertical
                Debug.DrawLine(GetWorldPosition(Position.x+x-2.3f,Position.y,-15, m_Counter), GetWorldPosition(Position.x+x-2.3f,Position.y+y,-15, m_Counter), Color.blue, 1000f,false);
                //Horizontal
                //Debug.DrawLine(GetWorldPosition(Position.x,Position.y+y,-15, m_Counter), GetWorldPosition(Position.x-2.3f+x,Position.y+y,-15, m_Counter), Color.green, 1000f,false);
                m_Counter++;
                if(m_Counter == 12)
                m_Counter = 0;
            }
        }
        /* Debug.DrawLine(GetWorldPosition(0,height,1), GetWorldPosition(width,height,1), Color.green, 1000f,false);
        Debug.DrawLine(GetWorldPosition(width,0,1), GetWorldPosition(width,height,1), Color.green, 1000f,false);  */
    }

    public Vector3 GetWorldPosition (float x, float y, float z, int Counter)
    {
        //Convert Grid Position to World
        if(m_Counter == 1 || m_Counter == 3 || m_Counter == 6 || m_Counter == 8 || m_Counter == 10)
        {
            return new Vector3(x,y,z) * cellSize;
        }
        else
        {
            return new Vector3(x,y,z) * cellSize;
        }
        
    }

    public Vector3 GetXY(Vector3 worldPosition)
    {
        //Convert World Position to Grid
        Vector3 Gridposition;
        worldPosition.x = Mathf.Round(worldPosition.x / cellSize);
        worldPosition.y = Mathf.Floor(worldPosition.y / cellSize);
        
        Gridposition = new Vector3(worldPosition.x * cellSize, worldPosition.y * cellSize,0);
//        Debug.Log("Neue Position: " + Gridposition);
        return Gridposition;

    }


}
