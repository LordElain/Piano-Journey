using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKeys : MonoBehaviour
{


    public int m_KeyID;
    public GameObject[] m_PianoKeys;

    public int m_MaxNotesOctave; //Notes per Octave
    public int m_MaxAllKeys; //All Keys on Piano
    public float m_KeyHeight; //Pixel Height

    public string[] m_Keys;
    public string[] m_AllKeys;
    // Start is called before the first frame update
    void Start()
    {
        
        FillArray_Numbers();
        GenerateKeys(m_PianoKeys);
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
                    m_AllKeys[k] = m_Keys[i] + j;
              /*       Debug.Log("All Keys: (j) " + m_AllKeys[k]);
                    Debug.Log("i" + i);
                    Debug.Log("j " + j);
                    Debug.Log("k" + k); */
                    
                };
                
            }; 
           
        };
    }

    public void GenerateKeys(GameObject[] PianoKeys)
    {
        float KeyOffset_Left = 1f; //White Key Left
        float KeyOffset_Middle = 1f;
        float KeyOffset_Right = 1f; // White Key Right
        float KeyOffset = 0;
        var KeyPos = new Vector3(0,0,0);
    
        
        //KeyGenerating first half of white keys
        for (int i = 0; i < m_MaxNotesOctave-1; i++)
        {
            KeyPos.x++;   
            m_KeyID += i;
            for (int j = 0; j < PianoKeys.Length; j++)
            {
              GameObject PKeyObject = Instantiate(PianoKeys[j], KeyPos, Quaternion.identity);
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
              
                
            
                Debug.Log("KeyOffset ist bei j" + j + " " + KeyOffset + " i: " + i);            
                PKeyObject.GetComponent<PianoKeys>().InitPianoKeys(m_KeyID,m_AllKeys, KeyPos, KeyOffset);
                PKeyObject.SetActive(true);
            }
        }
       
    }

    public float KeyOffsetAddition(float KeyOffset, float KeyOffsetAdd)
    {
        KeyOffset += KeyOffsetAdd;
        Debug.Log(KeyOffset);
        return KeyOffset;
    }

    
    public void InitPianoKeys(int KeyID, string[] PianoKeys, Vector3 KeyPos, float KeyOffset)
    {
        int Object_KeyID = KeyID;
        transform.position = new Vector3(KeyPos.x+KeyOffset,m_KeyHeight);
        /*transform.position = new Vector3(noteNumber,timeOfNote);
        GetComponent<SpriteRenderer>().transform.eulerAngles = Vector3.forward * 90;
        GetComponent<SpriteRenderer>().size = new Vector2(duration, 1f);
        GetComponent<SpriteRenderer>().color = Color.HSVToRGB(instrument / 10f, 1f, 1f);*/
    }
}
