using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intimidate : Challenge
{
    public override string ChallengeName()
    {
        return "Intimidate";
    }
    public override string KeyAbility()
    {
        return "Charisma";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Barbarian" || job.GetJobName() == "Gladiator" || job.GetJobName() == "Thief") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
