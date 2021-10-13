using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Devices;
using System.Threading;
using System.Linq;

public class NotesController : MonoBehaviour
{

    public int m_BPM;
    public float m_Position;
    public float m_TimePlayed;
    private int[] m_Notes;
    public string m_Path;
    public InputDevice[] m_InputDevices;
    public OutputDevice[] m_OutputDevices;
    public static Playback m_playback;




    // Start is called before the first frame update
    void Start()
    {
        var m_File = ReadFile(m_Path);
        var m_Duration = GetDuration(m_File);
       // DisplayNotes(m_File, m_Duration);
        m_InputDevices = GetInputDevices();
        m_OutputDevices = GetOutputDevices();
        PlayMidi(m_File, m_OutputDevices);
        WriteNotes(m_InputDevices);
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

    private IEnumerator PlayMidi(MidiFile File, OutputDevice[] OutPut)
    {
       
        m_playback = File.GetPlayback(OutPut[1]);
        m_playback.NotesPlaybackStarted += OnNotesPlaybackStarted;
        m_playback.Start();


        SpinWait.SpinUntil(() => !m_playback.IsRunning);
        while (m_playback.IsRunning) {
            yield return null;
            m_playback.TickClock();
        }

        Debug.Log("Playback stopped or finished");
        OutPut[1].Dispose();
        m_playback.Dispose();

       
    }
    
    private static void OnNotesPlaybackStarted(object sender, NotesEventArgs e)
        {
            if (e.Notes.Any(n => n.NoteName == Melanchall.DryWetMidi.MusicTheory.NoteName.B))
                m_playback.Stop();
        }

    private TimeSpan GetDuration(MidiFile File)
    {
        TimeSpan midiFileDuration = File.GetDuration<MetricTimeSpan>();
        return midiFileDuration;
    }
    

    private void DisplayNotes(MidiFile File, TimeSpan Duration)
    {

    }

    private void WriteNotes(InputDevice[] InputPiano)
    {
        var recording = new Recording(TempoMap.Default, InputPiano[0]);
        InputPiano[0].EventReceived += OnEventReceived;
        InputPiano[0].StartEventsListening();
        Debug.Log("Input Piano working");

        Console.WriteLine("Input device is listening for events. Press any key to exit...");
        Console.ReadKey();
        
        (InputPiano[0] as IDisposable)?.Dispose();
    
        
    }

     private void OnEventReceived(object sender, MidiEventReceivedEventArgs e)
    {
        var midiDevice = (MidiDevice)sender;
        Debug.Log("Event received from " + midiDevice.Name + ": " + e.Event); // does not run
    }
  

    private InputDevice[] GetInputDevices()
    {

        InputDevice[] inputList = InputDevice.GetAll().ToArray();
        for (int i = 0; i <= inputList.Length-1; i++)
        {
            Debug.Log("Input" + inputList[i] + "Nummer im Array" + i);
        }

        return inputList;
      
    }

    private OutputDevice[] GetOutputDevices()
    {

        OutputDevice[] outputList = OutputDevice.GetAll().ToArray();
        for (int i = 0; i <= outputList.Length-1; i++)
        {
            Debug.Log("Output" + outputList[i] + "Nummer im Array" + i);
        }

        return outputList;
      
    }
}
