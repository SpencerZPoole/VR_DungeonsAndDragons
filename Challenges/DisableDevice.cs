using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableDevice : Challenge
{
    public override string ChallengeName()
    {
        return "Disable Device";
    }
    public override string KeyAbility()
    {
        return "Intelligence";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Thief") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}

