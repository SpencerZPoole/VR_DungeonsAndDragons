using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeReligion : KnowledgeChallenge
{
    public override string AreaOfFocus() { return "Religion"; }

    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Cleric" || job.GetJobName() == "Monk" || job.GetJobName() == "Paladin" || job.GetJobName() == "Wizard") return true;
        else return false;
    }
}