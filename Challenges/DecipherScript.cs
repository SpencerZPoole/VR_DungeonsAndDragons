using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecipherScript : Challenge
{
    public override string ChallengeName()
    {
        return "Decipher Script";
    }
    public override string KeyAbility()
    {
        return "Intelligence";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Thief" || job.GetJobName() == "Wizard") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
