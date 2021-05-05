using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeNature : KnowledgeChallenge
{
    public override string AreaOfFocus() { return "Nature"; }

    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Druid" || job.GetJobName() == "Wizard" || job.GetJobName() == "Archer") return true;
        else return false;
    }
}