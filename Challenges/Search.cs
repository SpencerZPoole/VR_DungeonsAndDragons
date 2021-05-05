using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search : Challenge
{
    public override string ChallengeName()
    {
        return "Search";
    }
    public override string KeyAbility()
    {
        return "Intelligence";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Thief" || job.GetJobName() == "Archer") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
