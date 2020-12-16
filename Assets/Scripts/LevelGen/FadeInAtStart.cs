using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//use this to remove the blackscreen on areas where there is no level generation needed.
public class FadeInAtStart : MonoBehaviour
{
    [SerializeField]
    private Image BlackScreen;
    // Start is called before the first frame update
    void Start()
    {
        BlackScreen.CrossFadeAlpha(0.1f, 2.0f, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
