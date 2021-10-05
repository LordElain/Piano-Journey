using System.Collections;
using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.InputSystem;
 
 public class InputTest : MonoBehaviour, PianoJourney.IPlayerActions
 {
     public InputAction pianoAction;
     public InputActionMap pianoActionMap;
 
 
     public PianoJourney controls;
 
 
     private void Awake()
     {
        
     }
 
     public void OnEnable()
     {
        Debug.Log("Enable");
        pianoAction.Enable();
        pianoActionMap.Enable();
     }


     public void OnDisable()
     {
        Debug.Log("Disable");
        pianoAction.Disable();
        pianoActionMap.Disable();
     }

    public void OnPianoNotes(InputAction.CallbackContext context)
    {
        Debug.Log("PIANO");
    }

    public void OnPianoAction(InputAction.CallbackContext context)
    {
        Debug.Log("PIANO");
    }
}