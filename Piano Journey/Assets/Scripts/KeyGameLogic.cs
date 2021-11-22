using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGameLogic : MonoBehaviour
{
    public float m_Score;
    public float m_EarlyPoints;
    public float m_RightPoints;
    public float m_LatePoints;
    public float m_MissingPoints;
    // Start is called before the first frame update
    void Start()
    {
        m_Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Note")
        {
            Debug.Log(other.gameObject.tag);
            switch(other.gameObject.tag)
            {
                case "EARLY":
                {
                    Debug.Log("EARLY");
                    m_Score += m_EarlyPoints;
                    break;
                }
                    
                case "RIGHT":
                {
                    Debug.Log("RIGHT");
                    m_Score += m_RightPoints;
                    break;
                }
                    
                case "LATE":
                {
                    Debug.Log("LATE");
                    m_Score += m_LatePoints;
                    break;
                }
                
                default:
                Debug.Log("MISSING");
                m_Score += m_MissingPoints;
                break;
            }
        }
        else
        {
            
        }
        
    }
}
