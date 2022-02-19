using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultValues : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int Check = PlayerPrefs.GetInt("FirstStart",0);
        if(Check == 0)
        {
            PlayerPrefs.SetInt("Color_R", int.Parse("100"));
            PlayerPrefs.SetInt("Color_G", int.Parse("0"));
            PlayerPrefs.SetInt("Color_B", int.Parse("0"));

            PlayerPrefs.SetInt("Color_SR", int.Parse("100"));
            PlayerPrefs.SetInt("Color_SG", int.Parse("100"));
            PlayerPrefs.SetInt("Color_SB", int.Parse("0"));

            PlayerPrefs.SetInt("Color_WR", int.Parse("100"));
            PlayerPrefs.SetInt("Color_WG", int.Parse("100"));
            PlayerPrefs.SetInt("Color_WB", int.Parse("100"));

            PlayerPrefs.SetInt("Color_BR", int.Parse("100"));
            PlayerPrefs.SetInt("Color_BG", int.Parse("100"));
            PlayerPrefs.SetInt("Color_BB", int.Parse("100"));

            PlayerPrefs.SetInt("maxKey", int.Parse("88"));
            PlayerPrefs.SetInt("FirstStart", 1);
        }
                           

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
