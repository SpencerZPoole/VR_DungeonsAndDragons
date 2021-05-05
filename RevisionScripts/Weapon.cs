using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Weapon : Item
{
    public abstract string WeaponClass();
    public abstract bool RangedWeapon();
    public abstract bool ThrownWeapon();
    public abstract bool UnarmedAttack();
    public abstract bool LightMeleeWeapon();
    public abstract bool OneHandedMeleeWeapon();
    public abstract bool TwoHandedMeleeWeapon();
    public abstract bool ReachWeapon();
    public abstract int RangeIncrement();
    public abstract int DamageSmall();
    public abstract int DamageMedium();
    public abstract string Size();
    public abstract int CriticalRangeMin();
    public abstract int CriticalRangeMultiplier();
    public abstract string WeaponType();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
