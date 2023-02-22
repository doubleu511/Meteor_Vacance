using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCondition_PLAY : UnlockCondition
{
    public override bool IsUnlockable()
    {
        bool condition = SecurityPlayerPrefs.GetBool("SeenTutorial", false);
        return condition;
    }
}
