using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpeakLanguageChallenge : Challenge
{
    public abstract string Language();
    public override string KeyAbility()
    {
        return null;
    }

    public override string ChallengeName()
    {
        return ("Speak Language (" + Language() + ")");
    }

    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard") return true;
        else return false;
    }

    public override bool HasArmorPenalty()
    {
        return false;
    }

}
