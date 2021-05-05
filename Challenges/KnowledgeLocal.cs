using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeLocal : KnowledgeChallenge
{
    public override string AreaOfFocus() { return "Local"; }

    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Thief" || job.GetJobName() == "Wizard") return true;
        else return false;
    }
}