using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKeys : MonoBehaviour
{


    public string m_KeyID;
    public GameObject m_PianoKeys;

    public int m_MaxKeys;

    public string[] m_Keys;
    public string[] m_AllKeys;
    // Start is called before the first frame update
    void Start()
    {
        
        FillArray_Numbers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitPianoKeys(string KeyID)
    {
        transform.position = new Vector3();
        /*transform.position = new Vector3(noteNumber,timeOfNote);
        GetComponent<SpriteRenderer>().transform.eulerAngles = Vector3.forward * 90;
        GetComponent<SpriteRenderer>().size = new Vector2(duration, 1f);
        GetComponent<SpriteRenderer>().color = Color.HSVToRGB(instrument / 10f, 1f, 1f);*/
    }

    public void SetMaxkeys(int max)
    {

    }


    public void FillArray_Numbers()
    {
        string[] m_AllKeys = new string [89];   
        var MaxOctave = 7;
        for (int i = 0; i < m_MaxKeys; i++)
        {
            for (int j = 0; j < MaxOctave; j++)
            {
                m_AllKeys[i] = m_Keys[j] + i;
                Debug.Log("All Keys: (j) " + m_AllKeys[j]);
            };
        };
    }

    public void GenerateKeys()
    {
        for (int i = 0; i <= m_MaxKeys; i++)
        {
            m_KeyID += i;
            InitPianoKeys(m_KeyID);

        }
    }
}
