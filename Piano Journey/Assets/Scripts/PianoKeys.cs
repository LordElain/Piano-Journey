using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKeys : MonoBehaviour
{


    public int m_KeyID;
    public GameObject[] m_WhitePianoKeysObject;
    public GameObject[] m_BlackPianoKeysObject;

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
        GenerateBlackKeys(m_BlackPianoKeysObject);
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

    public void GenerateBlackKeys(GameObject[] BlackPianoKeysObject)
    {
        
         //KeyGenerating Black Keys
                var KeyPos_Black = new Vector3(-0.5f,0,0);
                float KeyOffsetBlack = 1f;
                float KeyOffsetBlack2 = 2f;
                float KOB_Base = 0;
                for (int k = 0; k < m_MaxNotesOctave-1; k++)
                {
                    KeyPos_Black.x++;   
                    m_KeyID += k;
                    for (int l = 0; l < BlackPianoKeysObject.Length; l++)
                    {
                        GameObject PKeyObject = Instantiate(BlackPianoKeysObject[l], KeyPos_Black, Quaternion.identity);
                        SpriteRenderer m_SpriteRenderer = PKeyObject.GetComponent<SpriteRenderer>();  
                        switch(l)
                        {
                            case 0:
                            {
                                KOB_Base  = KeyOffsetAddition(KOB_Base , KeyOffsetBlack);
                                break;
                            }
                            case 1:
                            {
                                KOB_Base  = KeyOffsetAddition(KOB_Base , KeyOffsetBlack);
                                break;
                            }
                            case 2:
                            {
                                KOB_Base  = KeyOffsetAddition(KOB_Base , KeyOffsetBlack2);
                                break;
                            }
                            case 3:
                            {
                                KOB_Base  = KeyOffsetAddition(KOB_Base , KeyOffsetBlack);
                                break;
                            }
                            case 4:
                            {
                                KOB_Base  = KeyOffsetAddition(KOB_Base , KeyOffsetBlack);
                                break;
                            }
                            default:
                            {
                                KOB_Base  = KeyOffsetAddition(KOB_Base , KeyOffsetBlack);
                                break;
                            }
                        }

                        Debug.Log("KeyOffset ist bei j" + l + " " + KOB_Base + " i: " + k); 
                        PKeyObject.GetComponent<PianoKeys>().InitPianoKeys(m_KeyID,m_AllKeys, KeyPos_Black, KOB_Base , m_KeyHeightBlack, 10);
                        PKeyObject.SetActive(true);
                    }
                }
                
    }

    public void GenerateWhiteKeys(GameObject[] WhitePianoKeysObject)
    {
        float KeyOffset_Left = 1f; //White Key Left Offset
        float KeyOffset_Middle = 1f; // White Key Middle Offset
        float KeyOffset_Right = 1f; // White Key Right Offset
        float KeyOffset = 0;
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
                
                    switch(j)
                    {
                        case 0:
                                {
                                   /*  KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Left);
                                    Debug.Log("Left" + KeyOffset); */
                                    break;
                                }
                        case 1:
                                {
                                    KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Middle);
                                    break;
                                }
                        case 2:
                                {
                                    KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Right);
                                    m_SpriteRenderer.flipX = true;
                                    break;
                                }
                        case 3:
                                {
                                    KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Left);
                                    break;
                                }
                        case 4:
                                {
                                    KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Middle);
                                    break;
                                }
                        case 5:
                                {
                                    KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Middle);
                                    KeyOffset += 0.2f;
                                    break;
                                }
                        case 6:
                                {
                                    KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Right);
                                    m_SpriteRenderer.flipX = true;
                                    break;
                                }
                        default: 
                        {
                            KeyOffset = KeyOffset = KeyOffsetAddition(KeyOffset, KeyOffset_Left);
                            break;
                        }
                    }
              
            
                
            
                           
                PKeyObject.GetComponent<PianoKeys>().InitPianoKeys(m_KeyID,m_AllKeys, KeyPos, KeyOffset,m_KeyHeightWhite, 50);
                PKeyObject.SetActive(true);
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
