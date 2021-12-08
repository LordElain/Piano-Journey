using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNote : MonoBehaviour
{
    Grid grid;
    public Camera m_Camera;
    public string[] m_NoteKeys;
    public string[] m_AllKeys;
    private Sprite m_NoteSprite;
    private SpriteRenderer m_sr;
    public GameObject m_Note;
    private int m_NoteCounter;
    private List<int> m_NoteCounterArray = new List<int>();

    private bool m_LeftClick;


    // Start is called before the first frame update
    void Start()
    {
        
        m_NoteCounter = 0;
        CreateNoteArray();
        CreateGrid();
        
    }


    // Update is called once per frame
    void Update()
    {
        int x,y;
        
        x = Mathf.FloorToInt(15f);
        y = Mathf.FloorToInt(15f);
        if (Input.GetMouseButtonDown(0))
        {
            //Right Hand Notes
            Vector3 mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            mousePos.x = Mathf.RoundToInt(mousePos.x);
            mousePos.y = Mathf.RoundToInt(mousePos.y);
            Debug.Log("Maus X: " + mousePos.x + " Maus Y: " + mousePos.y);
            m_LeftClick = true;
            CreateNoteBlock(x,y,mousePos,m_LeftClick);
            
        }

        if (Input.GetMouseButtonDown(1))
        {
            //Left Hand Notes
            Vector3 mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Debug.Log("Maus X: " + mousePos.x + " Maus Y: " + mousePos.y);
            m_LeftClick = false;
            CreateNoteBlock(x,y,mousePos,m_LeftClick);
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
        grid = new Grid(81, 100, 1.85f, m_Camera, StartPosition);
    }


    private void CreateNoteBlock(int x, int y, Vector3 mousePos, bool mouseclick)
    {
        GameObject note = Instantiate(m_Note,mousePos, Quaternion.identity);
        Texture2D m_Tex = new Texture2D(x,y);
        m_sr = note.GetComponent<SpriteRenderer>();
        note.name = m_NoteCounter.ToString();
        m_NoteCounterArray.Add(m_NoteCounter);
        m_NoteCounter++;
        m_NoteSprite = Sprite.Create(m_Tex, new Rect(0f,0f,m_Tex.width,m_Tex.height),new Vector2(0,0),100f);
        m_sr.sprite = m_NoteSprite;
        m_sr.transform.position = mousePos;
        
    }

    public void saveFile()
    {
        
    }
}
