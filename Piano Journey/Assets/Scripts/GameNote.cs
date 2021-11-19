using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNote : MonoBehaviour
{
    public float m_ZPos;
    public Text m_Text;

    private Vector3 m_NotePosition;
    private Vector3 m_PianoPosition;
    private int m_PianoArrayInt;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitGameNote(float timeOfNote,int noteNumber,float duration,float instrument, string NoteNameOctave)
    {
        transform.position = new Vector3(noteNumber,timeOfNote,m_ZPos);
        m_Text.text = NoteNameOctave;
        //GetComponent<SpriteRenderer>().transform.eulerAngles = Vector3.forward * 90;
        GetComponent<SpriteRenderer>().size = new Vector2(duration, 3f);
        GetComponent<SpriteRenderer>().color = Color.HSVToRGB(instrument / 10f, 1f, 1f);
    }

    public void CheckNoteKeyPosition()
    {

    }
}
