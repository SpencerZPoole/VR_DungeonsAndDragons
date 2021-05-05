using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeThePlanes : KnowledgeChallenge
{
    public override string AreaOfFocus() { return "The Planes"; }

    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Cleric" || job.GetJobName() == "Wizard") return true;
        else return false;
    }
}