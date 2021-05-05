using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherInformation : Challenge
{
    public override string ChallengeName()
    {
        return "Gather Information";
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
