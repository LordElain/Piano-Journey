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

    public void UpdateScore(float Score)
    {
        ScoreCounter.m_ScoreText = Score;
        print(Score);
    }
    
    public void OnPianoNotes(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
    
        var miniMidiDevice = Minis.MidiDevice.current;
        if(miniMidiDevice.wasUpdatedThisFrame)        
        {
            miniMidiDevice.onWillNoteOn += (note, velocity) => 
            {
                /* Debug.Log(note.shortDisplayName);
                Debug.Log(m_Trigger); */
                m_Trigger = true;
                //print("ON PIANO");
                if (m_AllowCollider == false)
                {
                    //print(m_NoteName + note.shortDisplayName);
                    if (m_NoteName == note.shortDisplayName)
                    {
                        Check();
                    }
                    else
                    {
                        m_Score -= m_MissingPoints;
                        if(m_Score < 0)
                        m_Score = 0;
                        UpdateScore(m_Score);
                        //GetComponent<SpriteRenderer>().color = Color.red;
                    }
                }
                else
                {
                    //GetComponent<SpriteRenderer>().color = Color.white;
                }
                
                
               
            };
            miniMidiDevice.onWillNoteOff += (velocity) =>
            {
                GetComponent<SpriteRenderer>().color = Color.white;
            };
        }
        
    }


    public void OnTriggerEnter(Collider other)
    {
            if(m_Trigger == true)
            {
                m_TriggerCase = other.gameObject.tag;
                m_TriggerBox = other.gameObject;
                m_Counter++;
                m_NoteName = other.transform.parent.name;
                m_AllowCollider = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                m_Score -= m_MissingPoints;
                if(m_Score < 0)
                m_Score = 0;
                UpdateScore(m_Score);
            }
            //other.transform.parent.gameObject.SetActive(false);

        
    }

    public void OnTriggerExit(Collider other)
    {
        m_Counter--;
        if (m_Counter == 0)
        m_AllowCollider = true;
        m_Trigger = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Check()
     {
        switch(m_TriggerCase)
        {
                            case "EARLY":
                            {
                                
                                m_Score += m_EarlyPoints;
                                GetComponent<SpriteRenderer>().color = Color.yellow;
                                UpdateScore(m_Score);
                                break;
                            }
                                
                            case "RIGHT":
                            {
                                
                                m_Score += m_RightPoints;
                                GetComponent<SpriteRenderer>().color = Color.green;
                                UpdateScore(m_Score);
                                break;
                            }
                                
                            case "LATE":
                            {
                                
                                m_Score += m_LatePoints;
                                GetComponent<SpriteRenderer>().color = Color.blue;
                                UpdateScore(m_Score);
                                break;
                            }
                            
                            default:
                            
                            //m_Score -= m_MissingPoints;
                            GetComponent<SpriteRenderer>().color = Color.white;
                            break;
        }

     }


  
}
