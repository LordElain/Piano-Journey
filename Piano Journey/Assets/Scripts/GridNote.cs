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
    private List<GameObject> m_NoteCounterArray = new List<GameObject>();

    private bool m_LeftClick;

    public float dragSpeed = 2;
    private Vector3 newPos;
    private Vector3 mousePos;
    private Vector3 mousePosUp;


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
            mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosUp = new Vector3 (0,0,0);
            mousePos.z = 0;
            Debug.Log("Maus X: " + mousePos.x + " Maus Y: " + mousePos.y);
            m_LeftClick = true;
            CreateNoteBlock(x,y,mousePos,mousePosUp,m_LeftClick);
        }

        if (Input.GetMouseButtonDown(1))
        {
            //Left Hand Notes
            mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosUp = new Vector3 (0,0,0);
            mousePos.z = 0;
            Debug.Log("Maus X: " + mousePos.x + " Maus Y: " + mousePos.y);
            m_LeftClick = false;
            CreateNoteBlock(x,y,mousePos,mousePosUp,m_LeftClick);
        } 

        if (Input.GetMouseButton(0))
        {
            mousePosUp = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosUp.z = 0;
            Debug.Log("Maus Hold");
            Debug.Log(mousePos + "Pos" + mousePosUp + "Up");
            m_LeftClick = true;
            CreateNoteBlock(x,y,mousePos,mousePosUp,m_LeftClick);
        }  

        if (Input.GetKey(KeyCode.Mouse2) && Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Camera Movement");
            newPos = new Vector3();
            newPos.y = Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime;
            m_Camera.transform.Translate(-newPos);
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


    private void CreateNoteBlock(int x, int y, Vector3 mousePos, Vector3 mousePosUp, bool mouseclick)
    {
        GameObject note = Instantiate(m_Note,mousePos, Quaternion.identity);
   /*      if (mousePosUp != mousePos)
        {
            Vector3 scale = new Vector3 (10, 10+(mousePosUp.y - mousePos.y), 10);
            note.transform.localScale = scale; 
        } */
        
        Texture2D m_Tex = new Texture2D(x,y);
        m_sr = note.GetComponent<SpriteRenderer>();
        note.name = m_NoteCounter.ToString();
        
        //CheckForParent(m_NoteCounterArray);
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
        if (!m_NoteCounterArray.Equals(note))
        {
            Debug.Log("Kommt rein");
            for (int i = 0; i < m_NoteCounterArray.Count; i++)
            {
                Debug.Log("i: " + i + "List Length" + m_NoteCounterArray.Count + "Pos" + m_sr.transform.position + "List Pos" + m_NoteCounterArray[i].transform.position);
                if (m_NoteCounterArray[i].transform.position == m_sr.transform.position)
                {
                    note.SetActive(false);
                    Debug.Log("Same Position");
                }
            }
            m_NoteCounterArray.Add(note);
            m_NoteCounter++;
        }
        
    }

    private void CheckForParent (List<int> NoteArray)
    {

    }

    public void saveFile()
    {
        
    }
}
