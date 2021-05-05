using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateLockBehavior : MonoBehaviour
{
    public LockScript myLock;
    public HingeJoint myLeftHinge;
    public HingeJoint myRightHinge;
    private List<HingeJoint> hinges = new List<HingeJoint>();
    public GameObject[] lockedPlayAreas;

    // Start is called before the first frame update
    void Start()
    {
        hinges.Add(myLeftHinge);
        hinges.Add(myRightHinge);
        SetHinges();
        if (myLock.locked)
        {
            foreach (GameObject area in lockedPlayAreas)
            {
                area.SetActive(false);
            }
        }
        else {
            foreach (GameObject area in lockedPlayAreas)
            {
                area.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHinges() {
        foreach (HingeJoint hinge in hinges)
        {
            if (myLock.locked)
            {
                hinge.useLimits = true;
                JointLimits limits = hinge.limits;
                limits.min = -8;
                limits.max = 8;
                hinge.limits = limits;
                hinge.useLimits = true;
                foreach (GameObject area in lockedPlayAreas)
                {
                    area.SetActive(false);
                }
            }
            else
            {
                hinge.useLimits = false;
                foreach (GameObject area in lockedPlayAreas)
                {
                    area.SetActive(true);
                }
            }
        }
    }
}
