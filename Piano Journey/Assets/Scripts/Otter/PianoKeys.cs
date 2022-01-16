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
    private GameObject Piano;

    public Text m_Text;
    public int m_MaxNotesOctave; //Notes per Octave
    public float m_KeyHeight; //Pixel Height
    public float m_FinalKeyPosX;
    public string Object_KeyID;
    public int m_MaxAllKeys;

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

    // Start is called before the first frame update
    void Start()
    {
        m_FinalKeyPosX = 0;
        m_MaxAllKeys = setMaxAllKeys();
        FillArray_Numbers(m_MaxAllKeys);
        
        KeyObjects = new GameObject[m_MaxAllKeys];
        HeightOffsetArray = new float [m_MaxAllKeys];
        m_KeyPositions = new float [m_MaxAllKeys];
        //m_AllKeys = new string [DataManager.m_MaxAllKeys];
        GenerateWhiteKeys(m_WhitePianoKeysObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        TransForm(m_Camera, KeyObjects);
    }

    public int setMaxAllKeys()
    {
        var i =PlayerPrefs.GetInt("maxKey");
        return i;
        
    }
     
    public void FillArray_Numbers(int maxAll) 
    {
        
        int MaxOctave = 8;
        int AllKeyCounter = 0;
        m_AllKeys = new string[maxAll];
        print(m_AllKeys.Length);
        //Create WhiteKeys
        for (int i = 0; i < MaxOctave; i++)
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
        print (m_AllKeys.Length);
    }

    public void GenerateWhiteKeys(GameObject[] WhitePianoKeysObject)
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
        var KeyPos = new Vector3(m_Camera.transform.position.x - 75,0,0);
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
                                FillKeyArray(PKeyObject, KeyHeight);
                                }
                                    
                            }
                            else
                            {
                                print (m_KeyID);
                                if(m_KeyID < m_AllKeys.Length-1)
                                { 
                                PKeyObject.GetComponent<PianoKeys>().InitPianoKeys(m_KeyID,m_AllKeys[m_KeyID], KeyPos, KOB_Base,KeyHeight, KeyZPos);
                                PKeyObject.SetActive(true);
                                PKeyObject.name = m_AllKeys[m_KeyID]+" Piano";
                                FillKeyArray(PKeyObject, KeyHeight);
                                }
                        
                            }
                                BlackCheck = false;   
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
        var Offfset = new Vector3(0,28,0);
        for (int i = 0; i <= m_Counter-1; i++)
        {
            Vector3 ypos = PianoKeys[i].transform.position;
            ypos.y = Kamera.transform.position.y + HeightOffsetArray[i];
            PianoKeys[i].transform.position = ypos - Offfset;
        }
    }
       

    
}
