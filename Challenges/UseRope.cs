using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseRope : Challenge
{
    public override string ChallengeName()
    {
        return "Use Rope";
    }
    public override string KeyAbility()
    {
        return "Dexterity";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Archer" || job.GetJobName() == "Thief") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
