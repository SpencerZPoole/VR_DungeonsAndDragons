﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : Challenge
{
    public override string ChallengeName()
    {
        return "Hide";
    }
    public override string KeyAbility()
    {
        return "Dexterity";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Monk" || job.GetJobName() == "Archer" || job.GetJobName() == "Thief") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return true;
    }

}
