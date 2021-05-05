using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perform : Challenge
{
    public override string ChallengeName()
    {
        return "Perform";
    }
    public override string KeyAbility()
    {
        return "Charisma";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Monk" || job.GetJobName() == "Thief") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
