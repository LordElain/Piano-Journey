using System;
using System.Collections;
using System.Collections.Generic;
using SimpleFileBrowser;
using UnityEngine;
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


    // Start is called before the first frame update
    void Start()
    {
        MainMenuButton();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            m_Confirmation = true;
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

    public void MainOptionTwitchSubmit()
    {
        setTwitchCredentials(m_TwitchInput_User.text, m_TwitchInput_Channel.text, m_TwitchInput_Token.text);
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
