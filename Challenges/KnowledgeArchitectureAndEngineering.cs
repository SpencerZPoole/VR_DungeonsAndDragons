using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeArchitectureAndEngineering : KnowledgeChallenge
{
    public override string AreaOfFocus() { return "Architecture and Engineering"; }

    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Wizard") return true;
        else return false;
    }
}
