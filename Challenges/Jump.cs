using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : Challenge
{
    public override string ChallengeName()
    {
        return "Jump";
    }
    public override string KeyAbility()
    {
        return "Strength";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Barbarian" || job.GetJobName() == "Bard" || job.GetJobName() == "Gladiator" || job.GetJobName() == "Archer" || job.GetJobName() == "Monk" || job.GetJobName() == "Thief") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return true;
    }

}
