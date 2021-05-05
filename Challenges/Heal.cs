using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Challenge
{
    public override string ChallengeName()
    {
        return "Heal";
    }
    public override string KeyAbility()
    {
        return "Wisdom";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Cleric" || job.GetJobName() == "Druid" || job.GetJobName() == "Paladin" || job.GetJobName() == "Archer") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
