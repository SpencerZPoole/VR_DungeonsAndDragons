using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KnowledgeChallenge : Challenge
{
    public abstract string AreaOfFocus();
    public override string KeyAbility()
    {
        return "Intelligence";
    }

    public override string ChallengeName()
    {
        return ("Knowledge (" + AreaOfFocus() + ")");
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
