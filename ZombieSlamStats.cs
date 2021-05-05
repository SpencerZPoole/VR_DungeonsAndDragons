using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZombieSlamStats : Weapon
{
    public override int DamageSmall()
    {
        return Random.Range(1, 7);
    }
    public override int DamageMedium()
    {
        return Random.Range(1, 7);
    }

    public override string WeaponClass() { return "natural"; }
    public override bool RangedWeapon() { return false; }
    public override bool ThrownWeapon() { return false; }
    public override bool UnarmedAttack() { return false; }
    public override bool LightMeleeWeapon() { return false; }
    public override bool OneHandedMeleeWeapon() { return false; }
    public override bool TwoHandedMeleeWeapon() { return false; }
    public override bool ReachWeapon() { return false; }
    public override int RangeIncrement() { return 0; }
    public override int Cost() { return 0; }
    public override int CriticalRangeMin() { return 20; }
    public override int CriticalRangeMultiplier() { return 2; }
    public override int Weight() { return 0; }
    public override string WeaponType() { return "slashing"; }
    public override string ItemName() { return "Zombie Slam"; }
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
