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

    public Text m_ScoreText;
    private bool m_Trigger;
    private string m_TriggerCase;

    private string m_NoteName;

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
       // m_ScoreText.text = m_Score.ToString();
    }

    
    public void OnPianoNotes(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        var miniMidiDevice = Minis.MidiDevice.current;
        if(miniMidiDevice.wasUpdatedThisFrame)        
        {
            miniMidiDevice.onWillNoteOn += (note, velocity) => 
            {
                //Debug.Log(note.shortDisplayName);

                 if (m_Trigger == true)
                {
                    if (m_NoteName == note.shortDisplayName)
                    {
                        Debug.Log("Hitbox Check");
                        Check(m_NoteName);
                    }
                    else
                    {
                        m_Score -= m_MissingPoints;
                    }
                }
               
            };
        }

    }

    public void OnTriggerEnter(Collider other)
    {
            m_Trigger = true;
            m_TriggerCase = other.gameObject.tag;
            //other.transform.parent.gameObject.SetActive(false);
            m_NoteName = other.transform.parent.name;

        
    }

    public void OnTriggerExit(Collider other)
    {
        m_Trigger = false;
    }

    public void Check(string NoteName)
     {
        Debug.Log(NoteName);
        switch(m_TriggerCase)
        {
                            case "EARLY":
                            {
                                Debug.Log("EARLY");
                                m_Score += m_EarlyPoints;

                                break;
                            }
                                
                            case "RIGHT":
                            {
                                Debug.Log("RIGHT");
                                m_Score += m_RightPoints;
                                break;
                            }
                                
                            case "LATE":
                            {
                                Debug.Log("LATE");
                                m_Score += m_LatePoints;
                                break;
                            }
                            
                            default:
                            Debug.Log("MISSING");
                            m_Score -= m_MissingPoints;
                            m_Trigger = false;
                            break;
        }
                    

     }
}
