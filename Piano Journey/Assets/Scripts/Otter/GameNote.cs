using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNote : MonoBehaviour
{
    public Text m_Text;
    public int m_NID;
    public string m_NoteName;
    public int m_NoteNumber;
    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitGameNote(float timeOfNote,float notePos,float duration,float instrument, string NoteNameOctave, string NoteName, float ZPos, int NoteID, int noteNumber)
    {
        int RED = 0;
        int GREEN = 0;
        int BLUE = 0;
        if(instrument == 0)
        {
            RED = PlayerPrefs.GetInt("Color_R");
            GREEN = PlayerPrefs.GetInt("Color_G");
            BLUE = PlayerPrefs.GetInt("Color_B");
        }
        else
        {
            RED = PlayerPrefs.GetInt("Color_SR");
            GREEN = PlayerPrefs.GetInt("Color_SG");
            BLUE = PlayerPrefs.GetInt("Color_SB");
        }
        
        m_Text.text = NoteNameOctave;
        m_NoteName = NoteNameOctave;
        transform.localScale = new Vector2(1.5f, duration / 3.75f);
        var offset = transform.localScale;
        transform.position = new Vector3(notePos,timeOfNote + (offset.y / 2),ZPos);
        m_NID = NoteID;
        m_NoteNumber = noteNumber;

        GetComponent<SpriteRenderer>().color =  new Color (RED,GREEN,BLUE,1);
    }

}
