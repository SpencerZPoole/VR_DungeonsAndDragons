using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : Challenge
{
    public override string ChallengeName()
    {
        return "Spot";
    }
    public override string KeyAbility()
    {
        return "Wisdom";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Druid" || job.GetJobName() == "Monk" || job.GetJobName() == "Archer" || job.GetJobName() == "Thief") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
