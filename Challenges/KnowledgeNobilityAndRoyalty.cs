using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeNobilityAndRoyalty : KnowledgeChallenge
{
    public override string AreaOfFocus() { return "Nobility and Royalty"; }

    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Bard" || job.GetJobName() == "Paladin" || job.GetJobName() == "Wizard") return true;
        else return false;
    }
}