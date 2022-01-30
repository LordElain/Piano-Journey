using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyGameLogic : MonoBehaviour, PianoJourney.IPlayerActions
{
    public float m_Score;
    public float m_EarlyPoints;
    public float m_RightPoints;
    public float m_LatePoints;
    public float m_MissingPoints;

    public bool m_Trigger;
    private string m_TriggerCase;

    public string m_NoteName;
    public enum m_KeyStatus {EARLY, RIGHT, LATE, MISSING, START};
    public Color m_NowKeyStatus;
    public GameObject m_TriggerBox;
    public bool m_AllowCollider;
    public int m_Counter;
    public bool m_Pressed;

    public int m_NoteID;
    public int m_oldID;
    public bool m_TriggerOfNote;
    public GameObject[] KeyArray;
    public int m_NotePos;
    private bool m_Status = false;
    public List<GameObject> m_PianoKeyList = new List<GameObject>();
    public string m_KeyNote;
    public GameObject m_Piano;

    PianoJourney controls;
    // Start is called before the first frame update
    void Start()
    {
        m_Score = 0;
        controls = new PianoJourney();
        controls.Player.SetCallbacks(this);
        controls.Enable();
        
       
    }

    // Update is called once per frame
    void Update()
    {
    }

    public List<GameObject> PianoList ()
    {
        KeyArray = GameObject.FindGameObjectsWithTag("Key");
        var o = m_Piano.GetComponent<PianoKeys>();

        return o.m_KeyList;  
    }

    public void UpdateScore(float Score)
    {
        ScoreCounter.m_ScoreText = Score;
    }
    
    public void OnPianoNotes(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
    
        var miniMidiDevice = Minis.MidiDevice.current;
        if(miniMidiDevice.wasUpdatedThisFrame)        
        {
            miniMidiDevice.onWillNoteOn += (note, velocity) => 
            {
                m_Trigger = true;
                m_Pressed = true;
                
                m_KeyNote = note.shortDisplayName;
                int k = note.noteNumber;
                GameObject key = gameObject;
                int KeyID = 1;

                
                m_oldID = Check(m_Trigger, m_KeyNote, m_oldID, m_NoteID, key, KeyID, m_NoteName);

                

            /*         else 
                    {
                        m_Score -= m_MissingPoints;
                        if(m_Score < 0)
                        m_Score = 0;
                        UpdateScore(m_Score);
                        
                    } */

               
            };
            miniMidiDevice.onWillNoteOff += (velocity) =>
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                m_Trigger = false;
            };
        }
        
    }


    public void OnTriggerEnter(Collider other)
    {
        
        m_NoteID = other.gameObject.GetComponentInParent<GameNote>().m_NID;
        m_NoteName = other.transform.parent.name;
        m_NotePos = other.gameObject.GetComponentInParent<GameNote>().m_NoteNumber;
        m_TriggerCase = other.name;
        m_TriggerOfNote = false;
        string m_StringOfNote = "No";
        GameObject key = gameObject;

        int oID = -1;
        int NoteObjectID = 0;
        oID = Check(m_TriggerOfNote,m_StringOfNote, oID, m_NoteID, key, NoteObjectID, m_KeyNote);



        
    }

    public void OnTriggerExit(Collider other)
    {
        m_TriggerOfNote = false;
        GetComponent<SpriteRenderer>().color = Color.white;
        
    }

    public int Check(bool Trigger, string Note, int oldID, int ID, GameObject Key, int ObjectID, string OtherObject)
     {  
        if(m_Status == false)
        {
            m_PianoKeyList = PianoList();
            m_Status = true;
        }
        
        
        if (Trigger == true)
        {
            var k = m_PianoKeyList.Find(obj => obj.name == Note + " Piano");
           
            if (oldID != ID && ObjectID == 1 && Note == OtherObject)
            {

                
                switch(m_TriggerCase)
                {
                                case "EARLY":
                                {
                                    m_Score += m_EarlyPoints;
                                    k.GetComponent<SpriteRenderer>().color = Color.yellow;
                                    UpdateScore(m_Score);
                                    break;
                                }
                                    
                                case "RIGHT":
                                {
                                    m_Score += m_RightPoints;
                                    k.GetComponent<SpriteRenderer>().color = Color.green;
                                    UpdateScore(m_Score);
                                    break;
                                }
                                    
                                case "LATE":
                                {
                                    m_Score += m_LatePoints;
                                    k.GetComponent<SpriteRenderer>().color = Color.blue;
                                    UpdateScore(m_Score);
                                    break;
                                }
                                
                                default:
                                {
                                    /* m_Score += m_LatePoints;
                                    Key.GetComponent<SpriteRenderer>().color = Color.red;
                                    UpdateScore(m_Score); */
                                    break;
                                }
                                
                }
                
            }
            else
            {
                m_Score -= m_MissingPoints;
                if(m_Score < 0)
                m_Score = 0;
                UpdateScore(m_Score);
                //k.GetComponent<SpriteRenderer>().color = Color.red;
            }
            oldID = ID;
        }
        else
        {
            m_Score -= m_MissingPoints;
            if(m_Score < 0)
            m_Score = 0;
            UpdateScore(m_Score);
            Key.GetComponent<SpriteRenderer>().color = Color.red;
        }
       

        return oldID;
     }


  
}
