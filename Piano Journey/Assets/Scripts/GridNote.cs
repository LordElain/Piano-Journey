using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
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

    public int m_GridWidth;
    public int m_GridHeight;
    public float m_CellSize;

    private NotesManager m_NotesManager;
    private TimedEventsManager m_EventManager;
    private ChordsManager m_ChordsManager;
    private TempoMap m_TempoMapManger;
    private List<Note> m_Notelist;
    private List <Chord> m_Chordlist;

    private MidiFile m_File;
    public string m_Path;


    // Start is called before the first frame update
    void Start()
    {
        
        m_NoteCounter = 0;
        CreateNoteArray();
        Vector3 StartPosition = GameObject.Find(m_AllKeys[1] + " Piano").transform.position;
        CreateGrid(StartPosition);
        
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
            m_LeftClick = true;
            CreateNoteBlock(x,y,mousePos,mousePosUp,m_LeftClick);
        }

        if (Input.GetMouseButtonDown(1))
        {
            //Left Hand Notes
            mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosUp = new Vector3 (0,0,0);
            mousePos.z = 0;
            m_LeftClick = false;
            CreateNoteBlock(x,y,mousePos,mousePosUp,m_LeftClick);
        } 

        /* if (Input.GetMouseButton(0))
        {
            mousePosUp = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosUp.z = 0;
            Debug.Log("Maus Hold");
            Debug.Log(mousePos + "Pos" + mousePosUp + "Up");
            m_LeftClick = true;
            CreateNoteBlock(x,y,mousePos,mousePosUp,m_LeftClick);
        }   */

        if (Input.GetKey(KeyCode.Mouse2) && Input.GetKey(KeyCode.Space))
        {
            newPos = new Vector3();
            newPos.y = Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime;
            m_Camera.transform.Translate(-newPos);
            var GridOffset = m_GridHeight + 80;

            Debug.Log(m_Camera.transform.position);
            Debug.Log(m_GridHeight);
            if (m_Camera.transform.position.y >= GridOffset)
            {
                Debug.Log("HERE");
                m_GridHeight = m_GridHeight + 100;
                Debug.Log(m_GridHeight  + "New Height");
                Vector3 NewPosition= GameObject.Find(m_AllKeys[1] + " Piano").transform.position;
                Debug.Log(NewPosition);
                CreateGrid(NewPosition);
            } 
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

    private void CreateGrid(Vector3 StartPosition)
    {
        grid = new Grid(m_GridWidth, m_GridHeight, m_CellSize, m_Camera, StartPosition);
    }


    private void CreateNoteBlock(int x, int y, Vector3 mousePos, Vector3 mousePosUp, bool mouseclick)
    {
        GameObject note = Instantiate(m_Note,mousePos, Quaternion.identity);
        var noteonEvent = new NoteOnEvent();
        var Test = GameObject.FindWithTag("Key");
        Debug.Log(Test);
        var PositionTest = Mathf.Floor(mousePos.x);
        Texture2D m_Tex = new Texture2D(x,y);
        m_sr = note.GetComponent<SpriteRenderer>();
        note.name = m_NoteCounter.ToString();
        int KeyPosition = (int)PositionTest;
      
        Debug.Log(KeyPosition);
        Debug.Log(m_AllKeys[KeyPosition]);
        
        //CheckForParent(m_NoteCounterArray);
        m_NoteSprite = Sprite.Create(m_Tex, new Rect(0f,0f,m_Tex.width,m_Tex.height),new Vector2(0,0),100f);
        m_sr.sprite = m_NoteSprite;

        if (mouseclick == true)
        {
            m_sr.color = Color.red;
            noteonEvent.Channel = (FourBitNumber)0;
        }
        else
        {
            m_sr.color = Color.yellow;
            noteonEvent.Channel = (FourBitNumber)1;
        }
        
        m_sr.transform.position = grid.GetXY(mousePos);
        if (!m_NoteCounterArray.Equals(note))
        {
            for (int i = 0; i < m_NoteCounterArray.Count; i++)
            {
                //Debug.Log("i: " + i + "List Length" + m_NoteCounterArray.Count + "Pos" + m_sr.transform.position + "List Pos" + m_NoteCounterArray[i].transform.position);
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

    public void LoadFile()
    {
        m_File = MidiFile.Read(m_Path);
        foreach(var note in m_File.GetNotes())
        {
            m_Notelist.Add(note);
        }
        foreach (var note in m_File.GetChords())
        {
            m_Chordlist.Add(note);
        }
    }
    public void SaveFile()
    {
        
    }
}
