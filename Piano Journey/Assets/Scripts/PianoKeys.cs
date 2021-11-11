using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKeys : MonoBehaviour
{


    public int m_KeyID;
    public GameObject[] m_WhitePianoKeysObject;

    public int m_MaxNotesOctave; //Notes per Octave
    public int m_MaxAllKeys; //All Keys on Piano
    public float m_KeyHeight; //Pixel Height

    public string[] m_NoteKeys;
    public string[] m_AllKeys;

    //Key Parameter
    public float m_KeyHeightWhite;
    public float m_KeyHeightBlack;
    // Start is called before the first frame update
    void Start()
    {
        
        FillArray_Numbers();
        GenerateWhiteKeys(m_WhitePianoKeysObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetMaxkeys(int max)
    {

    }


    public void FillArray_Numbers()
    {
        string[] m_AllKeys = new string [m_MaxAllKeys];
        int MaxOctave = 7;

        //Create WhiteKeys
        for (int i = 0; i < m_MaxNotesOctave-1; i++)
        {  
            for (int j = 0; j <= MaxOctave; j++)
            {    
                for (int k = 0; k < 50 ; k++)
                {
                    m_AllKeys[k] = m_NoteKeys[i] + j;
              /*       Debug.Log("All Keys: (j) " + m_AllKeys[k]);
                    Debug.Log("i" + i);
                    Debug.Log("j " + j);
                    Debug.Log("k" + k); */
                    
                };
                
            }; 
           
        };
    }

    public void GenerateWhiteKeys(GameObject[] WhitePianoKeysObject)
    {
        float KeyOffset_Left = 1f; //White Key Left Offset
        float KeyOffset_Middle = 1f; // White Key Middle Offset
        float KeyOffset_Right = 1f; // White Key Right Offset
        float KeyOffset = 0;

        float KOB_Base = 1;
        float KeyOffsetBlackFirst = 0.4f;
        float KeyOffsetBlackSecond = 0.6f;
        float KeyOffsetBlackThird = 0.4f;
        bool BlackCheck = false;


        float KeyZPos = 0;
        float KeyZPos_White = 50;
        float KeyZPos_Black = 10;

        float KeyHeight = 0;
        var KeyPos = new Vector3(0,0,0);
    
        
        //KeyGenerating WhiteKeys
        for (int i = 0; i < m_MaxNotesOctave-1; i++)
        {
            KeyPos.x++;   
            m_KeyID += i;
            for (int j = 0; j < WhitePianoKeysObject.Length; j++)
            {
              GameObject PKeyObject = Instantiate(WhitePianoKeysObject[j], KeyPos, Quaternion.identity);
              SpriteRenderer m_SpriteRenderer = PKeyObject.GetComponent<SpriteRenderer>(); 
              KeyZPos = 0;
              KeyHeight = 0;        
                
                    switch(j)
                    {
                        case 0:
                                {
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
                    }
                    else
                    {
                        PKeyObject.GetComponent<PianoKeys>().InitPianoKeys(m_KeyID,m_AllKeys, KeyPos, KOB_Base,KeyHeight, KeyZPos);
                        PKeyObject.SetActive(true);
                    }
                    BlackCheck = false;
                
            }

               
        }
       
    }

    public float KeyOffsetAddition(float KeyOffset, float KeyOffsetAdd)
    {
        KeyOffset += KeyOffsetAdd;
        return KeyOffset;
    }

    
    public void InitPianoKeys(int KeyID, string[] PianoKeys, Vector3 KeyPos, float KeyOffset, float KeyHeight, float KeyZ)
    {
        int Object_KeyID = KeyID;
        transform.position = new Vector3(KeyPos.x+KeyOffset,KeyHeight, KeyZ);
        /*transform.position = new Vector3(noteNumber,timeOfNote);
        GetComponent<SpriteRenderer>().transform.eulerAngles = Vector3.forward * 90;
        GetComponent<SpriteRenderer>().size = new Vector2(duration, 1f);
        GetComponent<SpriteRenderer>().color = Color.HSVToRGB(instrument / 10f, 1f, 1f);*/
    }
}
