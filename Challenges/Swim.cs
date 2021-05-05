using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swim : Challenge
{
    public override string ChallengeName()
    {
        return "Swim";
    }
    public override string KeyAbility()
    {
        return "Strength";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Barbarian" || job.GetJobName() == "Bard" || job.GetJobName() == "Druid" || job.GetJobName() == "Gladiator" || job.GetJobName() == "Monk" || job.GetJobName() == "Archer" || job.GetJobName() == "Thief") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return true;
    }

}
