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
    public Animator m_Anim;
    PianoJourney controls;
    private int m_NoteNumber;
    private Minis.MidiDevice miniMidiDevice;
    private List<Animator> m_AnimatorList = new List<Animator>();
    private int m_R;
    private int m_G;
    private int m_B;
    private Color m_Whitekey;
    private Color m_Blackkey;

    // Start is called before the first frame update
    void Start()
    {
        m_Score = 0;
        m_Anim = GetComponent<Animator>();
        CreateColor();
        m_PianoKeyList = PianoList();
        miniMidiDevice = Minis.MidiDevice.current;
        controls.Enable();
    }

    private void Awake()
    {
        controls = new PianoJourney();
        controls.Player.SetCallbacks(this); 
    }

    // Update is called once per frame
    void Update()
    {
    }

    public List<GameObject> PianoList ()
    {
        var o = m_Piano.GetComponent<PianoKeys>();
        foreach(GameObject ob in o.m_KeyList)
        {
            m_AnimatorList.Add(ob.GetComponent<Animator>());
        }
        return o.m_KeyList;  
    }


    public void UpdateScore(float Score)
    {
        ScoreCounter.m_ScoreText = Score;
    }

    public void CreateColor()
    {
        m_R = PlayerPrefs.GetInt("Color_WR");
        m_G = PlayerPrefs.GetInt("Color_WG");
        m_B = PlayerPrefs.GetInt("Color_WB");
        m_Whitekey = new Color(m_R, m_G, m_B);

        m_R = PlayerPrefs.GetInt("Color_BR");
        m_G = PlayerPrefs.GetInt("Color_BG");
        m_B = PlayerPrefs.GetInt("Color_BB");
        m_Blackkey = new Color(m_R, m_G, m_B);
    }
    
    private void Device(GameObject Key, bool Status)
    {
        Key.GetComponent<Animator>().SetBool("isPressed",Status);
        
    }
    public void OnPianoNotes(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        miniMidiDevice = Minis.MidiDevice.current;

            miniMidiDevice.onWillNoteOn += (note, velocity) => 
            {
                m_Trigger = true;
                m_Pressed = true;

                m_KeyNote = note.shortDisplayName;
                m_NoteNumber = note.noteNumber - 24;
                if(m_NoteNumber <= 0)
                {
                    m_NoteNumber = 0;
                }
                m_oldID = Check(m_Trigger, m_KeyNote, m_oldID, m_NoteID, m_NoteName, m_NoteNumber); 
            };

            miniMidiDevice.onWillNoteOff += (note) =>
            {
                m_AnimatorList[m_NoteNumber].SetBool("isPressed", false);
                if(m_AnimatorList[m_NoteNumber].ToString()[1] == 'S')
                {
                    m_AnimatorList[m_NoteNumber].GetComponent<SpriteRenderer>().color = m_Blackkey;
                }
                else
                {
                   m_AnimatorList[m_NoteNumber].GetComponent<SpriteRenderer>().color = m_Whitekey; 
                }
                m_Trigger = false;
            }; 
        }
        
    


    public void OnTriggerEnter(Collider other)
    {
        
        m_NoteID = other.gameObject.GetComponentInParent<GameNote>().m_NID;
        m_NoteName = other.transform.parent.name;
        m_NotePos = other.gameObject.GetComponentInParent<GameNote>().m_NoteNumber;
        m_NotePos = m_NotePos - 24;
        if(m_NotePos <= 0)
        m_NotePos = 0;
        m_TriggerCase = other.name;
        m_TriggerOfNote = false;
        string m_StringOfNote = "No";
        
        int oID = -1;
        
        oID = Check(m_TriggerOfNote,m_StringOfNote, oID, m_NoteID, m_KeyNote, m_NotePos);
    }

    public void OnTriggerExit(Collider other)
    {
        m_TriggerOfNote = false;
        string name = other.transform.parent.name + " Piano";
        if(m_AnimatorList[m_NoteNumber].ToString()[1] == 'S')
        {
            GetComponent<SpriteRenderer>().color = m_Blackkey;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = m_Whitekey; 
        }      
    }

    public int Check(bool Trigger, string Note, int oldID, int ID, string OtherObject, int Index)
     {  
        var Key = m_PianoKeyList[Index];
        var Anim = m_AnimatorList[Index];
        

        if (Trigger == true)
        {
            
            Anim.SetBool("isPressed", true);
            
            if (oldID != ID  && Note == OtherObject)
            {
                switch(m_TriggerCase)
                {
                                case "EARLY":
                                {
                                    m_Score += m_EarlyPoints;
                                    Key.GetComponent<SpriteRenderer>().color = Color.yellow;
                                    UpdateScore(m_Score);
                                    break;
                                }
                                    
                                case "RIGHT":
                                {
                                    m_Score += m_RightPoints;
                                    Key.GetComponent<SpriteRenderer>().color = Color.green;
                                    UpdateScore(m_Score);
                                    break;
                                }
                                    
                                case "LATE":
                                {
                                    m_Score += m_LatePoints;
                                    Key.GetComponent<SpriteRenderer>().color = Color.blue;
                                    UpdateScore(m_Score);
                                    break;
                                }
                                
                                default:
                                {
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
