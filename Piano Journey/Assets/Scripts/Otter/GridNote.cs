using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;
using SimpleFileBrowser;

public class GridNote : MonoBehaviour
{
    Grid grid;
    NoteBox m_NoteBox;
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
    private List<Melanchall.DryWetMidi.Interaction.Note> m_Notelist;
    private List <Melanchall.DryWetMidi.Interaction.Chord> m_Chordlist;

    private List <NoteBox> m_BoxIDList = new List<NoteBox>();
    private int m_BoxID;
    private int m_BoxIDCounter;

    private MidiFile m_File;
    public string m_Path;
    public string m_FileName; 
    private Vector3 PositionID;

    private bool m_bool;

    public bool m_SaveState;
    public bool m_LoadState;
    public bool m_ClickState;
    private TrackChunk m_trackChunk = new TrackChunk();


    // Start is called before the first frame update
    void Start()
    {
        m_bool = false;
        m_NoteCounter = 0;
        m_SaveState = false;
        m_LoadState = false;
        m_ClickState = true;
        FileBrowser.SetDefaultFilter(".mid");
        FileBrowser.AddQuickLink( "Users", "C:\\Users", null );
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
        if (Input.GetMouseButtonDown(0) && m_ClickState == true)
        {
            //Right Hand Notes
            mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosUp = new Vector3 (0,0,0);
            mousePos.z = 0;
            m_LeftClick = true;
            CreateNoteBlock(x,y,mousePos,mousePosUp,m_LeftClick);
        }

        if (Input.GetMouseButtonDown(1) && m_ClickState == true)
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

        if (Input.GetKey(KeyCode.Mouse2) && Input.GetKey(KeyCode.Space) && m_ClickState == true)
        {
            newPos = new Vector3();
            newPos.y = Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime;
            m_Camera.transform.Translate(-newPos);
            var GridOffset = m_GridHeight + 80;

            Debug.Log(m_Camera.transform.position);
            Debug.Log(m_GridHeight);
            if (m_Camera.transform.position.y >= GridOffset)
            {
                m_GridHeight = m_GridHeight + 100;
                Debug.Log(m_GridHeight  + "New Height");
                Vector3 NewPosition= GameObject.Find(m_AllKeys[1] + " Piano").transform.position;
                Debug.Log(NewPosition);
                CreateGrid(NewPosition);
            } 
        }

        if(Input.GetKey(KeyCode.E) && m_ClickState == true)
        {//Delete

        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Z) && m_ClickState == true)
        {//Reverse Last Action
        
        }
      
        if (m_SaveState == true)
        {
            m_ClickState = false;
            SaveFile();
        }

        if (m_LoadState == true)
        {
            m_ClickState = false;
            LoadFile();
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
        var noteEndTime = note.transform.position.y + transform.lossyScale.y;
        var noteonEvent = new NoteOnEvent();
        var PositionTest = Mathf.Floor(mousePos.x);
        Texture2D m_Tex = new Texture2D(x,y);
        m_sr = note.GetComponent<SpriteRenderer>();
        note.name = m_NoteCounter.ToString();
        
    


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
    
        if (m_bool == false)
        {
            PositionID = new Vector3 (-2,0,0);
            m_NoteCounter = 0;
            for (int i = 0; i < 240; i++)
            {
                switch (m_NoteCounter)
                {
                    case 0: case 1: case 2: case 4:  
                    {
     
                        m_NoteCounter++;
                        PositionID.x = PositionID.x+1f; 
                        m_BoxIDList.Add(new NoteBox{BoxID = i, BoxPos = PositionID.x});
                        break;
                    }
                    case 5: case 6: case 7: case 8: case 9: case 10:
                    {
                        
                        m_NoteCounter++;
                        PositionID.x = PositionID.x+1f;
                        m_BoxIDList.Add(new NoteBox{BoxID = i, BoxPos = PositionID.x});
                        break;
                    }
                
                    case 3: 
                    {
                        
                        m_NoteCounter++;
                        PositionID.x = PositionID.x+1;
                        m_BoxIDList.Add(new NoteBox{BoxID = i, BoxPos = PositionID.x});
                        break;
                    }
                    case 11:
                    {
                        
                        m_NoteCounter = 0;
                        PositionID.x = PositionID.x+1;
                        m_BoxIDList.Add(new NoteBox{BoxID = i, BoxPos = PositionID.x});
                        break;
                    }
                    default:
                    {
                        Debug.Log("Error" + m_NoteCounter);
                        break;
                    }
                }
            
            }
            m_bool = true;
        }
        NoteBox result = m_BoxIDList.Find(
            delegate(NoteBox nb)
            {
                return nb.BoxPos == Mathf.Floor(m_sr.transform.position.x /m_CellSize);
            }

        );
            m_NotesManager = m_trackChunk.ManageNotes();
        
            NotesCollection notes = m_NotesManager.Notes;
            if(result != null)
            {
                var allKeyResultNote = m_AllKeys[result.BoxID];
                string resultNote = Char.ToString(allKeyResultNote[0]);
                int resultOctave = (int)Char.GetNumericValue(allKeyResultNote[allKeyResultNote.Length-1]);


                var parsedNote = Melanchall.DryWetMidi.MusicTheory.Note.Parse(allKeyResultNote);
                notes.Add(new Melanchall.DryWetMidi.Interaction.Note(parsedNote.NoteName,resultOctave)
                {
                    Time = Convert.ToInt64(note.transform.position.y),
                    Length = Convert.ToInt64(noteEndTime),
                    Channel = noteonEvent.Channel,
                    Velocity = (SevenBitNumber)45

                }); 
            m_NotesManager.SaveChanges();
            }
            else
            {
                Debug.Log("ERROR");
                Debug.Log(result);
            }

            
        
        

        
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
        m_LoadState = false;
        Debug.Log("FILE LOADED");
        StartCoroutine( ShowLoadDialogCoroutine() );
        
        
/*         foreach(var note in m_File.GetNotes())
        {
            m_Notelist.Add(note);
        }
        foreach (var note in m_File.GetChords())
        {
            m_Chordlist.Add(note);
        } */
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load" );
        Debug.Log( FileBrowser.Success );

		if( FileBrowser.Success )
		{
			// Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
			for( int i = 0; i < FileBrowser.Result.Length; i++ )
            {
                Debug.Log( FileBrowser.Result[i] );
                
            }
			m_Path = FileBrowser.Result[0];	
            m_File = MidiFile.Read(m_Path);
            
            var m_Duration = m_File.GetDuration<MetricTimeSpan>();
            TempoMap tempo = m_File.GetTempoMap();
            IEnumerable<Melanchall.DryWetMidi.Interaction.Note> notes = m_File.GetNotes();

            var notePos = new Vector3(-4f,0,0);
            foreach (var note in notes)
            {
                float noteTime = note.TimeAs<MetricTimeSpan>(tempo).TotalMicroseconds / 100000.0f;
                int noteNumber = note.NoteNumber;
                var noteName = note.NoteName.ToString();
                var noteOctave = note.Octave.ToString();
                string noteNameOctave = noteName + noteOctave;
                float noteLength = note.LengthAs<MetricTimeSpan>(tempo).TotalMicroseconds / 100000f;
                float noteChannel = note.Channel;

                var NotePosition = GameObject.Find(noteNameOctave + " Piano").transform.position;
                Debug.Log(NotePosition);
                
                if(noteName == "B" || noteName == "E")
                {
                    NotePosition.x = NotePosition.x - 2.5f;
                }
                else
                {
                    NotePosition.x = NotePosition.x - 1.5f;
                }
                GameObject noteObject = Instantiate(m_Note, notePos, Quaternion.identity);
                
                //Debug.Log(noteNumber+noteOffset*noteOffset + " " + noteNameOctave);
                noteObject.GetComponent<GameNote>().InitGameNote(noteTime,NotePosition.x,noteLength,noteChannel,noteNameOctave, noteName);
                noteObject.SetActive(true);
                noteObject.name = noteNameOctave;
                noteObject.tag = "Note";
            }     
            

		}
        m_ClickState = true;
    }
    public void SaveFile()
    {
        m_SaveState = false;
        Debug.Log("FILE SAVING");
        StartCoroutine(ShowSaveDialogCoroutine());
       
        
    }

    IEnumerator ShowSaveDialogCoroutine()
    {
        yield return FileBrowser.WaitForSaveDialog( FileBrowser.PickMode.FilesAndFolders, true, null, null, "Save Files and Folders", "Save" );
        Debug.Log( FileBrowser.Success );

		if( FileBrowser.Success )
		{
			// Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
			for( int i = 0; i < FileBrowser.Result.Length; i++ )
            {
                Debug.Log( FileBrowser.Result[i] );
                
            }
			m_Path = FileBrowser.Result[0];	
            m_Path = m_Path + ".mid";
            m_File = new MidiFile();
            m_File.Chunks.Add(m_trackChunk);
        //TimedObjectUtilities.ToFile(m_NotesManager);
        m_File.Write(m_Path,true);
		}
        m_ClickState = true;
    }
}
