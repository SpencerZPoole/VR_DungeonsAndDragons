using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disguise : Challenge
{
    public override string ChallengeName()
    {
        return "Disguise";
    }
    public override string KeyAbility()
    {
        return "Charisma";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Thief" || job.GetJobName() == "Bard") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
