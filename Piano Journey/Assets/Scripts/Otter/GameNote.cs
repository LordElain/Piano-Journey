using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNote : MonoBehaviour
{
    public Text m_Text;

    public string m_NoteName;
    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitGameNote(float timeOfNote,float noteNumber,float duration,float instrument, string NoteNameOctave, string NoteName, float ZPos)
    {
        int RED = 0;
        int GREEN = 0;
        int BLUE = 0;
        if(instrument == 0)
        {
            RED = PlayerPrefs.GetInt("Color_R");
            GREEN = PlayerPrefs.GetInt("Color_G");
            BLUE = PlayerPrefs.GetInt("Color_B");
            print ("primary Note");
        }
        else
        {
            RED = PlayerPrefs.GetInt("Color_SR");
            GREEN = PlayerPrefs.GetInt("Color_SG");
            BLUE = PlayerPrefs.GetInt("Color_SB");
            Debug.Log(RED + " " + GREEN + " " + BLUE);
        }
        
        m_Text.text = NoteNameOctave;
        m_NoteName = NoteNameOctave;
        transform.localScale = new Vector2(1.5f, duration/5f);
        transform.position = new Vector3(noteNumber,timeOfNote,ZPos);
        //GetComponent<SpriteRenderer>().transform.eulerAngles = Vector3.forward * 90;
        //GetComponent<SpriteRenderer>().size = new Vector3(10f, duration,30);
        //GetComponent<SpriteRenderer>().color = Color.HSVToRGB(instrument / 10f, 1f, 1f);
        GetComponent<SpriteRenderer>().color =  new Color (RED,GREEN,BLUE,1);
    }

}
