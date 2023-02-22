using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCondition_NORMAL : UnlockCondition
{
    public override bool IsUnlockable()
    {
        bool condition = SecurityPlayerPrefs.GetBool("NormalEndingClear", false);
        return condition;
    }
}
