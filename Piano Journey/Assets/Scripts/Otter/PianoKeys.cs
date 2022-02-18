using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PianoKeys : MonoBehaviour
{


    public int m_KeyID;
    public GameObject[] m_WhitePianoKeysObject;
    public Camera m_Camera;

    public Text m_Text;
    public int m_MaxNotesOctave; //Notes per Octave
    public float m_KeyHeight; //Pixel Height
    public float m_FinalKeyPosX;
    public string Object_KeyID;
    public int m_MaxAllKeys;
    public int m_MaxAllKeys2;

    public string[] m_NoteKeys;
    public string[] m_AllKeys;
    public string m_KeyName;

    
    public Vector3[] m_PianoPosition;
    

    //Key Parameter
    public float m_KeyHeightWhite;
    public float m_KeyHeightBlack;

    public GameObject[] KeyObjects;
    public float[] m_KeyPositions;

    private float[] HeightOffsetArray;
    private int m_Counter;

    public Vector3 m_OffsetPerScene;
    public List<GameObject> m_KeyList = new List<GameObject>();
    public bool m_ListStatus;

    public Vector3 m_CameraSettings;
    public Vector3 m_Offset;

    public GameObject m_BackgroundDay;
    public GameObject m_BackgroundNight;

    // Start is called before the first frame update/* 
    void Start()
    {
        
        m_FinalKeyPosX = 0;
        m_MaxAllKeys = 88;
        m_MaxAllKeys2 = setMaxAllKeys();
        m_Offset = new Vector3(0,0,0);
        FillArray_Numbers(m_MaxAllKeys);
        m_CameraSettings = new Vector3(0,0,0);
        KeyObjects = new GameObject[m_MaxAllKeys];
        HeightOffsetArray = new float [m_MaxAllKeys];
        m_KeyPositions = new float [m_MaxAllKeys];
        //m_AllKeys = new string [DataManager.m_MaxAllKeys];
        m_CameraSettings = CameraSettings(m_CameraSettings);
        GenerateWhiteKeys(m_WhitePianoKeysObject, m_CameraSettings);
        setBackground();
        m_ListStatus = true;
    }

    // Update is called once per frame
    void Update()
    {
        TransForm(m_Camera, KeyObjects);
    }

    public void setBackground()
    {
        int Check = PlayerPrefs.GetInt("Background", -1);
        if(Check != -1)
        {
            if(Check == 0)
            {
                m_BackgroundDay.SetActive(true);
                m_BackgroundNight.SetActive(false);
            }
            else
            {
                m_BackgroundDay.SetActive(false);
                m_BackgroundNight.SetActive(true);
            }
        }
        else
        {
            m_BackgroundDay.SetActive(true);
            m_BackgroundNight.SetActive(false);
        }
    }
              
    public Vector3 CameraSettings(Vector3 StartPos)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GAME")
        {
            m_OffsetPerScene = new Vector3(75,0,0);
        }
        else
        {
            m_OffsetPerScene = new Vector3(75,0,0);
        }
        StartPos.x = m_Camera.transform.position.x - m_OffsetPerScene.x;
        var height = m_Camera.scaledPixelHeight;
        string aspect = m_Camera.aspect.ToString();
        var k = aspect.Substring(0,3);

        switch (height)
        {
            case 1080:
                m_Camera.orthographicSize = 41.76f;
                m_Offset = new Vector3(0,35,0);
            break;

            case 768:
                m_Camera.orthographicSize = 41.76f;
                m_Offset = new Vector3(0,35,0);
            break;

            case 1440:
                m_Camera.orthographicSize = 41.76f;
                m_Offset = new Vector3(0,35,0);
            break;

            case 2160:
                m_Camera.orthographicSize = 41.76f;
                m_Offset = new Vector3(0,35,0);
            break;
            default:
                m_Camera.orthographicSize = 35.06151f;
                m_Offset = new Vector3(0,28,0);
            break;
        }

        switch (k)
        {
            case "1.7":
                m_Camera.orthographicSize = 41.76f;
                m_Offset = new Vector3(0,35,0);
            break;

            case "1.5":
                m_Camera.orthographicSize = 46.44f;
                m_Offset = new Vector3(0,39,0);
            break;
            default:
                m_Camera.orthographicSize = 35.06151f;
                m_Offset = new Vector3(0,28,0);
            break;
        }

        return StartPos;
    }
    public int setMaxAllKeys()
    {
        var i = PlayerPrefs.GetInt("maxKey");
        return i;
        
    }
     
    public void FillArray_Numbers(int maxAll) 
    {
        
        int MaxOctave = 8;
        int AllKeyCounter = 0;
        m_AllKeys = new string[120];

        //Create WhiteKeys
        for (int i = 1; i <= MaxOctave; i++)
        {  
            for (int j = 0; j <= m_NoteKeys.Length-1; j++)
            {    
                
                if(AllKeyCounter < m_AllKeys.Length-1)
                {
                    m_AllKeys[AllKeyCounter] = m_NoteKeys[j]+i;
                    AllKeyCounter++;
                }
                
                
            }; 
           
        };
    }

    public void GenerateWhiteKeys(GameObject[] WhitePianoKeysObject, Vector3 StartPos)
    {
        float KeyOffset_Left = 3f; //White Key Left Offset
        float KeyOffset_Left2 = 2f; //White Key Left Offset
        float KeyOffset_Middle = 3f; // White Key Middle Offset
        float KeyOffset_Right = 3f; // White Key Right Offset
        float KeyOffset = 0;

        float KOB_Base = 1;
        float KeyOffsetBlackFirst = 1.1f;
        float KeyOffsetBlackSecond = 1.7f;
        float KeyOffsetBlackThird = 1.2f;
        bool BlackCheck = false;


        float KeyZPos = 0;
        float KeyZPos_White = -5;
        float KeyZPos_Black = -10;

        float KeyHeight = 0;

        int RED = 0;
        int GREEN = 0;
        int BLUE = 0;
        
        var KeyPos = new Vector3(StartPos.x,0,0);
        
        m_KeyID = -1;
       
        
        //KeyGenerating WhiteKeys
            for (int i = 0; i < m_MaxNotesOctave-1; i++)
            {
                KeyPos.x++;   
                if(m_KeyID < m_AllKeys.Length-1)
                {    
                    for (int j = 0; j < WhitePianoKeysObject.Length; j++)
                    {
                    GameObject PKeyObject = Instantiate(WhitePianoKeysObject[j], KeyPos, Quaternion.identity);
                    PKeyObject.SetActive(false);
                    PKeyObject.tag = "Key";
                    m_KeyList.Add(PKeyObject);
                    SpriteRenderer m_SpriteRenderer = PKeyObject.GetComponent<SpriteRenderer>();
                
                    KeyZPos = 0;
                    KeyHeight = 0;
                    m_KeyID++;
                    
                        
                        
                            switch(j)
                            {
                                case 0:
                                        {
                                            KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Left2);
                                            KeyZPos = KeyZPos_White;
                                            break;
                                        }
                                case 1:
                                        {
                                            KOB_Base = KeyOffsetAddition(KeyOffset, KeyOffsetBlackFirst);
                                            KeyZPos = KeyZPos_Black;
                                            KeyHeight = m_KeyHeightBlack;
                                            BlackCheck = true;
                                            break;
                                        }
                                case 2:
                                        {
                                            KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Middle);
                                            KeyZPos = KeyZPos_White;
                                            break;
                                        }
                                case 3:
                                        {
                                            KOB_Base = KeyOffsetAddition(KeyOffset, KeyOffsetBlackSecond);
                                            KeyZPos = KeyZPos_Black;
                                            KeyHeight = m_KeyHeightBlack;
                                            BlackCheck = true;
                                            break;
                                        }
                                case 4:
                                        {
                                            KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Right);
                                            m_SpriteRenderer.flipX = true;
                                            KeyZPos = KeyZPos_White;
                                            break;
                                        }
                                case 5:
                                        {
                                            KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Left);
                                            KeyZPos = KeyZPos_White;
                                            break;
                                        }
                                case 6:
                                        {
                                            KOB_Base = KeyOffsetAddition(KeyOffset, KeyOffsetBlackThird);
                                            KeyZPos = KeyZPos_Black;
                                            KeyHeight = m_KeyHeightBlack;
                                            BlackCheck = true;
                                            break;
                                        }
                                case 7:
                                        {
                                            KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Middle);
                                            KeyZPos = KeyZPos_White;
                                            break;
                                        }
                                case 8:
                                        {
                                            KOB_Base = KeyOffsetAddition(KeyOffset, KeyOffsetBlackSecond);
                                            KeyZPos = KeyZPos_Black;
                                            KeyHeight = m_KeyHeightBlack;
                                            BlackCheck = true;
                                            break;
                                        }
                                case 9:
                                        {
                                            KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Middle);
                                            KeyZPos = KeyZPos_White;
                                            break;
                                        }
                                case 10:
                                        {
                                            KOB_Base = KeyOffsetAddition(KeyOffset, KeyOffsetBlackSecond);
                                            KeyZPos = KeyZPos_Black;
                                            KeyHeight = m_KeyHeightBlack;
                                            BlackCheck = true;
                                            break;
                                        }
                                case 11:
                                        {
                                            KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Right);
                                            m_SpriteRenderer.flipX = true;
                                            KeyZPos = KeyZPos_White;
                                            break;
                                        }
                                default: 
                                {
                                    KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Left2);
                                    KeyZPos = KeyZPos_White;
                                    break;
                                }
                            }
                            
                            
                            if (BlackCheck == false)
                            {
                                if(m_KeyID < m_AllKeys.Length-1)
                                { 
                                    PKeyObject.GetComponent<PianoKeys>().InitPianoKeys(m_KeyID,m_AllKeys[m_KeyID], KeyPos, KeyOffset,KeyHeight, KeyZPos);
                                    PKeyObject.SetActive(true);
                                    PKeyObject.name = m_AllKeys[m_KeyID]+" Piano";
                                    RED = PlayerPrefs.GetInt("Color_WR");
                                    GREEN = PlayerPrefs.GetInt("Color_WG");
                                    BLUE = PlayerPrefs.GetInt("Color_WB");
                                    m_SpriteRenderer.color = new Color(RED,GREEN,BLUE);
                                    FillKeyArray(PKeyObject, KeyHeight);
                                    if(PKeyObject.name[0] == 'C')
                                    {
                                        var t = PKeyObject.GetComponentInChildren<Text>();
                                        t.text = m_AllKeys[m_KeyID];
                                    }
                                    else
                                    {
                                        m_Text.text = " ";
                                    }
                                }
                                    
                            }
                            else
                            {
                                if(m_KeyID < m_AllKeys.Length-1)
                                { 
                                    PKeyObject.GetComponent<PianoKeys>().InitPianoKeys(m_KeyID,m_AllKeys[m_KeyID], KeyPos, KOB_Base,KeyHeight, KeyZPos);
                                    PKeyObject.SetActive(true);
                                    PKeyObject.name = m_AllKeys[m_KeyID]+" Piano";
                                    RED = PlayerPrefs.GetInt("Color_BR");
                                    GREEN = PlayerPrefs.GetInt("Color_BG");
                                    BLUE = PlayerPrefs.GetInt("Color_BB");
                                    m_SpriteRenderer.color = new Color(RED,GREEN,BLUE);
                                    FillKeyArray(PKeyObject, KeyHeight);
                                }
                        
                            }
                            BlackCheck = false;   

                            if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "EDITOR")
                            {
                                m_MaxAllKeys2 = 88;
                            }
                            switch(m_MaxAllKeys2)
                            {
                                case 71:
                                        {

                                            if(PKeyObject.name[PKeyObject.name.Length-7] == '0' || PKeyObject.name[PKeyObject.name.Length-7] == '7')
                                            {
                                                if(PKeyObject.name[0] == 'C' && PKeyObject.name[1] != 'S')
                                                {
                                                    var t = PKeyObject.GetComponentInChildren<Text>();
                                                    t.text = " ";
                                                }
                                                PKeyObject.GetComponent<SpriteRenderer>().enabled = false;
                                                
                                            }
                                            else
                                            {
                                                PKeyObject.GetComponent<SpriteRenderer>().enabled = true;
                                            }
                                            break;
                                        }
                                case 61:
                                        {
                                            if(PKeyObject.name[PKeyObject.name.Length-7] == '0' || PKeyObject.name[PKeyObject.name.Length-7] == '1'|| PKeyObject.name[PKeyObject.name.Length-7] == '6'|| PKeyObject.name[PKeyObject.name.Length-7] == '7')
                                            {
                                                if(PKeyObject.name[0] == 'C' && PKeyObject.name[1] != 'S')
                                                {
                                                    var t = PKeyObject.GetComponentInChildren<Text>();
                                                    t.text = " ";
                                                }
                                                PKeyObject.GetComponent<SpriteRenderer>().enabled = false;                                         
                                            }
                                            else
                                            {
                                                PKeyObject.GetComponent<SpriteRenderer>().enabled = true;
                                            }
                                            break;
                                        }
                                case 25:
                                        {
                                            if(PKeyObject.name[PKeyObject.name.Length-7] == '0' || PKeyObject.name[PKeyObject.name.Length-7] == '1'|| PKeyObject.name[PKeyObject.name.Length-7] == '6'|| PKeyObject.name[PKeyObject.name.Length-7] == '7')
                                            {
                                                if(PKeyObject.name[0] == 'C' && PKeyObject.name[1] != 'S')
                                                {
                                                    var t = PKeyObject.GetComponentInChildren<Text>();
                                                    t.text = " ";
                                                }
                                                PKeyObject.GetComponent<SpriteRenderer>().enabled = false;
                                            }
                                            else
                                            {
                                                PKeyObject.GetComponent<SpriteRenderer>().enabled = true;
                                            }
                                            break;
                                        }
                                case 88:
                                        {
                                            PKeyObject.GetComponent<SpriteRenderer>().enabled = true;
                                            break;
                                        }

                                default:
                                        {
                                            PKeyObject.GetComponent<SpriteRenderer>().enabled = true;
                                            break;
                                        }
                            }
                        }
                    }
                    else
                    {
                        print("DONE");
                    }
            }               
        
    }

    public void FillKeyArray(GameObject PianoKeyObject, float Offset)
    {

        if(m_Counter < m_MaxAllKeys)
        {
            KeyObjects[m_Counter] = PianoKeyObject;
            HeightOffsetArray[m_Counter] = Offset;
            m_Counter++;
        }
    }

    public float KeyOffsetAddition(float KeyOffset, float KeyOffsetAdd)
    {
        KeyOffset += KeyOffsetAdd;
        return KeyOffset;
    }

    
    public void InitPianoKeys(int KeyID, string PianoKeys, Vector3 KeyPos, float KeyOffset, float KeyHeight, float KeyZ)
    {
        m_KeyName = PianoKeys;
        m_FinalKeyPosX = KeyPos.x+KeyOffset;
        /* m_KeyPositions = new float [m_MaxAllKeys];
        m_KeyPositions[KeyID] = m_FinalKeyPosX; */
        transform.position = new Vector3(m_FinalKeyPosX,KeyHeight, KeyZ);
    }

    public float GetPosition(int ID)
    {
        return m_KeyPositions[ID];
    }
    public void TransForm (Camera Kamera, GameObject[] PianoKeys)
    {
        
        for (int i = 0; i <= m_Counter-1; i++)
        {
            Vector3 ypos = PianoKeys[i].transform.position;
            ypos.y = Kamera.transform.position.y + HeightOffsetArray[i];
            PianoKeys[i].transform.position = ypos - m_Offset;
        }
    }
       

    
}
