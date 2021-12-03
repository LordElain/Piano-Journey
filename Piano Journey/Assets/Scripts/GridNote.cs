using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNote : MonoBehaviour
{
    Grid grid;
    public Camera m_Camera;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 Pos = GameObject.Find("Piano").transform.position;
        Debug.Log(m_Camera.transform.position);
        Debug.Log(Pos);
        grid = new Grid(200, 100, 3, m_Camera, Pos);
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
