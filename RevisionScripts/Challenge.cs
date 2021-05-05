using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Challenge : ScriptableObject
{
    private Character myCharacter;
    public abstract string ChallengeName();
    public abstract bool HasArmorPenalty();
    private int racialModifier = 0;
    public int ArmorPenalty() {
        if (HasArmorPenalty() == false) return 0;
        else if (ChallengeName() == "Swim") return myCharacter.ArmorChallengePenalty() * 2;
        else return myCharacter.ArmorChallengePenalty();
    }
    public abstract string KeyAbility();
    public bool RequiresEducation() {
        if (ChallengeName() == "Appraise") return false;
        else if (ChallengeName() == "Balance") return false;
        else if (ChallengeName() == "Bluff") return false;
        else if (ChallengeName() == "Climb") return false;
        else if (ChallengeName() == "Concentration") return false;
        else if (ChallengeName() == "Craft") return false;
        else if (ChallengeName() == "Decipher Script") return true;
        else if (ChallengeName() == "Diplomacy") return false;
        else if (ChallengeName() == "Disable Device") return true;
        else if (ChallengeName() == "Disguise") return false;
        else if (ChallengeName() == "Escape Artist") return false;
        else if (ChallengeName() == "Forgery") return false;
        else if (ChallengeName() == "Gather Information") return false;
        else if (ChallengeName() == "Handle Animal") return true;
        else if (ChallengeName() == "Heal") return false;
        else if (ChallengeName() == "Hide") return false;
        else if (ChallengeName() == "Intimidate") return false;
        else if (ChallengeName() == "Jump") return false;
        else if (ChallengeName().Contains("Knowledge")) return true;
        else if (ChallengeName() == "Listen") return false;
        else if (ChallengeName() == "Move Silently") return false;
        else if (ChallengeName() == "Open Lock") return true;
        else if (ChallengeName().Contains("Perform")) return false;
        else if (ChallengeName().Contains("Profession")) return true;
        else if (ChallengeName() == "Ride") return false;
        else if (ChallengeName() == "Search") return false;
        else if (ChallengeName() == "Sense Motive") return false;
        else if (ChallengeName() == "Sleight of Hand") return true;
        else if (ChallengeName().Contains("Speak Language")) return true;
        else if (ChallengeName() == "Spellcraft") return true;
        else if (ChallengeName() == "Spot") return false;
        else if (ChallengeName() == "Survival") return false;
        else if (ChallengeName() == "Swim") return false;
        else if (ChallengeName() == "Tumble") return true;
        else if (ChallengeName() == "Use Magic Device") return true;
        else if (ChallengeName() == "Use Rope") return false;
        else throw new System.Exception("Failed to match a challenge name");
    }
    private int points;
    public int SizeModifier() {
        if (ChallengeName() == "Hide")
        {
            if (myCharacter.GetSize() == "Fine") return 16;
            else if (myCharacter.GetSize() == "Diminutive") return 12;
            else if (myCharacter.GetSize() == "Tiny") return 8;
            else if (myCharacter.GetSize() == "Small") return 4;
            else if (myCharacter.GetSize() == "Medium") return 0;
            else if (myCharacter.GetSize() == "Large") return -4;
            else if (myCharacter.GetSize() == "Huge") return -8;
            else if (myCharacter.GetSize() == "Gargantuan") return -12;
            else if (myCharacter.GetSize() == "Colossal") return -16;
            else throw new System.Exception("Failed to match a size modifier");
        }
        else return 0;
    }
    public abstract bool IsJobChallenge(Job job);
    public int AbilityModifier() {
        if (KeyAbility() == "Strength") { return myCharacter.StrengthModifier(); }
        else if (KeyAbility() == "Dexterity") { return myCharacter.DexterityModifier(); }
        else if (KeyAbility() == "Constitution") { return myCharacter.ConstitutionModifier(); }
        else if (KeyAbility() == "Wisdom") { return myCharacter.WisdomModifier(); }
        else if (KeyAbility() == "Intelligence") { return myCharacter.IntelligenceModifier(); }
        else if (KeyAbility() == "Charisma") { return myCharacter.CharismaModifier(); }
        else throw new System.Exception("Failed to match an ability modifier"); 
    }
    private int miscModifier;
    public int TotalChallengeBonus() {
        return points + AbilityModifier() + miscModifier + SizeModifier() + ArmorPenalty() + racialModifier;
    }


    // GET METHODS
    public int GetChallengePointsTotal() { return points; }
    public int GetMiscModifierTotal() { return miscModifier; }
    public Character GetAttachedCharacter() { return myCharacter; }
    public int GetArmorPenaltyTotal() { return ArmorPenalty(); }
    public int GetRacialModifier() { return racialModifier; }


    // SET METHODS
    public void SetTotalPoints(int newPointsTotal) { points = newPointsTotal; }
    public void AdjustTotalPoints(int adjustAmount) { points += adjustAmount; }
    public void SetMiscModifier(int newMiscModifierTotal) { miscModifier = newMiscModifierTotal; }
    public void AdjustMiscModifier(int adjustAmount) { miscModifier += adjustAmount; }
    public void SetAttachedCharacter(Character character) { myCharacter = character; }
    public void SetRacialModiferTotal(int racialModifierTotal) { racialModifier = racialModifierTotal; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
