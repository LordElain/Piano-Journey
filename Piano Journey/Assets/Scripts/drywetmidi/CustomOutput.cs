using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Devices;
using System;
using Melanchall.DryWetMidi.Core;

public sealed class CustomOutput : IOutputDevice
{
    public event EventHandler<MidiEventSentEventArgs> EventSent;

    public void PrepareForEventsSending()
    {
    }

    public void SendEvent(MidiEvent midiEvent)
    {
        Console.WriteLine(midiEvent);
    }

}
