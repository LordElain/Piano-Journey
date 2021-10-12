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
    public Playback m_playback;



    // Start is called before the first frame update
    void Start()
    {
        var m_File = ReadFile(m_Path);
        var m_Duration = GetDuration(m_File);
       // DisplayNotes(m_File, m_Duration);
        m_InputDevices = GetInputDevices();
        m_OutputDevices = GetOutputDevices();
        PlayMidi(m_playback, m_File, m_OutputDevices);
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

    private void PlayMidi(Playback play, MidiFile File, OutputDevice[] OutPut)
    {
       
        play = File.GetPlayback(OutPut[1]);
        //play.NotesPlaybackStarted += OnNotesPlaybackStarted;
        play.Start();

        SpinWait.SpinUntil(() => !play.IsRunning);
        Debug.Log("Playback stopped or finished");
        OutPut[1].Dispose();
        play.Dispose();

       
    }

    private TimeSpan GetDuration(MidiFile File)
    {
        TimeSpan midiFileDuration = File.GetDuration<MetricTimeSpan>();
        return midiFileDuration;
    }
    

    private void DisplayNotes(MidiFile File, TimeSpan Duration)
    {

    }

    private void WriteNotes()
    {

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
