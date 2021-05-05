using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseMotive : Challenge
{
    public override string ChallengeName()
    {
        return "Sense Motive";
    }
    public override string KeyAbility()
    {
        return "Wisdom";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Paladin" || job.GetJobName() == "Monk" || job.GetJobName() == "Thief") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
