using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerStats : Weapon
{
    public override int DamageSmall()
    {
        return Random.Range(1, 4);
    }
    public override int DamageMedium()
    {
        return Random.Range(1, 5);
    }

    public override string WeaponClass() { return "simple"; }
    public override bool RangedWeapon() { return false; }
    public override bool ThrownWeapon() { return false; }
    public override bool UnarmedAttack() { return false; }
    public override bool LightMeleeWeapon() { return true; }
    public override bool OneHandedMeleeWeapon() { return false; }
    public override bool TwoHandedMeleeWeapon() { return false; }
    public override bool ReachWeapon() { return false; }
    public override int RangeIncrement() { return 10; }
    public override int Cost() { return 2; }
    public override int CriticalRangeMin() { return 19; }
    public override int CriticalRangeMultiplier() { return 2; }
    public override int Weight() { return 1; }
    public override string WeaponType() { return "piercing/slashing"; }
    public override string ItemName() { return "Dagger"; }
    public override string Size() { return "medium"; }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    //awake is called before the game starts, when the script instance is loaded in the editor
    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
