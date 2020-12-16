using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField]
    private int SceneToLoad = 1;
    [SerializeField]
    private GameObject MainMenuUI;
    [SerializeField]
    private GameObject OptionsMenuUI;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnExitPressed()
    {
        Application.Quit();
    }

    public void OnStartGamePressed()
    {
        SceneManager.LoadScene(SceneToLoad);
    }

    public void OnOptionsPressed()
    {
        MainMenuUI.SetActive(false);
        OptionsMenuUI.SetActive(true);
    }

    public void OnBackPressed()
    {
        MainMenuUI.SetActive(true);
        OptionsMenuUI.SetActive(false);
    }
}
