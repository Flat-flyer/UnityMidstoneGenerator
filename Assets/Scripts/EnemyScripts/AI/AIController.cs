using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    public float AIRandomness = 0.3f;
    public float Frequency = 1;

    private float waited = 0;
    private List<AIBehaviour> Ais = new List<AIBehaviour>();
    private AIBehaviour CurrentAction = null;
    // Start is called before the first frame update
    void Start()
    {
        //gets each AI choice from all the ones attached using AIBehaviour and adds them to a list
        foreach (var ai in GetComponentsInChildren<AIBehaviour>())
        {
            Ais.Add(ai);
        }
    }

    // Update is called once per frame
    void Update()
    {
        waited += Time.deltaTime;
        //checks if enough time has been spent to perform another action
        if (waited < Frequency)
        {
            return;
        }
        if (CurrentAction != null)
        {
            //checks if an action is already active
            if (CurrentAction.CurrentlyAttacking == true)
            {
                return;
            }
        }
        
        string AIDebug = "";
        //sets variables for choosing the best AI option
        float BestAIValue = float.MinValue;
        AIBehaviour bestAI = null;

        //runs through each AI option in the list, gets the weight of each one and multiplies it by a randomness factor
        foreach(var ai in Ais)
        {
            ai.TimePassed += waited;
            var AIValue = ai.GetWeight() * ai.WeightMultipler + Random.Range(0, AIRandomness);
            AIDebug += ai.GetType().Name + ": " + AIValue + "\n";
            //if the current AI option has a higher weight than the last, it replaces the current as the best option
            if (AIValue > BestAIValue)
            {
                BestAIValue = AIValue;
                bestAI = ai;
            }
        }
        //Debug.Log(AIDebug);
        //executes the best AI option
        bestAI.Execute();
        CurrentAction = bestAI;
        waited = 0;


        
    }
}
