using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeArtist : Challenge
{
    public override string ChallengeName()
    {
        return "Escape Artist";
    }
    public override string KeyAbility()
    {
        return "Dexterity";
    }
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Thief" || job.GetJobName() == "Monk" || job.GetJobName() == "Bard") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return true;
    }

}
