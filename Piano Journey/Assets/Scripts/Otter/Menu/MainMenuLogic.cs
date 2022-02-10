using System;
using System.Collections;
using System.Collections.Generic;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Melanchall.DryWetMidi.Devices;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Linq;

public class MainMenuLogic : MonoBehaviour
{
    //MainMenu
    public GameObject m_MainMenu;
    public GameObject m_MainMenu_Option;
    public GameObject m_MainMenu_Game;
    public GameObject m_MainGame;
    public GameObject m_MainEditor;
    public GameObject m_MainOption;
    public GameObject m_MainExit;

    public bool m_PressedState;
    public bool m_Confirmation;

//Twitch Overlay
    public GameObject m_MainOptionTwitch;
    public InputField m_TwitchInput_User;
    public InputField m_TwitchInput_Channel;
    public InputField m_TwitchInput_Token;

//Piano Overlay
    public InputField m_PianoInput;
    public GameObject m_MainOptionPiano;
    public GameObject m_TwitchScript;

//Skin Overlay
    public GameObject m_MainOptionColor;
    public InputField m_R;
    public InputField m_G;
    public InputField m_B;
    public bool m_IsPrimaryHand;
    public GameObject m_MainColorPrimaryKey;
    public GameObject m_MainColorSecondKey;
    public GameObject m_MainColorInputBoard;
    public GameObject m_MainColorSubmit;
    public GameObject m_MainColorLook;
    public GameObject m_MainOptionBackground;
    public GameObject m_BackgroundDay;
    public GameObject m_BackgroundNight;
    public Boolean m_BackgroundCheck;
    public int m_ColorID;
    public GameObject m_MainColorWhiteKey;
    public GameObject m_MainColorBlackKey;

//Option
    public Text m_DeviceList;
    public  EventSystem system;





    // Start is called before the first frame update
    void Start()
    {
        MainMenuButton();
        system = EventSystem.current;
        int Check = PlayerPrefs.GetInt("Background", -1);
        if(Check != -1)
        { 
            if(Check == 0)
            {
                m_BackgroundCheck = false;
            }
            else
            {
                m_BackgroundCheck = true;
            }
        }
        else
        {
            m_BackgroundCheck = false;
        }
        MainOptionBackgroundChange();
        CreateList();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
        
            if (next!= null) 
            {
                            
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield !=null) inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret
                            
                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
        }
    }
    
    public void CreateList()
    {
        OutputDevice[] outputList = OutputDevice.GetAll().ToArray();
        for (int i = 0; i <= outputList.Length-1; i++)
        {
            m_DeviceList.text += outputList[i].ToString() + "\n";
        }
    }
    public void MainMenuButton()
    {
        m_MainMenu.SetActive(true);
        m_MainMenu_Game.SetActive(false);
        m_MainMenu_Option.SetActive(false);
        m_PressedState = false;
    }

    public void MainGameButton()
    {
        m_MainMenu_Game.SetActive(true);
        m_MainMenu.SetActive(false);
    }

    public void MainLoadFile(GameObject Scene)
    {
        string SceneName = Scene.name;
        Debug.Log("FILE LOADED");
        StartCoroutine( ShowLoadDialogCoroutine(SceneName) );
        
    }
    IEnumerator ShowLoadDialogCoroutine(String SceneName)
    {
        yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load" );
       
		if( FileBrowser.Success )
		{
            DataManager.m_Path = FileBrowser.Result[0];	
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
        }
    }

    public void MainOptionBackgroundChange()
    {
        if (m_BackgroundCheck == false)
        {
            m_BackgroundDay.SetActive(true);
            m_BackgroundNight.SetActive(false);
            m_BackgroundCheck = true;
            PlayerPrefs.SetInt("Background", 0);
            PlayerPrefs.Save();
        }
        else
        {
            m_BackgroundDay.SetActive(false);
            m_BackgroundNight.SetActive(true);
            m_BackgroundCheck = false;
            PlayerPrefs.SetInt("Background", 1);
            PlayerPrefs.Save();
        }

    }
    public void MainEditorButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("EDITOR");
    }

    public void MainOptionButton()
    {
        m_MainMenu_Option.SetActive(true);
        m_MainMenu.SetActive(false);
    }

    public void MainOptionTwitch()
    {
        if(m_PressedState == false)
        {
            m_MainOptionTwitch.SetActive(true);  
        }
        else
        {
            m_MainOptionTwitch.SetActive(false);
        }
        
    }

    public void MainOptionColor()
    {
        if (m_PressedState == false)
        {
            m_MainOptionColor.SetActive(true);
            m_ColorID = 0;
            m_MainColorInputBoard.SetActive(true);
        }
        else
        {
            m_MainOptionColor.SetActive(false);
            m_MainColorInputBoard.SetActive(false);
        }
    }

    public void MainOptionColorSubmit()
    {
        if(m_MainColorBlackKey.activeSelf == true)
        {
            m_ColorID = 3;
        }
        else if (m_MainColorPrimaryKey.activeSelf == true)
        {
            m_ColorID = 0;
        }
        setColorSettings(m_R.text, m_G.text, m_B.text, m_ColorID);
        var R = float.Parse(m_R.text);
        var G = float.Parse(m_G.text);
        var B = float.Parse(m_B.text);
        m_MainColorSubmit.GetComponent<Image>().color = new Color(R,G,B,1);
        m_MainColorLook.SetActive(true);
        
    }

    public void MainOptionChangeButton()
    {
        if(m_MainColorPrimaryKey.activeSelf == true)
        {
            print(0);
            m_MainColorPrimaryKey.SetActive(false);
            m_MainColorSecondKey.SetActive(true);
            m_MainColorWhiteKey.SetActive(false);
            m_MainColorBlackKey.SetActive(false);
            m_ColorID = 1;
            
        }

        else if(m_MainColorSecondKey.activeSelf == true)
        {
            print(1);
            m_MainColorPrimaryKey.SetActive(false);
            m_MainColorSecondKey.SetActive(false);
            m_MainColorWhiteKey.SetActive(true);
            m_MainColorBlackKey.SetActive(false);
            m_ColorID = 2;
        }

        else if(m_MainColorWhiteKey.activeSelf == true)
        {
            print(2);
            m_MainColorPrimaryKey.SetActive(false);
            m_MainColorSecondKey.SetActive(false);
            m_MainColorWhiteKey.SetActive(false);
            m_MainColorBlackKey.SetActive(true);
            m_ColorID = 3;
        }

        else if(m_MainColorBlackKey.activeSelf == true)
        {
            print(3);
            m_MainColorPrimaryKey.SetActive(true);
            m_MainColorSecondKey.SetActive(false);
            m_MainColorWhiteKey.SetActive(false);
            m_MainColorBlackKey.SetActive(false);
            m_ColorID = 0;
        } 


    }

    public void setColorSettings(string RED, string GREEN, string BLUE, int SceneID)
    {
        switch(SceneID)
        {
            case 0: 
                {   //PRIMARY KEY
                    PlayerPrefs.SetInt("Color_R", int.Parse(RED));
                    PlayerPrefs.SetInt("Color_G", int.Parse(GREEN));
                    PlayerPrefs.SetInt("Color_B", int.Parse(BLUE));
                    break;
                }
            case 1:
                {   //SECONDARY KEY
                    PlayerPrefs.SetInt("Color_SR", int.Parse(RED));
                    PlayerPrefs.SetInt("Color_SG", int.Parse(GREEN));
                    PlayerPrefs.SetInt("Color_SB", int.Parse(BLUE));
                    break;
                }
            case 2:
                {   //WHITE KEY
                    PlayerPrefs.SetInt("Color_WR", int.Parse(RED));
                    PlayerPrefs.SetInt("Color_WG", int.Parse(GREEN));
                    PlayerPrefs.SetInt("Color_WB", int.Parse(BLUE));
                    break;
                }
            case 3:
                {   //BLACK KEY
                    PlayerPrefs.SetInt("Color_BR", int.Parse(RED));
                    PlayerPrefs.SetInt("Color_BG", int.Parse(GREEN));
                    PlayerPrefs.SetInt("Color_BB", int.Parse(BLUE));
                    break;
                }
            default:
            break;
        }
        PlayerPrefs.Save();        
    }

    public void MainOptionTwitchLink()
    {
        Application.OpenURL("https://twitchapps.com/tmi/");
    }

    public void MainOptionTwitchSubmit()
    {
        setTwitchCredentials(m_TwitchInput_User.text, m_TwitchInput_Channel.text, m_TwitchInput_Token.text);
        m_TwitchScript.SetActive(true);
        Debug.Log("Submit");
    }

     public void setTwitchCredentials (string usr, string channel, string token)
    {
        PlayerPrefs.SetString("user", usr);
        PlayerPrefs.SetString("channel", usr);
        DataManager.m_Token = token;
        PlayerPrefs.Save();

    }

    public void MainCloseButton(GameObject Scene)
    {
        m_PressedState = false;
        if(m_MainColorInputBoard.activeSelf == true)
        {
            m_MainColorInputBoard.SetActive(false);
        }
    
        Scene.SetActive(false);
    }
    public void MainExitButton()
    {
        Application.Quit();
    }

    public void MainBackButton(GameObject Scene)
    {
        m_MainMenu.SetActive(true);
        Scene.SetActive(false);
        m_PressedState = false;
    }

    public void MainOptionPiano()
    {
        if (m_PressedState == false)
        {
            m_MainOptionPiano.SetActive(true);
            
        }
        else
        {
            m_MainOptionTwitch.SetActive(false);
        }
    }

    public void MainOptionPianoSubmit()
    {
        PlayerPrefs.SetInt("maxKey", int.Parse(m_PianoInput.text));
        Debug.Log("Submit");
        PlayerPrefs.Save();
    }
}
