using System.Net.Mime;
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
using UnityEngine.UI;

public class NotesController : MonoBehaviour
{
    //MIDI Properties

    public double m_BPM;
    public float m_Position;
    public float m_TimePlayed;
    public string m_Path;
    private bool m_ButtonPressed;

    //Playback Related
    public DevicesConnector m_DeviceConnector;
    public InputDevice[] m_InputDevices;
    public OutputDevice[] m_OutputDevices;
    public static Playback m_playback;
    public bool m_PlayStatus;
    public GameObject m_Camera;
    public GameObject m_Piano;
    public GameObject[] Key_List;
    

    public float m_ZPos;
    public bool m_BlackCheck;

    //Notes Generating
    public GameObject m_Prefab_Notes;
    


    //Menu & Overlay

    public GameObject m_PauseMenu;
    public GameObject m_EndMenu;
    public GameObject m_Score;
    public GameObject m_Loop;
    public Text m_SpeedDisplay;

    private bool m_Pressed;
    public bool m_End;


    // Start is called before the first frame update
    void Start()
    {
        m_Path = DataManager.m_Path;
        var m_File = ReadFile(m_Path);
        var m_Duration = GetDuration(m_File);
        m_BlackCheck = false;
        m_ButtonPressed = false;
        m_PlayStatus = true;
        m_End = false;
        m_EndMenu.SetActive(false);
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
            var d = SetPlaybackSpeed();
            m_SpeedDisplay.text = d.ToString();
            m_playback.Speed = d; 
            m_playback.MoveBack(new MetricTimeSpan(0, 0, 0, 10));
        }
         
        if(m_PlayStatus == true)
        {
            m_playback.Start();
            CameraMovement(m_Camera);
        }
         
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_Pressed == false && m_End == false)
            {
                
                m_PlayStatus = false;
                m_PauseMenu.SetActive(true);
                m_Pressed = true;
            }
            
            else
            {
                m_PauseMenu.SetActive(false);
                m_Pressed = false;
                m_PlayStatus = true;
            }

        }

        if (m_End == true)
        {
            m_Score.SetActive(false);
            m_EndMenu.SetActive(true);
        }

       
    }

    public double SetPlaybackSpeed()
    {
        if(m_ButtonPressed == false)
        {
           m_BPM = 1;
        }
        else
        {
           m_BPM = 0.5;
        }
    
        return m_BPM;
    }

    public void LoopSong()
    {
        print("Loop");
        if(m_playback != null)
        {
            m_playback.Stop();
            m_playback.Dispose();
            PlaybackCurrentTimeWatcher.Instance.Dispose();
            m_OutputDevices[0].Dispose();
            m_OutputDevices[1].Dispose();
            m_InputDevices[0].Dispose();
        }
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("GAME");
    }
    public void ChangeSpeedButton()
    {
        if(m_ButtonPressed == false)
        {
            m_ButtonPressed = true;
        }
        else
        {
            m_ButtonPressed = false;
        }
    }
    public void PauseResumeButton()
    {
        m_PauseMenu.SetActive(false);
        m_Pressed = false;
        m_PlayStatus = true;
    }
    public void PauseBackButton()
    {
        m_playback.Stop();
        m_playback.Dispose();
        PlaybackCurrentTimeWatcher.Instance.Dispose();
        m_OutputDevices[0].Dispose();
        m_OutputDevices[1].Dispose();
        m_InputDevices[0].Dispose();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MAIN MENU");

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
            m_playback.Loop = false;
            m_playback.Finished += PlayBackFinished; 
            PlaybackCurrentTimeWatcher.Instance.AddPlayback(m_playback, TimeSpanType.Midi);
            PlaybackCurrentTimeWatcher.Instance.CurrentTimeChanged += OnCurrentTimeChanged;
            PlaybackCurrentTimeWatcher.Instance.Start();

            while (m_playback.IsRunning)     
            {
                CameraMovement(Kamera);
                yield return null;           
            }
    }

    private void PlayBackFinished(object sender, EventArgs e)
    {
        
        //Playback play = sender as Playback;
        m_End = true;
        
    }

    public TimeSpan GetDuration(MidiFile File)
    {
        TimeSpan midiFileDuration = File.GetDuration<MetricTimeSpan>();
        return midiFileDuration;
    }

    public IEnumerator DisplayNotes(MidiFile File, TimeSpan Duration, GameObject PrefabNotes)
    {
        //Create Note Blocks
        TempoMap tempo = File.GetTempoMap();
        IEnumerable<Note> notes = File.GetNotes();
        int noteCounter = 0;

        var notePos = new Vector3(-3f,0,0);        
        
        
        //int KeyName = m_Piano.gameObject.;
        
         foreach (var note in notes)
            {
                noteCounter++;
                float noteTime = note.TimeAs<MetricTimeSpan>(tempo).TotalMicroseconds / 100000.0f;
                int noteNumber = note.NoteNumber;

                var noteName = note.NoteName.ToString();
                var noteOctave = note.Octave.ToString();
                string noteNameOctave = noteName + noteOctave;
                if(noteNameOctave[1] == 'S')
                {
                    m_BlackCheck = true;
                }
                else
                {
                    m_BlackCheck = false;
                }
                float noteLength = note.LengthAs<MetricTimeSpan>(tempo).TotalMicroseconds / 100000.0f;
                float noteChannel = note.Channel;

                var NotePosition = GameObject.Find(noteNameOctave + " Piano").transform.position;
                GameObject noteObject = Instantiate(PrefabNotes, notePos, Quaternion.identity);
                
                if(m_BlackCheck == false)
                {
                    m_ZPos = 0;
                }
                else
                {
                    m_ZPos = -9;
                }
                noteObject.GetComponent<GameNote>().InitGameNote(noteTime,NotePosition.x,noteLength,noteChannel,noteNameOctave, noteName, m_ZPos, noteCounter, noteNumber);
                noteObject.SetActive(true);
                noteObject.name = noteNameOctave;
                noteObject.tag = "Note";
                yield return null;
            }
          
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
                
                var camerawidth =   Camera.main.orthographicSize * 2f;
                var cameraheight =  camerawidth * Screen.width / Screen.height;

                var currentTime = m_playback.GetCurrentTime<MetricTimeSpan>().TotalMicroseconds / 100000.0f;

                Kamera.transform.position = new Vector3(Kamera.transform.position.x,currentTime + cameraheight/5.5f, Kamera.transform.position.z);
            
    }

    private void OnCollisionEnter(Collision collision) 
    {
        ContactPoint contact = collision.contacts[0];
        Debug.Log(contact + " Counter Z??hler");
    }
}
