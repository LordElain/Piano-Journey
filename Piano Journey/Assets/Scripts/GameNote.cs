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

    public void InitGameNote(float timeOfNote,float noteNumber,float duration,float instrument, string NoteNameOctave)
    {
        transform.position = new Vector3(noteNumber,timeOfNote,m_ZPos);
        m_Text.text = NoteNameOctave;
        m_NoteName = NoteNameOctave;
        //GetComponent<SpriteRenderer>().transform.eulerAngles = Vector3.forward * 90;
        GetComponent<SpriteRenderer>().size = new Vector2(duration, 12f);
        GetComponent<SpriteRenderer>().color = Color.HSVToRGB(instrument / 10f, 1f, 1f);
    }

}
