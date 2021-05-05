using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    // misc/utility variables

    public GameObject currentLock;
    public bool playerIsLockpicking;


    // Start is called before the first frame update
    void Start()
    {
        if (GetChallengeList() == null)
        {
            ResetChallengeList();
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
