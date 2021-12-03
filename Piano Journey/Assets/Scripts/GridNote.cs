using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNote : MonoBehaviour
{
    Grid grid;
    public Camera m_Camera;
    public string[] m_NoteKeys;
    public string[] m_AllKeys;

    // Start is called before the first frame update
    void Start()
    {
        CreateNoteArray();
        CreateGrid();
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

    private void CreateNoteArray()
    {
        int MaxOctave = 8;
        int AllKeyCounter = 1;
        //Create WhiteKeys
        for (int i = 0; i < MaxOctave; i++)
        {  
            for (int j = 0; j <= m_NoteKeys.Length-1; j++)
            {    
                m_AllKeys[AllKeyCounter] = m_NoteKeys[j]+i;
                AllKeyCounter++;
            }; 
           
        };
    }

    private void CreateGrid()
    {
        Debug.Log(m_AllKeys[1]);
        Vector3 StartPosition = GameObject.Find(m_AllKeys[1] + " Piano").transform.position;
        Vector3 Pos = GameObject.Find("Piano").transform.position;
        Debug.Log(StartPosition);
        grid = new Grid(200, 100, 3f, m_Camera, StartPosition);
    }

    public void saveFile()
    {
        
    }
}
