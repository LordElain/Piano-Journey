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
    public static bool m_Trigger;
    private string m_TriggerCase;

    public static string m_NoteName;
    public enum m_KeyStatus {EARLY, RIGHT, LATE, MISSING, START};
    public static Color m_NowKeyStatus;
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
       // m_ScoreText.text = m_Score.ToString();
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
                if (m_AllowCollider == false)
                {
                    if (m_NoteName == note.shortDisplayName)
                    {
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
            if(m_Trigger == true)
            {
                m_TriggerCase = other.gameObject.tag;
                m_TriggerBox = other.gameObject;
                m_Counter++;
                m_NoteName = other.transform.parent.name;
                m_AllowCollider = false;
            }
            //other.transform.parent.gameObject.SetActive(false);

        
    }

    public void OnTriggerExit(Collider other)
    {
        m_Counter--;
        if (m_Counter == 0)
        m_AllowCollider = true;
        m_Trigger = false;
    }

    public void Check(string NoteName)
     {
        
        switch(m_TriggerCase)
        {
                            case "EARLY":
                            {
                                m_Score += m_EarlyPoints;
                                m_NowKeyStatus = Color.yellow;
                                break;
                            }
                                
                            case "RIGHT":
                            {
                                m_Score += m_RightPoints;
                                m_NowKeyStatus = Color.green;
                                break;
                            }
                                
                            case "LATE":
                            {
                                m_Score += m_LatePoints;
                                m_NowKeyStatus = Color.blue;
                                break;
                            }
                            
                            default:
                            m_Score -= m_MissingPoints;
                            m_NowKeyStatus = Color.red;
                            m_Trigger = false;
                            break;
        }
     }


  
}
