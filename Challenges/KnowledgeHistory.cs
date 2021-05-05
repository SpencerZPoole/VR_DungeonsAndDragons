using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeHistory : KnowledgeChallenge
{
    public override string AreaOfFocus() { return "History"; }

    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Cleric" || job.GetJobName() == "Wizard") return true;
        else return false;
    }
}
