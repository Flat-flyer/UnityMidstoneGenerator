using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDoNothing : AIBehaviour
{

    public override float GetWeight()
    {
        return 1;
    }

    public override void Execute()
    {

    }

    }
