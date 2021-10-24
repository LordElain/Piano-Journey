using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNote : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitGameNote(float timeOfNote,int noteNumber,float duration,float instrument)
    {
        transform.position = new Vector3(timeOfNote, -noteNumber);
        GetComponent<SpriteRenderer>().size = new Vector2(duration, 1f);
        GetComponent<SpriteRenderer>().color = Color.HSVToRGB(instrument / 10f, 1f, 1f);
    }
}
