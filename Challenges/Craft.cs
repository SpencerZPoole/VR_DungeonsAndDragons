using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : Challenge
{
    public override string ChallengeName()
    {
        return "Craft";
    }
    public override string KeyAbility()
    {
        return "Intelligence";
    }
    public override bool IsJobChallenge(Job job)
    {
        return true;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
