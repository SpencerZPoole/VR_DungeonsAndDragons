using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeDungeons : KnowledgeChallenge
{
    public override string AreaOfFocus() { return "Dungeons"; }

    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Archer" || job.GetJobName() == "Wizard") return true;
        else return false;
    }
}
