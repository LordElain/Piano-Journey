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

    public float dragSpeed = 2;
    private Vector3 newPos;


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
        
        x = Mathf.FloorToInt(18.99999f);
        y = Mathf.FloorToInt(18.99999f);
        if (Input.GetMouseButtonDown(0))
        {
            //Right Hand Notes
            Vector3 mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            /* mousePos.x = Mathf.Round(mousePos.x);
            mousePos.y = Mathf.Round(mousePos.y); */
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

        if (Input.GetKey(KeyCode.Mouse2) && Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Camera Movement");
            newPos = new Vector3();
            newPos.y = Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime;
            m_Camera.transform.Translate(-newPos);
        }

       /*  Vector3 pos = m_Camera.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Debug.Log("pos" + pos);
        Vector3 move = new Vector3(0, 0, pos.y * dragSpeed);
 
        m_Camera.transform.Translate(move, Space.World);  */
        
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

        if (mouseclick == true)
        {
            m_sr.color = Color.red;
        }
        else
        {
            m_sr.color = Color.yellow;
        }
        m_sr.transform.position = grid.GetXY(mousePos);
        
    }


    public void saveFile()
    {
        
    }
}
