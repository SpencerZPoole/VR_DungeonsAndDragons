using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diplomacy : Challenge
{
    public override string ChallengeName()
    {
        return "Diplomacy";
    }
    public override string KeyAbility()
    {
        return "Charisma";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Cleric" || job.GetJobName() == "Druid" || job.GetJobName() == "Monk" || job.GetJobName() == "Paladin" || job.GetJobName() == "Thief") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
