using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GusTheFighter : PlayerCharacter
{
    
    // Start is called before the first frame update
    void Start()
    {
        
        SetUniqueName("Gus The Fighter");
        SetStrengthScore(18);
        SetDexterityScore(16);
        SetConstitutionScore(16);
        SetWisdomScore(13);
        SetIntelligenceScore(10);
        SetCharismaScore(13);
        SetMaxHealth(10 + ConstitutionModifier());
        SetCurrentHealth(GetMaxHealth());
        ResetChallengeList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
