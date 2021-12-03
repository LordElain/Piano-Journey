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
        for (float x = Kamera.transform.position.x; x < gridArray.GetLength(0); x++)
        {
            for (float y= Kamera.transform.position.y; y < gridArray.GetLength(1); y++)
            {
                Debug.Log("x " + x + " " + y);
                Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x,y+1), Color.green, 1000f,false);
                Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x+1,y), Color.green, 1000f,false);
            }
        }
        Debug.DrawLine(GetWorldPosition(0,height), GetWorldPosition(width,height), Color.green, 1000f,false);
        Debug.DrawLine(GetWorldPosition(width,0), GetWorldPosition(width,height), Color.green, 1000f,false);
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
