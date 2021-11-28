using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNote : MonoBehaviour
{
    public float m_ZPos;
    public Text m_Text;

    private GameObject[] Key_List;
    private GameObject[] Note_List;
    public string m_NoteName;
    // Start is called before the first frame update
    void Start()
    {
        Key_List = GameObject.FindGameObjectsWithTag("Key");
        Note_List = GameObject.FindGameObjectsWithTag("Note");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitGameNote(float timeOfNote,float noteNumber,float duration,float instrument, string NoteNameOctave, string NoteName)
    {
        switch (NoteName)
        {
            case "C":
            {
                transform.position = new Vector3(noteNumber,timeOfNote,m_ZPos);
                break;
            }
            case "D":
            {
                transform.position = new Vector3(noteNumber+1F,timeOfNote,m_ZPos);
                break;
            }
            case "E":
            {
                transform.position = new Vector3(noteNumber+2f,timeOfNote,m_ZPos);
                break;
            }
            case "F":
            {
                transform.position = new Vector3(noteNumber+3f,timeOfNote,m_ZPos);
                break;
            }
            case "G":
            {
                transform.position = new Vector3(noteNumber+4f,timeOfNote,m_ZPos);
                break;
            }
            case "A":
            {
                transform.position = new Vector3(noteNumber+5f,timeOfNote,m_ZPos);
                break;
            }
            case "B":
            {
                transform.position = new Vector3(noteNumber+6f,timeOfNote,m_ZPos);
                break;
            }
            default:
            break;
        }
       // transform.position = new Vector3(noteNumber,timeOfNote,m_ZPos);
        m_Text.text = NoteNameOctave;
        m_NoteName = NoteNameOctave;
        transform.localScale = new Vector2(3, duration/4);
        //GetComponent<SpriteRenderer>().transform.eulerAngles = Vector3.forward * 90;
        //GetComponent<SpriteRenderer>().size = new Vector3(10f, duration,30);
        GetComponent<SpriteRenderer>().color = Color.HSVToRGB(instrument / 10f, 1f, 1f);
    }

}
