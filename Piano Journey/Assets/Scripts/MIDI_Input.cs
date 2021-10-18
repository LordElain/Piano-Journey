using System;
using System.Collections;
using System.Collections.Generic;
using Minis;
using UnityEngine;
 using UnityEngine.InputSystem;
 
 public class MIDI_Input : MonoBehaviour, PianoJourney.IPlayerActions
 {
 PianoJourney controls;
 
 
     private void Awake()
     {
        controls = new PianoJourney();
        controls.Player.SetCallbacks(this);
  
     }

    public void OnEnable()
     {
        Debug.Log("Enable");
        controls.Enable();
     }


     public void OnDisable()
     {
        Debug.Log("Disable");
        controls.Disable();
     }

    public void OnPianoNotes(InputAction.CallbackContext context)
    {
        controls.Player.PianoNotes.performed += _ => 
        {
            Debug.Log("PIANO ON");
        };

        controls.Player.PianoNotes.canceled += _ =>
        {
            Debug.Log("PIANO SCRIPT OFF");
        };

       
        
        
    }

}