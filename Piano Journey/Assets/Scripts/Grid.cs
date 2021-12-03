using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private int[,] gridArray;
    private float cellSize;
    // Start is called before the first frame update
    public Grid(int width, int height, float cellSize, Camera Kamera, Vector3 Position)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        
        gridArray = new int[width,height];
        Debug.Log(Position);
        Debug.Log(gridArray.Length);
        for (float x = 0; x < gridArray.GetLength(0); x++)
        {
            for (float y= 0; y < gridArray.GetLength(1); y++)
            {
                Debug.Log(x + "x " + y + "y " );
                //Vertical
                Debug.DrawLine(GetWorldPosition(Position.x+x-2.5f,Position.y-7f), GetWorldPosition(Position.x+x-2.5f,Position.y+y-7f), Color.blue, 1000f,false);
                //Horizontal
                Debug.DrawLine(GetWorldPosition(Position.x+x,Position.y+y-7f), GetWorldPosition(Position.x-2.5f+x,Position.y+y-7f), Color.green, 1000f,false);
            }
        }
        /* Debug.DrawLine(GetWorldPosition(0,height), GetWorldPosition(width,height), Color.green, 1000f,false);
        Debug.DrawLine(GetWorldPosition(width,0), GetWorldPosition(width,height), Color.green, 1000f,false); */
    }

    private Vector3 GetWorldPosition (float x, float y)
    {
        return new Vector3(x,y) * cellSize;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y )
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.y / cellSize);
        Debug.Log(x + " " + y);
    }

    public void SetValue(int x, int y, int value)
    {
        Debug.Log(x+" " + y + " " + value);
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x,y] = value;
            Debug.Log("YO" + gridArray[x,y]);
            Debug.Log(gridArray[x+3,y+4]);
            
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        
        GetXY(worldPosition, out x, out y);
        SetValue(x,y,value);
    }
}
