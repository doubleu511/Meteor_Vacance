using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCondition_ALL : UnlockCondition
{
    public override bool IsUnlockable()
    {
        bool condition1 = SecurityPlayerPrefs.GetBool("NormalEndingClear", false);
        bool condition2 = SecurityPlayerPrefs.GetBool("HappyEndingClear", false);
        return condition1 && condition2;
    }
}
