using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    public GameObject m_MainMenu;
    public GameObject m_MainGame;
    public GameObject m_MainEditor;
    public GameObject m_MainOption;
    public GameObject m_MainExit;
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
    }

    public void MainGameButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GAME");
    }

    public void MainEditorButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("EDITOR");
    }

    public void MainOptionButton()
    {

    }

    public void MainExitButton()
    {
        Application.Quit();
    }
}
