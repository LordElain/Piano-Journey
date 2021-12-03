using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minis;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Devices;
using Melanchall.DryWetMidi.Standards;
using System.Threading;
using System.Linq;



public class NotesController : MonoBehaviour
{
    //MIDI Properties

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
    public GameObject m_Piano;
    public GameObject[] Key_List;



    //Notes Generating
    public GameObject m_Prefab_Notes;


    // Start is called before the first frame update
    void Start()
    {
        var m_File = ReadFile(m_Path);
        var m_Duration = GetDuration(m_File);
        

        
        m_InputDevices = GetInputDevices();
        m_OutputDevices = GetOutputDevices();
        setDevices(m_InputDevices, m_OutputDevices);

       

        StartCoroutine(DisplayNotes(m_File, m_Duration, m_Prefab_Notes));
        StartCoroutine(PlayMidi(m_File, m_OutputDevices,m_Duration, m_PlayStatus, m_Camera));
        //WriteNotes(m_InputDevices, m_OutputDevices);
        
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if(m_PlayStatus == false)
        {
            m_playback.Stop();
        }
         
        else
        {
            m_playback.Start();
            CameraMovement(m_Camera);
        }
         

       
    }

    private MidiFile ReadFile(string Path)
    {
        //Read MidiFile with ReadingSettings
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
        //Reading Midi File and playing it
           
            m_playback = File.GetPlayback(m_OutputDevices[0]);
            m_playback.InterruptNotesOnStop = true;
            m_playback.Start(); 
            m_playback.Loop = true;

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
        //Get Duration of Midi File
        TimeSpan midiFileDuration = File.GetDuration<MetricTimeSpan>();
        //Debug.Log("Duration " + midiFileDuration);
        return midiFileDuration;
    }
    

    private IEnumerator DisplayNotes(MidiFile File, TimeSpan Duration, GameObject PrefabNotes)
    {
        //Create Note Blocks
        TempoMap tempo = File.GetTempoMap();
        IEnumerable<Note> notes = File.GetNotes();
        
        
        var NoteWidth = 6f;
        float noteOffset = 3f;
        float noteOffset2 = 4f;
        float noteOffsetPos = 0;
        var notePos = new Vector3(-4f,0,0);        
        
        
        //int KeyName = m_Piano.gameObject.;
        
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
                
                if(noteName == "B" || noteName == "E")
                {
                    NotePosition.x = NotePosition.x - 2.5f;
                }
                else
                {
                    NotePosition.x = NotePosition.x - 1.5f;
                }
                GameObject noteObject = Instantiate(PrefabNotes, notePos, Quaternion.identity);
                
                //Debug.Log(noteNumber+noteOffset*noteOffset + " " + noteNameOctave);
                noteObject.GetComponent<GameNote>().InitGameNote(noteTime,NotePosition.x,noteLength,noteChannel,noteNameOctave, noteName);
                noteObject.SetActive(true);
                noteObject.name = noteNameOctave;
                noteObject.tag = "Note";
                yield return null;
            }
          
    }

    private float NoteOffsetAddition(float NoteOffset, float NoteOffsetAdd)
    {
        NoteOffset += NoteOffsetAdd;
        return NoteOffset;
    }

    private void WriteNotes(InputDevice[] InputPiano, OutputDevice[] Output)
    {
        //Write Midi Files
        InputPiano[0].EventReceived += OnEventReceived;
        InputPiano[0].StartEventsListening();
        Debug.Log("Input Piano working");
        
    }

     private void OnEventReceived(object sender, MidiEventReceivedEventArgs e)
    {
        var midiDevice = (Melanchall.DryWetMidi.Devices.MidiDevice)sender;
        Debug.Log("Event received from " + midiDevice.Name + ": " + e.Event); 
    }
  

    private InputDevice[] GetInputDevices()
    {
        //Get All InputDevices
        InputDevice[] inputList = InputDevice.GetAll().ToArray();
        for (int i = 0; i <= inputList.Length-1; i++)
        {
            Debug.Log("Input " + inputList[i] + " Nummer im Array " + i);
        }

        return inputList;
      
    }

    private OutputDevice[] GetOutputDevices()
    {
        //Get All OutputDevices
        OutputDevice[] outputList = OutputDevice.GetAll().ToArray();
        for (int i = 0; i <= outputList.Length-1; i++)
        {
            Debug.Log("Output " + outputList[i] + " Nummer im Array " + i);
        }

        return outputList;
      
    }

    private void setDevices(InputDevice[] Input, OutputDevice[] Output)
    {
        //Connect the Input and Output Devices
        //Output on Piano and InGame
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

    public void CameraMovement(GameObject Kamera)
    {
                
                var camerawidth =   Camera.main.orthographicSize*2f;
                var cameraheight =  camerawidth * Screen.width / Screen.height;

                var currentTime = m_playback.GetCurrentTime<MetricTimeSpan>().TotalMicroseconds / 100000.0f;

                  Kamera.transform.position = new Vector3(Kamera.transform.position.x,currentTime + cameraheight/8f, Kamera.transform.position.z);

                m_playback.TickClock();
    }

    private void OnCollisionEnter(Collision collision) 
    {
        ContactPoint contact = collision.contacts[0];
        Debug.Log(contact + " Counter Zähler");

    }
}
