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



public class NotesController : MonoBehaviour
{

    public int m_BPM;
    public float m_Position;
    public float m_TimePlayed;
    private int[] m_Notes;
    public string m_Path;

    public DevicesConnector m_DeviceConnector;
    public InputDevice[] m_InputDevices;
    public OutputDevice[] m_OutputDevices;
    public static Playback m_playback;

    public GameObject m_Prefab;



    // Start is called before the first frame update
    void Start()
    {
        var m_File = ReadFile(m_Path);
        var m_Duration = GetDuration(m_File);

        DisplayNotes(m_File, m_Duration);
        m_InputDevices = GetInputDevices();
        m_OutputDevices = GetOutputDevices();
        setDevices(m_InputDevices, m_OutputDevices);
        //PlayMidi(m_File, m_OutputDevices,m_Duration);
        WriteNotes(m_InputDevices, m_OutputDevices);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void PlayMidi(MidiFile File, OutputDevice[] OutPut, TimeSpan Duration)
    {     
        m_playback = File.GetPlayback();
        m_playback.Start();
        Debug.Log("Playback started");
        m_playback.NotesPlaybackStarted += OnNotesPlaybackStarted;
        PlaybackCurrentTimeWatcher.Instance.AddPlayback(m_playback, TimeSpanType.Midi);
        PlaybackCurrentTimeWatcher.Instance.CurrentTimeChanged += OnCurrentTimeChanged;
        PlaybackCurrentTimeWatcher.Instance.Start();

        SpinWait.SpinUntil(() => !m_playback.IsRunning);


        Debug.Log("Playback stopped or finished");
        OutPut[1].Dispose();
        m_playback.Dispose();
        PlaybackCurrentTimeWatcher.Instance.Dispose();
       
    }
    
    private static void OnNotesPlaybackStarted(object sender, NotesEventArgs e)
        {
            if (e.Notes.Any(n => n.Length == Melanchall.DryWetMidi.MusicTheory.Interval.Eight))
                m_playback.Stop();
        }

    private TimeSpan GetDuration(MidiFile File)
    {
        TimeSpan midiFileDuration = File.GetDuration<MetricTimeSpan>();
        return midiFileDuration;
    }
    

    private void DisplayNotes(MidiFile File, TimeSpan Duration)
    {
        TempoMap tempoMap = File.GetTempoMap();
        IEnumerable<Note> notes = File.GetNotes(); 
        foreach(var note in notes)
        {
            Debug.Log(note);
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

}
