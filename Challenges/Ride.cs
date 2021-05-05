using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ride : Challenge
{
    public override string ChallengeName()
    {
        return "Ride";
    }
    public override string KeyAbility()
    {
        return "Dexterity";
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
