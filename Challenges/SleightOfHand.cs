using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleightOfHand : Challenge
{
    public override string ChallengeName()
    {
        return "Sleight of Hand";
    }
    public override string KeyAbility()
    {
        return "Dexterity";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Thief") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return true;
    }

}
