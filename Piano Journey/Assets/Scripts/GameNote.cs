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

    public void InitGameNote(float timeOfNote,int noteNumber,float duration,float instrument, string NoteNameOctave)
    {
        transform.position = new Vector3(noteNumber,timeOfNote,m_ZPos);
        m_Text.text = NoteNameOctave;
        //GetComponent<SpriteRenderer>().transform.eulerAngles = Vector3.forward * 90;
        GetComponent<SpriteRenderer>().size = new Vector2(duration, 3f);
        GetComponent<SpriteRenderer>().color = Color.HSVToRGB(instrument / 10f, 1f, 1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER");
    }
}
