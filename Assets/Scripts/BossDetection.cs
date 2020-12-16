using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDetection : MonoBehaviour
{
    [SerializeField]
    private EnemyHealthManager BossHealthManager;
    [SerializeField]
    private GameObject WinText;
    [SerializeField]
    private CameraPlayMusic BossMusic;

    // Start is called before the first frame update
    void Start()
    {
        if (BossHealthManager != null)
        {
            BossMusic.StartMusic();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (BossHealthManager.HasDied == true)
        {
            WinText.SetActive(true);
            StartCoroutine(ReturnToMenu(8f));
        }
    }

    IEnumerator ReturnToMenu (float TimeDelay)
    {
        yield return new WaitForSeconds(TimeDelay);
        SceneManager.LoadScene(0);
    }
}
