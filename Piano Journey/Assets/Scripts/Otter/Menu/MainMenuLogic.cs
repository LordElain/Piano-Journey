using System;
using System.Collections;
using System.Collections.Generic;
using SimpleFileBrowser;
using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    public GameObject m_MainMenu;
    public GameObject m_MainMenu_Option;
    public GameObject m_MainMenu_Game;
    public GameObject m_MainGame;
    public GameObject m_MainEditor;
    public GameObject m_MainOption;
    public GameObject m_MainExit;

    public static string m_Path;
    // Start is called before the first frame update
    void Start()
    {
        MainMenuButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenuButton()
    {
        m_MainMenu.SetActive(true);
        m_MainMenu_Game.SetActive(false);
        m_MainMenu_Option.SetActive(false);
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

    public void MainExitButton()
    {
        Application.Quit();
    }

    public void MainBackButton(GameObject Scene)
    {
        m_MainMenu.SetActive(true);
        Scene.SetActive(false);
    }
}
