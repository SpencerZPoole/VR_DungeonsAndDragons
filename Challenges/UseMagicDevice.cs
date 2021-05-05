using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseMagicDevice : Challenge
{
    public override string ChallengeName()
    {
        return "Use Magic Device";
    }
    public override string KeyAbility()
    {
        return "Charisma";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Thief") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
