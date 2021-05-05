using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job : MonoBehaviour
{
    public abstract string JobName();
    private int jobLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // SET METHODS
    public void SetJobLevel(int newTotalJobLevel) { jobLevel = newTotalJobLevel; }
    public void AdjustJobLevel(int jobLevelAdjustAmount) { jobLevel += jobLevelAdjustAmount; }

    // GET METHODS
    public string GetJobName() { return JobName(); }
    public int GetJobLevel() { return jobLevel; }
}
