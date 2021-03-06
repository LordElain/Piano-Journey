using System.Net.Mime;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;
using SimpleFileBrowser;
using Melanchall.DryWetMidi.Devices;
using System.Threading;
using UnityEngine.UI;

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
    public float m_CellSize2;

    private NotesManager m_NotesManager;
    private TimedEventsManager m_EventManager;
    private ChordsManager m_ChordsManager;
    private TempoMap m_TempoMapManger;
    private TempoMap m_TempoMap;
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
    private bool m_AllowedPlay;
    private bool m_PlayStatus;
    public Playback m_playback;
    private OutputDevice[] m_OutputDevice;
    private TrackChunk m_trackChunk;
    private EventsCollection m_Event;
    private bool m_StatusForPlayback;

    private MidiFile m_TempFile;
    public bool m_Test;
    public int m_MaxOctave;
    private TrackChunk m_NoteChunk;
    private static Playback m_NotePlayback;
    private MidiFile m_NoteFile;
    //Background
    public Image m_Octave;
    public Image m_Octave1;
    public Image m_Octave2;
    public Image m_Octave3;
    public Image m_Octave4;
    public Image m_Octave5;
    public Image m_Octave6;
    

    //Menu

    public GameObject m_OverlayMenu;

    // Start is called before the first frame update
    void Start()
    {
        m_bool = false;
        m_NoteCounter = 0;
        m_SaveState = false;
        m_LoadState = false;
        m_ClickState = true;
        m_AllowedPlay = true;
        FileBrowser.SetDefaultFilter(".mid");
        FileBrowser.AddQuickLink( "Users", "C:\\Users", null );
        m_File = new MidiFile();
        m_TempFile = new MidiFile();
        m_TempoMap = TempoMap.Default;
        m_trackChunk = new TrackChunk();
        m_NoteFile = new MidiFile();
        m_NoteChunk = new TrackChunk();
      
        CreateNoteArray();
        //SetBackground();
        CreateGrid();
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
            m_AllowedPlay = true;
            CreateNoteBlock(x,y,mousePos,mousePosUp,m_LeftClick, m_AllowedPlay);
        }

        if (Input.GetMouseButtonDown(1) && m_ClickState == true )
        {
            //Left Hand Notes
            mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosUp = new Vector3 (0,0,0);
            mousePos.z = 0;
            m_LeftClick = false;
            m_AllowedPlay = true;
            CreateNoteBlock(x,y,mousePos,mousePosUp,m_LeftClick, m_AllowedPlay);
           
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

        if (Input.GetKey(KeyCode.Mouse2) && m_ClickState == true)
        {
            newPos = new Vector3();
            newPos.y = Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime;
            m_Camera.transform.Translate(-newPos);
            var GridOffset = m_GridHeight + 80;

            if (m_Camera.transform.position.y >= GridOffset)
            {
                m_GridHeight = m_GridHeight + 100;
                CreateGrid();
            } 
        }

        if(Input.GetKeyDown(KeyCode.E) && m_ClickState == true)
        {//Delete
            mousePos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            DeleteBlock(mousePos);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_ClickState == true)
            {
                m_ClickState = false;
                m_OverlayMenu.SetActive(true);
            }
            else
            {
                m_ClickState = true;
                m_OverlayMenu.SetActive(false);
            }
            
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

        if (Input.GetKeyDown(KeyCode.Space) && m_ClickState == true)
        {
            
            if(m_PlayStatus == true) 
            {   
                m_playback.InterruptNotesOnStop = true;
                m_PlayStatus = false;
                m_playback.Dispose();
                m_OutputDevice[0].Dispose();
                m_StatusForPlayback = false;
            }
            else
            {
                Playback(m_TempFile, m_StatusForPlayback);
                m_PlayStatus = true;
                m_playback.Stop();
                m_playback.Start();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && m_ClickState == true)
        {
            m_playback.MoveToStart();
        } 

    }
    public void OverlayBackButton()
    {
        
        if (m_playback != null)
        {
            m_playback.Dispose();
            PlaybackCurrentTimeWatcher.Instance.Dispose();
        }
            
        
        if (m_OutputDevice != null)
        {
            m_OutputDevice[0].Dispose();
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("MAIN MENU");
    }
    private bool HitBoxCheck(Vector3 mousePos)
    {
        bool HitStatus;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        print("Check");
        if(Physics.Raycast(ray, out hit))
        {
            print("Hit");
            Debug.Log(hit.transform.tag);
            if (hit.transform.tag == "Key" || hit.transform.tag == "Button")
            {
                HitStatus = true;
            }
            else
            {
                HitStatus = false;
            }
        }
        else
        {
            print("No hit");
            HitStatus = false;
        }
        
        Debug.Log("Hit Status: " + HitStatus);
        return HitStatus;
    }

    private void CreateNoteArray()
       {
        
        int AllKeyCounter = 0;
        m_AllKeys = new string[120];

        //Create WhiteKeys
        for (int i = 1; i <= m_MaxOctave; i++)
        {  
            for (int j = 0; j <= m_NoteKeys.Length-1; j++)
            {    
                
                if(AllKeyCounter < m_AllKeys.Length-1)
                {
                    m_AllKeys[AllKeyCounter] = m_NoteKeys[j]+i;
                    AllKeyCounter++;
                }
                
                
            }; 
           
        };
    }

    private void CreateGrid()
    {
        Vector3 StartPosition = GameObject.Find(m_AllKeys[0] + " Piano").transform.position;
        StartPosition.y = -15f;
        for (int i = 0; i < m_MaxOctave; i++)
        {
            grid = new Grid(m_GridWidth, m_GridHeight, m_CellSize, m_Camera, StartPosition, m_CellSize2);
            StartPosition.x = StartPosition.x + 12.1f;
        }
    }


    private void CreateNoteBlock(int x, int y, Vector3 mousePos, Vector3 mousePosUp, bool mouseclick, bool isAllowedPlay)
    {
        GameObject note = Instantiate(m_Note,mousePos, Quaternion.identity);
        var noteEndTime = Mathf.Floor((note.transform.position.y + transform.lossyScale.y) / m_CellSize);
        var noteonEvent = new NoteOnEvent();
        var PositionTest = Mathf.Floor(mousePos.x);
        
        
        int RED = 0;
        int GREEN = 0;
        int BLUE = 0;

        Texture2D m_Tex = new Texture2D(x,y);
        m_sr = note.GetComponent<SpriteRenderer>();
        note.name = m_NoteCounter.ToString();
        note.SetActive(true);   
        note.tag = "Note";
    
        m_NoteSprite = Sprite.Create(m_Tex, new Rect(0f,0f,m_Tex.width,m_Tex.height),new Vector2(0,0),100f);
        m_sr.sprite = m_NoteSprite;

        if (mouseclick == true)
        {
            RED = PlayerPrefs.GetInt("Color_R");
            GREEN = PlayerPrefs.GetInt("Color_G");
            BLUE = PlayerPrefs.GetInt("Color_B");
            m_sr.color = new Color(RED,GREEN,BLUE);
            noteonEvent.Channel = (FourBitNumber)0;
        }
        else
        {
            RED = PlayerPrefs.GetInt("Color_SR");
            GREEN = PlayerPrefs.GetInt("Color_SG");
            BLUE = PlayerPrefs.GetInt("Color_SB");
            m_sr.color = new Color(RED,GREEN,BLUE);
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
            
          
        
          
           using(m_NotesManager = new NotesManager(m_trackChunk.Events))
           { 
               if(result != null)
                {
                    NotesCollection notes = m_NotesManager.Notes;
                    var allKeyResultNote = m_AllKeys[result.BoxID];
                    string resultNote = Char.ToString(allKeyResultNote[0]);
                    int resultOctave = (int)Char.GetNumericValue(allKeyResultNote[allKeyResultNote.Length-1]);
                    string noteNameOctave = resultNote + resultOctave;
                    var NotePosition = GameObject.Find(noteNameOctave + " Piano").transform.position.y;
                    if (NotePosition < 0)
                    {
                        NotePosition = NotePosition * -1;
                    }
                    var NoteposY = note.transform.position.y;
                    NoteposY = Math.Abs(NotePosition - NoteposY);
                    var NotePos = Mathf.Floor(NoteposY/m_CellSize);
                    
                    NotePos = NotePos * 100.0f;

                    var parsedNote = Melanchall.DryWetMidi.MusicTheory.Note.Parse(allKeyResultNote);
                    notes.Add(new Melanchall.DryWetMidi.Interaction.Note(parsedNote.NoteName,resultOctave)
                    { 
                        Time = Convert.ToInt64(NotePos),
                        Length = LengthConverter.ConvertFrom(MusicalTimeSpan.Quarter, 0, m_TempoMap),
                        Channel = noteonEvent.Channel,
                        Velocity = (SevenBitNumber)45

                    });
                    
                   
                    PlayNote(parsedNote, resultOctave);
                }
                 
                else
                {
                    Debug.Log("ERROR");
                    Debug.Log(result);
                }
             
            }
            m_TempFile.Chunks.Add(m_trackChunk);
            m_TempFile.Write("T", overwriteFile: true);
            
            

        
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
    private void PlayNote(Melanchall.DryWetMidi.MusicTheory.Note PlayNote, int PlayOctave)
    {
        var noteTempoMap = TempoMap.Default;
        m_NoteChunk.RemoveTimedEvents();
        m_NoteChunk = new TrackChunk();
        if(m_OutputDevice != null)
        {
            m_OutputDevice[0].Dispose();
        } 
        using(var notesManager = m_NoteChunk.ManageNotes())
        {
            var length = LengthConverter.ConvertFrom(new MetricTimeSpan(hours: 0, minutes: 0, seconds: 1),0, noteTempoMap);
            var note = new Melanchall.DryWetMidi.Interaction.Note(PlayNote.NoteName, PlayOctave, length);
            notesManager.Notes.Add(note);
        }
        m_NoteFile.Chunks.Add(m_NoteChunk);
        m_NoteFile.Write("Note",overwriteFile: true);
        m_OutputDevice = OutputDevice.GetAll().ToArray();
        m_NotePlayback = m_NoteFile.GetPlayback(m_OutputDevice[0]);

        m_NotePlayback.Play();        
    }

    public void DeleteBlock(Vector3 MousePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
 
        Physics.Raycast(ray, out hit);
        GameObject result = m_NoteCounterArray.Find(
            delegate(GameObject ob)
            {
                return ob.transform.name == hit.transform.name;
            });
            if (result != null)
            {
           
                m_NoteCounterArray.Remove(result);
                result.SetActive(false);
                Destroy(result);
            }
            else
            {
                Debug.Log("NO HIT");
            }
        
      
    }
    public void LoadFile()
    {
        m_LoadState = false;
        Debug.Log("FILE LOADED");
        StartCoroutine( ShowLoadDialogCoroutine() );
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load" );
       
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
            m_AllowedPlay = false;
            int x,y;
        
            x = Mathf.FloorToInt(18.99999f);
            y = Mathf.FloorToInt(18.99999f);
            bool channelCheck = false;
            var notePos = new Vector3(-4f,2f,0);
            foreach (var note in notes)
            {
                float noteTime = note.TimeAs<MetricTimeSpan>(tempo).TotalMicroseconds / 100000.0f;
                int noteNumber = note.NoteNumber;
                var noteName = note.NoteName.ToString();
                var noteOctave = note.Octave.ToString();
                string noteNameOctave = noteName + noteOctave;
                float noteChannel = note.Channel;
                var NotePosition = GameObject.Find(noteNameOctave + " Piano").transform.position;
              
                
                if(noteName == "B" || noteName == "E")
                {
                    NotePosition.x = NotePosition.x - 2.5f;
                }
                else
                {
                    NotePosition.x = NotePosition.x - 1.5f;
                }

                if(noteChannel == 0)
                {
                    channelCheck = true;
                }
                else
                {
                    channelCheck = false;
                }
                Vector3 obj = new Vector3(NotePosition.x,noteTime,-1);
                
                CreateNoteBlock(x,y,obj,obj,channelCheck,false);
            }     
		}
        m_ClickState = false;
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
        //TimedObjectUtilities.ToFile(m_NotesManager);
        m_File.Chunks.Add(m_trackChunk);
        m_File.Write(m_Path,overwriteFile: true);
		}
        m_ClickState = false;
    }

    private void Playback(MidiFile File, bool Status)
    {
        if(Status == false)
        {
            if (m_OutputDevice[0] != null)
            {
                m_OutputDevice[0].Dispose();
            }
            m_OutputDevice = OutputDevice.GetAll().ToArray();
            m_playback = File.GetPlayback(m_OutputDevice[0]);
            m_playback.InterruptNotesOnStop = true;
            m_playback.Stop();
            m_playback.Finished += PlayBackFinished; 
            m_playback.Start(); 
            m_playback.Loop = false;
            m_StatusForPlayback = true;
        }
    }

    private void PlayBackFinished(object sender, EventArgs e)
    {
        Playback play = sender as Playback;
        play.Dispose();
        m_OutputDevice[0].Dispose();

    }

    private void OnApplicationQuit()
    {
        if (m_playback != null)
        {
            m_playback.Stop();
            m_playback.Dispose();
        }
            
        PlaybackCurrentTimeWatcher.Instance.Dispose();
        if (m_OutputDevice != null)
        {
            m_OutputDevice[0].Dispose();
        }
        
    }
}
