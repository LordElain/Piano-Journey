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


    // Start is called before the first frame update
    void Start()
    {
        CreateNoteArray();
        CreateGrid();
        m_sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int x,y;
            //Right Hand Notes
            Vector3 mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Debug.Log("Maus X: " + mousePos.x + " Maus Y: " + mousePos.y);

            Texture2D m_Tex = new Texture2D(500,250);
            m_NoteSprite = Sprite.Create(m_Tex, new Rect(0f,0f,m_Tex.width,m_Tex.height),new Vector2(0,0),100f);
            m_sr.sprite = m_NoteSprite;
            m_sr.transform.position = mousePos;
        }

        if (Input.GetMouseButtonDown(1))
        {
            //Left Hand Notes

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
        grid = new Grid(120, 100, 1.85f, m_Camera, StartPosition);
    }

    public void saveFile()
    {
        
    }
}
