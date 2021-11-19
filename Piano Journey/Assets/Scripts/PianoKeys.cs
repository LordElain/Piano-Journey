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
    public int m_MaxAllKeys; //All Keys on Piano
    public float m_KeyHeight; //Pixel Height

    public string[] m_NoteKeys;
    public string[] m_AllKeys;

    public Vector3[] m_PianoPosition;

    //Key Parameter
    public float m_KeyHeightWhite;
    public float m_KeyHeightBlack;

    public GameObject[] KeyObjects;
    private float[] HeightOffsetArray;
    private int m_Counter;
    private int m_CounterArrayCopy;
    public List<GameObject> m_KeyList;
    // Start is called before the first frame update
    void Start()
    {
        
        FillArray_Numbers();
        KeyObjects = new GameObject[m_MaxAllKeys];
        HeightOffsetArray = new float [m_MaxAllKeys];
        m_AllKeys = new string [m_MaxAllKeys];
        GenerateWhiteKeys(m_WhitePianoKeysObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        TransForm(m_Camera, KeyObjects);
    }

    public void FillList()
    {

    }

     
    public void FillArray_Numbers() 
    {
        
        int MaxOctave = 7;

        //Create WhiteKeys
        for (int i = 0; i < m_MaxNotesOctave-1; i++)
        {  
            for (int j = 0; j <= MaxOctave; j++)
            {    
                var KeyObject = m_NoteKeys[i]+j;
            }; 
           
        };
    }

    public void CopyArray (string NoteKeys)
    {
        if (m_CounterArrayCopy <= m_MaxAllKeys-1)
        {
            m_AllKeys[m_CounterArrayCopy] = NoteKeys;
            Debug.Log(m_AllKeys[m_CounterArrayCopy]);
            m_CounterArrayCopy++;
        } 
    }

    public void GenerateWhiteKeys(GameObject[] WhitePianoKeysObject)
    {
        float KeyOffset_Left = 2.6f; //White Key Left Offset
        float KeyOffset_Middle = 3f; // White Key Middle Offset
        float KeyOffset_Right = 3f; // White Key Right Offset
        float KeyOffset = 0;

        float KOB_Base = 1;
        float KeyOffsetBlackFirst = 1.1f;
        float KeyOffsetBlackSecond = 1.9f;
        float KeyOffsetBlackThird = 1.8f;
        bool BlackCheck = false;


        float KeyZPos = 0;
        float KeyZPos_White = 50;
        float KeyZPos_Black = 10;

        float KeyHeight = 0;
        var KeyPos = new Vector3(30,0,0);
    
        
        //KeyGenerating WhiteKeys
        for (int i = 0; i < m_MaxNotesOctave-1; i++)
        {
            KeyPos.x++;   
            m_KeyID += i;
            for (int j = 0; j < WhitePianoKeysObject.Length; j++)
            {
              GameObject PKeyObject = Instantiate(WhitePianoKeysObject[j], KeyPos, Quaternion.identity);
              PKeyObject.tag = "Key";
              SpriteRenderer m_SpriteRenderer = PKeyObject.GetComponent<SpriteRenderer>(); 
              KeyZPos = 0;
              KeyHeight = 0;        
                
                    switch(j)
                    {
                        case 0:
                                {
                                    KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Left);
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
                            KeyOffset = KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Left);
                            KeyZPos = KeyZPos_White;
                            break;
                        }
                    }


                    if (BlackCheck == false)
                    {
                        PKeyObject.GetComponent<PianoKeys>().InitPianoKeys(m_KeyID,m_AllKeys, KeyPos, KeyOffset,KeyHeight, KeyZPos);
                        PKeyObject.SetActive(true);
                        FillKeyArray(PKeyObject, KeyHeight);
                             
                    }
                    else
                    {
                        PKeyObject.GetComponent<PianoKeys>().InitPianoKeys(m_KeyID,m_AllKeys, KeyPos, KOB_Base,KeyHeight, KeyZPos);
                        PKeyObject.SetActive(true);
                        FillKeyArray(PKeyObject, KeyHeight);
                
                    }
                    BlackCheck = false;
                    
                    
                    
            }

               
        }
       
    }

    public void FillKeyArray(GameObject PianoKeyObject, float Offset)
    {
        if(m_Counter < m_MaxAllKeys-1)
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

    
    public void InitPianoKeys(int KeyID, string[] PianoKeys, Vector3 KeyPos, float KeyOffset, float KeyHeight, float KeyZ)
    {
        string Object_KeyID = PianoKeys[KeyID];
        transform.position = new Vector3(KeyPos.x+KeyOffset,KeyHeight, KeyZ);
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
