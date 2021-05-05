using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survival : Challenge
{
    public override string ChallengeName()
    {
        return "Survival";
    }
    public override string KeyAbility()
    {
        return "Wisdom";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Druid" || job.GetJobName() == "Barbarian" || job.GetJobName() == "Archer") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
