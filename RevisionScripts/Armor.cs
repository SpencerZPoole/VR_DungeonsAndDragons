using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Armor : Item
{
    public abstract int ArmorBonus();

    public abstract int MaxDexBonus();

    public abstract int ArmorPenalty();

    public abstract int ArcaneSpellFailureChance();

    public abstract int Speed30ft();
    public abstract int Speed20ft();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
