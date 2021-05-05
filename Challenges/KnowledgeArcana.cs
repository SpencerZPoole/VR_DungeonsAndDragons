using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeArcana : KnowledgeChallenge
{
    public override string AreaOfFocus() { return "Arcana"; }
    
    public override bool IsJobChallenge(Job job)
    {
        if (job.GetJobName() == "Cleric" || job.GetJobName() == "Bard" || job.GetJobName() == "Monk" || job.GetJobName() == "Sorcerer" || job.GetJobName() == "Wizard") return true;
        else return false;
    }
}
