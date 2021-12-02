using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNote : MonoBehaviour
{
    Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(20, 10, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Debug.Log(mousePos.x);
            Debug.Log(mousePos.y);
            grid.SetValue(mousePos, 100);
        }   
    }

    public void saveFile()
    {
        
    }
}
