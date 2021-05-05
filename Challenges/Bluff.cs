using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bluff : Challenge
{
    public override string ChallengeName()
    {
        return "Bluff";
    }
    public override string KeyAbility()
    {
        return "Charisma";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Thief" || job.GetJobName() == "Sorcerer") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
