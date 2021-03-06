using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProfessionChallenge : Challenge
{
    public abstract string AreaOfFocus();
    public override string KeyAbility()
    {
        return "Wisdom";
    }

    public override string ChallengeName()
    {
        return ("Profession (" + AreaOfFocus() + ")");
    }

    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Cleric" || job.GetJobName() == "Druid" || job.GetJobName() == "Monk" || job.GetJobName() == "Paladin"
            || job.GetJobName() == "Archer" || job.GetJobName() == "Thief" || job.GetJobName() == "Sorcerer" || job.GetJobName() == "Wizard") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
