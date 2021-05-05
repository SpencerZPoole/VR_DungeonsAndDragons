using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellcraft : Challenge
{
    public override string ChallengeName()
    {
        return "Spellcraft";
    }
    public override string KeyAbility()
    {
        return "Intelligence";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Cleric" || job.GetJobName() == "Druid" || job.GetJobName() == "Sorcerer" || job.GetJobName() == "Wizard") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
