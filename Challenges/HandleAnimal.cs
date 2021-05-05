using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAnimal : Challenge
{
    public override string ChallengeName()
    {
        return "Handle Animal";
    }
    public override string KeyAbility()
    {
        return "Charisma";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Barbarian" || job.GetJobName() == "Druid" || job.GetJobName() == "Gladiator" || job.GetJobName() == "Paladin" || job.GetJobName() == "Archer") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
