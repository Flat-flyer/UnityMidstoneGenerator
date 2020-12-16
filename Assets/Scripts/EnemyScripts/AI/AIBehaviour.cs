using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBehaviour : MonoBehaviour
{
    public float WeightMultipler = 1;
    public float TimePassed = 0;
    public bool CurrentlyAttacking = false;
    public abstract float GetWeight();
    public abstract void Execute();
}
