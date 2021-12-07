using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private int[,] gridArray;
    private float cellSize;

    private Sprite m_NoteSprite;
    private SpriteRenderer m_sr;
    // Start is called before the first frame update
    public Grid(int width, int height, float cellSize, Camera Kamera, Vector3 Position)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        
        gridArray = new int[width,height];
        for (float x = 0; x < gridArray.GetLength(0); x++)
        {
            for (float y= 0; y < gridArray.GetLength(1); y++)
            {
                //Vertical
                Debug.DrawLine(GetWorldPosition(Position.x+x-2.4f,Position.y-11f), GetWorldPosition(Position.x+x-2.4f,Position.y+y-11f), Color.blue, 1000f,false);
                //Horizontal
                Debug.DrawLine(GetWorldPosition(Position.x+x,Position.y+y-11f), GetWorldPosition(Position.x-2.4f+x,Position.y+y-11f), Color.green, 1000f,false);
            }
        }
        /* Debug.DrawLine(GetWorldPosition(0,height), GetWorldPosition(width,height), Color.green, 1000f,false);
        Debug.DrawLine(GetWorldPosition(width,0), GetWorldPosition(width,height), Color.green, 1000f,false); */
    }

    public Vector3 GetWorldPosition (float x, float y)
    {
        //Convert Grid Position to World
        return new Vector3(x,y) * cellSize;
    }

    public Vector3 GetXY(Vector3 worldPosition)
    {
        //Convert World Position to Grid
        Vector3 Gridposition;
        worldPosition.x = worldPosition.x / cellSize;
        worldPosition.y = worldPosition.y / cellSize;
        Gridposition = new Vector3(worldPosition.x, worldPosition.y,0);
        Debug.Log("Neue Position: " + Gridposition);
        return Gridposition;

    }

    public void SetValue(int x, int y, int value)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x,y] = value;
            Debug.Log("YO" + gridArray[x,y]);
            Debug.Log(gridArray[x+3,y+4]);
            
        }

      
    }

   /*  public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        
        GetXY(worldPosition, out x, out y);
        SetValue(x,y,value);
    } */
}
