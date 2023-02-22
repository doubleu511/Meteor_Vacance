using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCondition_HAPPY : UnlockCondition
{
    public override bool IsUnlockable()
    {
        bool condition = SecurityPlayerPrefs.GetBool("HappyEndingClear", false);
        return condition;
    }
}
