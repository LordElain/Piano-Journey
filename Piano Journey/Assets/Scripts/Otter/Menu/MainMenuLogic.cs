using System;
using System.Collections;
using System.Collections.Generic;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    public  EventSystem system;



    // Start is called before the first frame update
    void Start()
    {
        MainMenuButton();
        system = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            m_Confirmation = true;
        }

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
            m_IsPrimaryHand = true;
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
        setColorSettings(m_R.text, m_G.text, m_B.text, m_IsPrimaryHand);
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
            m_MainColorPrimaryKey.SetActive(false);
            m_MainColorSecondKey.SetActive(true);
            m_IsPrimaryHand = false;
        }
        else
        {
            m_MainColorPrimaryKey.SetActive(true);
            m_MainColorSecondKey.SetActive(false);
            m_IsPrimaryHand = true;
        }

    }

    public void setColorSettings(string RED, string GREEN, string BLUE, bool PrimaryHand)
    {
        if (PrimaryHand == true)
        {
            PlayerPrefs.SetInt("Color_R", int.Parse(RED));
            PlayerPrefs.SetInt("Color_G", int.Parse(GREEN));
            PlayerPrefs.SetInt("Color_B", int.Parse(BLUE));
        }
        else
        {
            PlayerPrefs.SetInt("Color_SR", int.Parse(RED));
            PlayerPrefs.SetInt("Color_SG", int.Parse(GREEN));
            PlayerPrefs.SetInt("Color_SB", int.Parse(BLUE));
        }
        
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
    }
}
