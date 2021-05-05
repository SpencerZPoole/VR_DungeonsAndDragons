using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : Challenge
{
    public override string ChallengeName()
    {
        return "Climb";
    }
    public override string KeyAbility()
    {
        return "Strength";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Thief" || job.GetJobName() == "Barbarian" || job.GetJobName() == "Gladiator" || job.GetJobName() == "Monk" || job.GetJobName() == "Archer") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return true;
    }

}
