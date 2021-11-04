using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Devices;
using Melanchall.DryWetMidi.Standards;
using System.Threading;
using System.Linq;



public class NotesController : MonoBehaviour, PianoJourney.IPlayerActions
{
    //MIDI Properties
    PianoJourney controls;
    public int m_BPM;
    public float m_Position;
    public float m_TimePlayed;

    public string m_Path;


    //Playback Related
    public DevicesConnector m_DeviceConnector;
    public InputDevice[] m_InputDevices;
    public OutputDevice[] m_OutputDevices;
    public static Playback m_playback;
    public bool m_PlayStatus;
    public GameObject m_Camera;



    //Notes Generating
    public GameObject m_Prefab_Notes;
  //  public GameObject m_Prefab_Grid;
    private int[] m_Notes;
    [SerializeField] private int m_Row;
    [SerializeField] private int m_Column;
    [SerializeField] private float m_XStartPos;
    [SerializeField] private float m_YStartPos;
    [SerializeField] private float m_XSpace;
    [SerializeField] private float m_YSpace;




    // Start is called before the first frame update
    void Start()
    {
        controls = new PianoJourney();
        controls.Player.SetCallbacks(this);
        controls.Enable();
        var m_File = ReadFile(m_Path);
        var m_Duration = GetDuration(m_File);
       // var m_Camera = GameObject.Find("Main Camera");

        
        m_InputDevices = GetInputDevices();
        m_OutputDevices = GetOutputDevices();
        setDevices(m_InputDevices, m_OutputDevices);

       

        DisplayNotes(m_File, m_Duration, m_Prefab_Notes);
        StartCoroutine(PlayMidi(m_File, m_OutputDevices,m_Duration, m_PlayStatus, m_Camera));
        //WriteNotes(m_InputDevices, m_OutputDevices);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        var m_File = ReadFile(m_Path);
        var m_Duration = GetDuration(m_File);
        var m_NoteObject = GameObject.Find("ObjectNotes");
        var m_GridObject = GameObject.Find("GRID_Square");
        var m_Camera = GameObject.Find("Main Camera");

        
         if(m_PlayStatus == false)
         m_playback.Stop();
         
         else
         m_playback.Start();
         CameraMovement(m_Camera);


       
    }

    private MidiFile ReadFile(string Path)
    {
        var File = MidiFile.Read(Path, new ReadingSettings
        {
            InvalidChannelEventParameterValuePolicy = InvalidChannelEventParameterValuePolicy.ReadValid,
            InvalidChunkSizePolicy = InvalidChunkSizePolicy.Ignore,
            InvalidMetaEventParameterValuePolicy = InvalidMetaEventParameterValuePolicy.SnapToLimits,
            MissedEndOfTrackPolicy = MissedEndOfTrackPolicy.Ignore,
            NoHeaderChunkPolicy = NoHeaderChunkPolicy.Ignore,
            NotEnoughBytesPolicy = NotEnoughBytesPolicy.Ignore,
            UnexpectedTrackChunksCountPolicy = UnexpectedTrackChunksCountPolicy.Ignore,
            UnknownChannelEventPolicy = UnknownChannelEventPolicy.SkipStatusByteAndOneDataByte,
            UnknownChunkIdPolicy = UnknownChunkIdPolicy.ReadAsUnknownChunk,
            UnknownFileFormatPolicy = UnknownFileFormatPolicy.Ignore,
        });
        return File;
    }

    private IEnumerator PlayMidi(MidiFile File, OutputDevice[] OutPut, TimeSpan Duration, bool PlayStatus, GameObject Kamera)
    {
        //System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();
        Debug.Log("Playback Function started, Status is " + PlayStatus);
           
            m_playback = File.GetPlayback(m_OutputDevices[0]);
            m_playback.InterruptNotesOnStop = true;
            m_playback.Start(); 
            m_playback.Loop = false;
            Debug.Log("Playback started");
            m_playback.NotesPlaybackStarted += OnNotesPlaybackStarted;
            PlaybackCurrentTimeWatcher.Instance.AddPlayback(m_playback, TimeSpanType.Midi);
            PlaybackCurrentTimeWatcher.Instance.CurrentTimeChanged += OnCurrentTimeChanged;
            PlaybackCurrentTimeWatcher.Instance.Start();

            while (m_playback.IsRunning)     
            {
                CameraMovement(Kamera);
                yield return null;           
            }

           
       
    }
    
    private static void OnNotesPlaybackStarted(object sender, NotesEventArgs e)
        {
            /*if (e.Notes.Any(n => n.Length == Melanchall.DryWetMidi.MusicTheory.Interval.Eight))
                m_playback.Stop();*/
        }

    private TimeSpan GetDuration(MidiFile File)
    {
        TimeSpan midiFileDuration = File.GetDuration<MetricTimeSpan>();
        //Debug.Log("Duration " + midiFileDuration);
        return midiFileDuration;
    }
    

    private void DisplayNotes(MidiFile File, TimeSpan Duration, GameObject PrefabNotes)
    {
        TempoMap tempo = File.GetTempoMap();
        IEnumerable<Note> notes = File.GetNotes(); 
        var NoteWidth = 5f;

        var notePos = new Vector3(1,1,1);

         foreach (var note in notes)
            {
                float noteTime = note.TimeAs<MetricTimeSpan>(tempo).TotalMicroseconds / 100000.0f;
                int noteNumber = note.NoteNumber;
                float noteLength = note.LengthAs<MetricTimeSpan>(tempo).TotalMicroseconds / 100000f * NoteWidth;
                float noteChannel = note.Channel;
               
               /*Debug.Log("NoteTime " + noteTime);
                Debug.Log("NoteNumber " + noteNumber);
                Debug.Log("Note Length " + noteLength);
                Debug.Log("Note Channel " + noteChannel);*/

                GameObject noteObject = Instantiate(PrefabNotes, notePos, Quaternion.identity);
                noteObject.GetComponent<GameNote>().InitGameNote(noteTime, noteNumber,noteLength,noteChannel);
                noteObject.SetActive(true);
            }

          
    }

    private void WriteNotes(InputDevice[] InputPiano, OutputDevice[] Output)
    {
        InputPiano[0].EventReceived += OnEventReceived;
        InputPiano[0].StartEventsListening();
        Debug.Log("Input Piano working");
        
    }

     private void OnEventReceived(object sender, MidiEventReceivedEventArgs e)
    {
        var midiDevice = (MidiDevice)sender;
        Debug.Log("Event received from " + midiDevice.Name + ": " + e.Event); 
    }
  

    private InputDevice[] GetInputDevices()
    {

        InputDevice[] inputList = InputDevice.GetAll().ToArray();
        for (int i = 0; i <= inputList.Length-1; i++)
        {
            Debug.Log("Input " + inputList[i] + " Nummer im Array " + i);
        }

        return inputList;
      
    }

    private OutputDevice[] GetOutputDevices()
    {

        OutputDevice[] outputList = OutputDevice.GetAll().ToArray();
        for (int i = 0; i <= outputList.Length-1; i++)
        {
            Debug.Log("Output " + outputList[i] + " Nummer im Array " + i);
        }

        return outputList;
      
    }

    private void setDevices(InputDevice[] Input, OutputDevice[] Output)
    {
        m_DeviceConnector = new DevicesConnector(Input[0], Output[0], Output[1]);
        m_DeviceConnector.Connect();
    }

    private static void OnCurrentTimeChanged(object sender, PlaybackCurrentTimeChangedEventArgs e)
    {
        foreach (var playbackTime in e.Times)
        {
            var playback = playbackTime.Playback;
            var time = (MidiTimeSpan)playbackTime.Time;

            Console.WriteLine($"Current time is {time}.");
        }
    }

 

    private void OnApplicationQuit() 
        {
            m_playback.Stop();
            m_playback.Dispose();
            PlaybackCurrentTimeWatcher.Instance.Dispose();
            m_OutputDevices[0].Dispose();
            m_OutputDevices[1].Dispose();
            m_InputDevices[0].Dispose();
            
        }

    public void OnPianoNotes(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Debug.Log("Jump in ONPIANONOTES");
        controls.Player.PianoNotes.performed += _ => 
        {
            m_PlayStatus = false;
            m_playback.Stop();
            Debug.Log("PIANO ON");
        };

        controls.Player.PianoNotes.canceled += _ =>
        {
            m_PlayStatus = true;
            m_playback.Start();
            Debug.Log("PIANO SCRIPT OFF");
        };

    }

    public void CameraMovement(GameObject Kamera)
    {
                var currentTime = m_playback.GetCurrentTime<MetricTimeSpan>().TotalMicroseconds / 100000.0f;
                var cameraheight = Camera.main.orthographicSize * 2.0f;
                var camerawidth = cameraheight * Screen.width / Screen.height;
                Kamera.transform.position = new Vector3(Kamera.transform.position.x,currentTime - camerawidth / 200f, Kamera.transform.position.z);
            
                m_playback.TickClock();
    }
}
