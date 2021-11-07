using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKeys : MonoBehaviour
{


    public int m_KeyID;
    public GameObject m_PianoKeys;

    public int m_MaxKeys;
    public float m_KeyHeight;

    public string[] m_Keys;
    public string[] m_AllKeys;
    // Start is called before the first frame update
    void Start()
    {
        
        FillArray_Numbers();
       // GenerateKeys(m_PianoKeys);
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
        string[] m_AllKeys = new string [100];   
        int MaxOctave = 7;
        int counter = 0;

        for (int i = 0; i < m_MaxKeys-1; i++)
        {  
            for (int j = 0; j <= 7; j++)
            {    
                for (int k = 0; k < 50 ; k++)
                {
                    m_AllKeys[k] = m_Keys[i] + j;
                    Debug.Log("All Keys: (j) " + m_AllKeys[k]);
                    Debug.Log("i" + i);
                    Debug.Log("j " + j);
                    Debug.Log("k" + k);
                    
                };
                
            }; 
           
        };
    }

    public void GenerateKeys(GameObject PianoKeys)
    {
        var KeyPos = new Vector3(1,1,1);
        for (int i = 0; i <= m_MaxKeys; i++)
        {
            m_KeyID += i;
            GameObject PKeyObject = Instantiate(PianoKeys, KeyPos, Quaternion.identity);
            PKeyObject.GetComponent<PianoKeys>().InitPianoKeys(m_KeyID,m_AllKeys);
            PKeyObject.SetActive(true);

        }
    }

    
    public void InitPianoKeys(int KeyID, string[] PianoKeys)
    {
        transform.position = new Vector3(KeyID,m_KeyHeight);
        /*transform.position = new Vector3(noteNumber,timeOfNote);
        GetComponent<SpriteRenderer>().transform.eulerAngles = Vector3.forward * 90;
        GetComponent<SpriteRenderer>().size = new Vector2(duration, 1f);
        GetComponent<SpriteRenderer>().color = Color.HSVToRGB(instrument / 10f, 1f, 1f);*/
    }
}
