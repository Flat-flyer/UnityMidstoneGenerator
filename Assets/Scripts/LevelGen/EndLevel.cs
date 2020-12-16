using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    private bool LevelEndReached = false;
    private int CurScene;
    [SerializeField]
    private int NextScene = 0;

    // Start is called before the first frame update
    void Start()
    {
        //gets the current scene and next scene on the build index
        CurScene = SceneManager.GetActiveScene().buildIndex;
        NextScene = CurScene + 1;
    }

    // Update is called once per frame
    void Update()
    {
        //checks if player has reached the end of the level and loads the next scene
        if (LevelEndReached == true)
        {
            SceneManager.LoadScene(NextScene);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            LevelEndReached = true;
        }
    }
}
