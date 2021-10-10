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

    private static Playback m_Play;


    // Start is called before the first frame update
    void Start()
    {
        var m_File = ReadFile(m_Path);
        var m_Duration = GetDuration(m_File);
        DisplayNotes(m_File, m_Duration);
        PlayMidi(m_Play);
        GetDevices();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private MidiFile ReadFile(string Path)
    {
        var m_File = MidiFile.Read(Path, new ReadingSettings
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
        return m_File;
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

   private void PlayMidi(Playback play)
    {       
            
            var midiFile = MidiFile.Read("The Greatest Song Ever.mid");
            var outputDevice = OutputDevice.GetByName("Microsoft GS Wavetable Synth");
            

            play = midiFile.GetPlayback(outputDevice);
            //play.NotesPlaybackStarted += OnNotesPlaybackStarted;
            play.Start();

            SpinWait.SpinUntil(() => !play.IsRunning);

            Console.WriteLine("Playback stopped or finished.");

            outputDevice.Dispose();
            play.Dispose();
    }

    private void GetDevices()
    {
        foreach (var outputDevice in OutputDevice.GetAll())
        {
            Console.WriteLine(outputDevice.Name);
        }
    }
}
