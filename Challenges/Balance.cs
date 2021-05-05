using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance : Challenge
{
    public override string ChallengeName()
    {
        return "Balance";
    }
    public override string KeyAbility()
    {
        return "Dexterity";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Thief" || job.GetJobName() == "Monk") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return true;
    }
    
}
