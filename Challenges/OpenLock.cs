using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLock : Challenge
{
    public override string ChallengeName()
    {
        return "Open Lock";
    }
    public override string KeyAbility()
    {
        return "Dexterity";
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
