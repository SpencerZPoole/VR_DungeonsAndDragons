using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceRollLibrary;

public abstract class Character : MonoBehaviour
{
    private int currentHealth;
    private int maxHealth;
    private Race race;
    private string uniqueName;
    private List<Job> jobList;
    private List<Item> equipmentList;
    private int copperPieces;
    private int silverPieces;
    private int goldPieces;
    private int platinumPieces;
    private Armor equippedArmor;
    private Shield equippedShield;
    private List<Challenge> challengeList;
    private int hitDieTotalAmount;
    private int hitDieSize;
    private int baseAttackBonus;
    public int ArmorClass() { 
        int totalAC = 10 + ArmorBonus() + ShieldBonus() + SizeAttackAndACModifier() + race.NaturalArmorBonus() + miscACBonus;
        int maxDexBonus = 100;
        if (equippedArmor != null) {
            maxDexBonus = equippedArmor.MaxDexBonus();
        }
        if (equippedShield != null) {
            if (equippedShield.MaxDexBonus() < maxDexBonus) {
                maxDexBonus = equippedShield.MaxDexBonus();
            }
        }
        if (DexterityModifier() > maxDexBonus)
        {
            totalAC += maxDexBonus;
        }
        else totalAC += DexterityModifier();
        return totalAC;
    }
    private int strengthScore;
    public int StrengthModifier() { return CalculateModifier(strengthScore); }
    private int dexterityScore;
    public int DexterityModifier() { return CalculateModifier(dexterityScore); }
    private int constitutionScore;
    public int ConstitutionModifier() { return CalculateModifier(constitutionScore); }
    private int intelligenceScore;
    public int IntelligenceModifier() { return CalculateModifier(intelligenceScore); }
    private int wisdomScore;
    public int WisdomModifier() { return CalculateModifier(wisdomScore); }
    private int charismaScore;
    public int CharismaModifier() { return CalculateModifier(charismaScore); }
    private int ArmorBonus() {
        if (equippedArmor != null)
        {
            return equippedArmor.ArmorBonus();
        }
        else return 0;
    }
    private int miscACBonus;
    private int miscGrappleModifier;
    private int miscAttackRollBonus;
    public int ArmorChallengePenalty()
    {
        int totalPenalty = 0;
        if (equippedArmor != null) {
            totalPenalty += equippedArmor.ArmorPenalty();
        }
        if (equippedShield != null) {
            totalPenalty += equippedShield.ArmorPenalty();
        }
        return totalPenalty;
    }
    private int ShieldBonus() {
        if (equippedShield != null)
        {
            return equippedShield.ArmorBonus();
        }
        else return 0;
    }
    private Item itemHeldRightHand;
    private Item itemHeldLeftHand;
    private Weapon equippedWeaponMainHand;
    private Weapon equippedWeaponOffHand;
    private string size;
    public int SpecialSizeModifier()
    {
        if (size == "Colossal") return 16;
        else if (size == "Gargantuan") return 12;
        else if (size == "Huge") return 8;
        else if (size == "Large") return 4;
        else if (size == "Medium") return 0;
        else if (size == "Small") return -4;
        else if (size == "Tiny") return -8;
        else if (size == "Diminutive") return -12;
        else if (size == "Fine") return -16;
        else throw new System.Exception("Failed to match a size modifier");
    }
    public int SizeAttackAndACModifier() {
        if (size == "Fine") return 8;
        else if (size == "Diminutive") return 4;
        else if (size == "Tiny") return 2;
        else if (size == "Small") return 1;
        else if (size == "Medium") return 0;
        else if (size == "Large") return -1;
        else if (size == "Huge") return -2;
        else if (size == "Gargantuan") return -4;
        else if (size == "Colossal") return -8;
        else throw new System.Exception("Failed to match a size modifier");
    }
    public int GrappleModifier() { return baseAttackBonus + StrengthModifier() + SpecialSizeModifier() + miscGrappleModifier; }
    public float staminaLength;
    public float currentStamina;
    public bool StaminaFull()
    {
        if (currentStamina >= staminaLength) return true;
        else return false;
    }
    public AudioClip[] damagedSounds;
    public Animator damageMaskAnimator;
    public FloatingCombatText myCombatText;
    private PauseCanvasToggle GuiManager() { return GameObject.Find("GUIParent").GetComponent<PauseCanvasToggle>(); }
    private AudioSource MyAudioSource() { return gameObject.GetComponent<AudioSource>(); }



    // Start is called before the first frame update
    void Start()
    {
        int newRoll = DiceRoller.RollAD20();
        Debug.Log("Dice roll library roller: " + newRoll.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // SET METHODS
    public void AdjustCopperPieces(int amount) { copperPieces += amount; }
    public void AdjustSilverPieces(int amount) { silverPieces += amount; }
    public void AdjustGoldPieces(int amount) { goldPieces += amount; }
    public void AdjustPlatinumPieces(int amount) { platinumPieces += amount; }
    public void SetHitDieTotalAmount(int newHitDieTotalAmount) { hitDieTotalAmount = newHitDieTotalAmount; }
    public void AdjustHitDieTotalAmount(int adjustAmount) { hitDieTotalAmount += adjustAmount; }
    public void SetHitDieSize(int newHitDieRangeSize) { hitDieSize = newHitDieRangeSize; }
    public void SetMiscGrappleModifier(int newMiscGrappleModifierTotal) { miscGrappleModifier = newMiscGrappleModifierTotal; }
    public void AdjustMiscGrappleModifier(int adjustAmount) { miscGrappleModifier += adjustAmount; }
    public void SetBaseAttackBonus(int newBaseAttackBonusTotal) { baseAttackBonus = newBaseAttackBonusTotal; }
    public void AdjustBaseAttackBonus(int adjustAmount) { baseAttackBonus += adjustAmount; }
    public void SetMiscToHitBonus(int newTotalMiscToHitBonus) { miscACBonus = newTotalMiscToHitBonus; }
    public void AdjustMiscToHitBonus(int adjustAmount) { miscACBonus += adjustAmount; }
    public void SetCurrentHealth(int currentHealthTotal) { currentHealth = currentHealthTotal; }
    public void AdjustCurrentHealth(int currentHealthAdjustAmount, string damageSource) {
        currentHealth += currentHealthAdjustAmount;
        if (currentHealthAdjustAmount < 0) MyAudioSource().PlayOneShot(damagedSounds[Random.Range(0, damagedSounds.Length)]);
        if (gameObject.GetComponent<PlayerCharacter>() != null && damageMaskAnimator != null)
        {
            damageMaskAnimator.SetBool("Mask", true);
            StartCoroutine(FadeDamageMask(0.1f));
        }

        if (currentHealth <= 0)
        {
            if (gameObject.GetComponent<PlayerCharacter>() != null)
            {
                GuiManager().GameOver(damageSource);
            }
            else
            {
                this.gameObject.GetComponent<NPC>().DoDead();
                this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                // this.enabled = false; (disabling for now, as maybe player can resurrect/heal dead/unconcious/dying npc or player
            }
            
        }
    }
    public void SetMaxHealth(int maxHealthTotal) { maxHealth = maxHealthTotal; }
    public void AdjustMaxHealth(int maxHealthAdjustAmount) { maxHealth += maxHealthAdjustAmount; }
    public void SetStrengthScore(int newStrengthScoreTotal) { strengthScore = newStrengthScoreTotal; }
    public void SetDexterityScore(int newDexterityScoreTotal) { dexterityScore = newDexterityScoreTotal; }
    public void SetConstitutionScore(int newConstitutionScoreTotal) { constitutionScore = newConstitutionScoreTotal; }
    public void SetIntelligenceScore(int newIntelligenceScoreTotal) { intelligenceScore = newIntelligenceScoreTotal; }
    public void SetWisdomScore(int newWisdomScoreTotal) { wisdomScore = newWisdomScoreTotal; }
    public void SetCharismaScore(int newCharismaScoreTotal) { charismaScore = newCharismaScoreTotal; }
    public void AdjustStrengthScore(int adjustAmount) { strengthScore += adjustAmount; }
    public void AdjustDexterityScore(int adjustAmount) { dexterityScore += adjustAmount; }
    public void AdjustConstitutionScore(int adjustAmount) { constitutionScore += adjustAmount; }
    public void AdjustWisdomScore(int adjustAmount) { wisdomScore += adjustAmount; }
    public void AdjustIntelligenceScore(int adjustAmount) { intelligenceScore += adjustAmount; }
    public void AdjustCharismaScore(int adjustAmount) { charismaScore += adjustAmount; }
    public void SetSize(string newSize) { size = newSize; }
    public void SetUniqueName(string newUniqueName) { uniqueName = newUniqueName; }
    public void SetRace(Race newRace) { race = newRace; }


    // GET METHODS
    public float MoneyTotalInGoldPieces() {
        return goldPieces + (copperPieces * 0.01f) + (silverPieces * 0.1f) + (platinumPieces * 10);
    }
    public Weapon GetEquippedWeaponMainHand() {
        if (equippedWeaponMainHand != null) return equippedWeaponMainHand;
        else return null;
    }
    public Weapon GetEquippedWeaponOffHand() {
        if (equippedWeaponOffHand != null) return equippedWeaponOffHand;
        else return null;
    }
    public Item GetHeldItemRightHand() {
        if (itemHeldRightHand != null) return itemHeldRightHand;
        else return null;
    }
    public Item GetHeldItemLeftHand() {
        if (itemHeldLeftHand != null) return itemHeldLeftHand;
        else return null;
    }
    public int GetMiscGrappleModifier() { return miscGrappleModifier; }
    public int GetCurrentHealth() { return currentHealth; }
    public int GetMaxHealth() { return maxHealth; }
    public int GetHitDieTotalAmount() { return hitDieTotalAmount; }
    public int GetHitDieSize() { return hitDieSize; }
    public int GetBaseAttackBonusTotal() { return baseAttackBonus; }
    public List<Job> GetJobList() { return jobList; }
    public int GetStrengthScore() { return strengthScore; }
    public int GetDexterityScore() { return dexterityScore; }
    public int GetConstitutionScore() { return constitutionScore; }
    public int GetWisdomScore() { return wisdomScore; }
    public int GetIntelligenceScore() { return intelligenceScore; }
    public int GetCharismaScore() { return charismaScore; }
    public int GetArmorBonus() { return ArmorBonus(); }
    public int GetShieldBonus() { return ShieldBonus(); }
    public int GetMiscACBonus() { return miscACBonus; }
    public string GetSize() { return size; }
    public string GetUniqueName() { return uniqueName; }
    public Race GetRace() { return race; }
    public List<Challenge> GetChallengeList() { return challengeList; }

    // JobList METHODS

    public void AddJob(Job newJob, int jobLevel) { newJob.SetJobLevel(jobLevel); jobList.Add(newJob); }
    public void LevelUpJob(string jobName, int howManyLevelsUp)
    {
        bool matchedJob = false;
        foreach (Job job in jobList)
        {
            if (job.JobName() == jobName)
            {
                job.AdjustJobLevel(howManyLevelsUp);
                matchedJob = true;
                break;
            }
        }
        if(matchedJob == false) throw new System.Exception("Tried to level up job " + jobName + " but failed to match job name.");

    }

    // ChallengeList Methods

    public Challenge GetChallengeFromChallengeList(string challengeName) {
        foreach (Challenge challenge in challengeList) {
            if (challenge.ChallengeName() == challengeName) return challenge;
        }
        return null;
    }

    public void ResetChallengeList()
    {
        challengeList = new List<Challenge>();
        challengeList.Add(ScriptableObject.CreateInstance<Appraise>());
        challengeList.Add(ScriptableObject.CreateInstance<Balance>());
        challengeList.Add(ScriptableObject.CreateInstance<Bluff>());
        challengeList.Add(ScriptableObject.CreateInstance<Climb>());
        challengeList.Add(ScriptableObject.CreateInstance<Concentration>());
        challengeList.Add(ScriptableObject.CreateInstance<Craft>());
        challengeList.Add(ScriptableObject.CreateInstance<DecipherScript>());
        challengeList.Add(ScriptableObject.CreateInstance<Diplomacy>());
        challengeList.Add(ScriptableObject.CreateInstance<DisableDevice>());
        challengeList.Add(ScriptableObject.CreateInstance<Disguise>());
        challengeList.Add(ScriptableObject.CreateInstance<EscapeArtist>());
        challengeList.Add(ScriptableObject.CreateInstance<Forgery>());
        challengeList.Add(ScriptableObject.CreateInstance<GatherInformation>());
        challengeList.Add(ScriptableObject.CreateInstance<HandleAnimal>());
        challengeList.Add(ScriptableObject.CreateInstance<Heal>());
        challengeList.Add(ScriptableObject.CreateInstance<Hide>());
        challengeList.Add(ScriptableObject.CreateInstance<Intimidate>());
        challengeList.Add(ScriptableObject.CreateInstance<Jump>());
        challengeList.Add(ScriptableObject.CreateInstance<KnowledgeArcana>());
        challengeList.Add(ScriptableObject.CreateInstance<KnowledgeArchitectureAndEngineering>());
        challengeList.Add(ScriptableObject.CreateInstance<KnowledgeDungeons>());
        challengeList.Add(ScriptableObject.CreateInstance<KnowledgeGeography>());
        challengeList.Add(ScriptableObject.CreateInstance<KnowledgeHistory>());
        challengeList.Add(ScriptableObject.CreateInstance<KnowledgeLocal>());
        challengeList.Add(ScriptableObject.CreateInstance<KnowledgeNature>());
        challengeList.Add(ScriptableObject.CreateInstance<KnowledgeNobilityAndRoyalty>());
        challengeList.Add(ScriptableObject.CreateInstance<KnowledgeReligion>());
        challengeList.Add(ScriptableObject.CreateInstance<KnowledgeThePlanes>());
        challengeList.Add(ScriptableObject.CreateInstance<Listen>());
        challengeList.Add(ScriptableObject.CreateInstance<MoveSilently>());
        challengeList.Add(ScriptableObject.CreateInstance<OpenLock>());
        challengeList.Add(ScriptableObject.CreateInstance<Perform>());
        challengeList.Add(ScriptableObject.CreateInstance<ProfessionCook>());
        challengeList.Add(ScriptableObject.CreateInstance<ProfessionMiner>());
        challengeList.Add(ScriptableObject.CreateInstance<Ride>());
        challengeList.Add(ScriptableObject.CreateInstance<Search>());
        challengeList.Add(ScriptableObject.CreateInstance<SenseMotive>());
        challengeList.Add(ScriptableObject.CreateInstance<SleightOfHand>());
        challengeList.Add(ScriptableObject.CreateInstance<SpeakLanguageCommon>());
        challengeList.Add(ScriptableObject.CreateInstance<SpeakLanguageGoblin>());
        challengeList.Add(ScriptableObject.CreateInstance<Spellcraft>());
        challengeList.Add(ScriptableObject.CreateInstance<Spot>());
        challengeList.Add(ScriptableObject.CreateInstance<Survival>());
        challengeList.Add(ScriptableObject.CreateInstance<Swim>());
        challengeList.Add(ScriptableObject.CreateInstance<Tumble>());
        challengeList.Add(ScriptableObject.CreateInstance<UseMagicDevice>());
        challengeList.Add(ScriptableObject.CreateInstance<UseRope>());
        foreach (Challenge challenge in challengeList)
        {
            //Debug.Log(challenge.ChallengeName());
            challenge.SetAttachedCharacter(this);
        }
    }

    // Character actions/methods

    public void MakeBasicAttack(Weapon weapon, bool melee, Character opponentCharacterSheet)
    {
        int attackRoll = DiceRoller.RollAD20();
        if (attackRoll >= weapon.CriticalRangeMin())
        {
            int criticalRoll;
            if (melee)
            {
                criticalRoll = MakeMeleeAttackRoll(DiceRoller.RollAD20());
            }
            else
            {
                criticalRoll = MakeRangedAttackRoll(DiceRoller.RollAD20());
            }
            if (criticalRoll >= opponentCharacterSheet.ArmorClass())
            {
                int damage = RollDamage(true, weapon);
                opponentCharacterSheet.myCombatText.DoAttackResultText("Critical! " + damage.ToString());
                opponentCharacterSheet.AdjustCurrentHealth(-damage, uniqueName);
                //Debug.Log("Critical! Damage = " + damage.ToString());
            }
            else
            {
                int damage = RollDamage(false, weapon);
                opponentCharacterSheet.myCombatText.DoAttackResultText(damage.ToString());
                opponentCharacterSheet.AdjustCurrentHealth(-damage, uniqueName);
                //Debug.Log("Did not confirm critical. Damage = " + damage.ToString());
            }
        }
        else
        {
            if (melee)
            {
                attackRoll = MakeMeleeAttackRoll(attackRoll);
            }
            else
            {
                attackRoll = MakeRangedAttackRoll(attackRoll);
            }
            if (attackRoll >= opponentCharacterSheet.ArmorClass())
            {
                int damage = RollDamage(false, weapon);
                opponentCharacterSheet.myCombatText.DoAttackResultText(damage.ToString());
                opponentCharacterSheet.AdjustCurrentHealth(-damage, uniqueName);
                //Debug.Log("Damage = " + damage.ToString());
            }
            else
            {
                opponentCharacterSheet.myCombatText.DoAttackResultText("Miss!");
                //Debug.Log("Miss - rolled " + attackRoll.ToString());
            }

        }
    }

    public int MakeMeleeAttackRoll(int baseRoll)
    {
        return baseRoll + baseAttackBonus + StrengthModifier() + SizeAttackAndACModifier() + miscAttackRollBonus;
    }

    public int MakeRangedAttackRoll(int baseRoll)
    {
        return baseRoll + baseAttackBonus + DexterityModifier() + SizeAttackAndACModifier() + miscAttackRollBonus; // need to add range penalty
    }

    public int RollDamage(bool critical, Weapon weapon)
    {
        if (critical)
        {
            int totalDamage = 0;
            for (int x = 0; x < weapon.CriticalRangeMultiplier(); x++)
            {
                int newDamageRoll = MakeSingleDamageRoll(weapon);
                totalDamage += newDamageRoll;
            }
            return totalDamage;
        }
        else
        {
            return MakeSingleDamageRoll(weapon);
        }
    }

    public int MakeSingleDamageRoll(Weapon weapon)
    {
        int damageRoll;
        if (weapon.Size() == "small")
        {
            damageRoll = weapon.DamageSmall();
        }
        else
        {
            damageRoll = weapon.DamageMedium();
        }
        if (weapon.ThrownWeapon() || weapon.LightMeleeWeapon() || weapon.OneHandedMeleeWeapon() || weapon.WeaponClass() == "natural")
        {
            damageRoll += StrengthModifier();
        }
        else if (weapon.TwoHandedMeleeWeapon())
        {
            damageRoll += Mathf.FloorToInt(StrengthModifier() * 1.5f);
        }
        return damageRoll;
    }

    public int MakeChallengeRoll(Challenge challenge) {
        return DiceRoller.RollAD20() + challenge.TotalChallengeBonus();
    }

    // misc Utility Methods

    private int CalculateModifier(int statScore)
    {
        if (statScore == 0 || statScore == 1)
        {
            return -5;
        }
        else if (statScore == 2 || statScore == 3)
        {
            return -4;
        }
        else if (statScore == 4 || statScore == 5)
        {
            return -3;
        }
        else if (statScore == 6 || statScore == 7)
        {
            return -2;
        }
        else if (statScore == 8 || statScore == 9)
        {
            return -1;
        }
        else if (statScore == 10 || statScore == 11)
        {
            return 0;
        }
        else if (statScore == 12 || statScore == 13)
        {
            return 1;
        }
        else if (statScore == 14 || statScore == 15)
        {
            return 2;
        }
        else if (statScore == 16 || statScore == 17)
        {
            return 3;
        }
        else if (statScore == 18 || statScore == 19)
        {
            return 4;
        }
        else if (statScore == 20 || statScore == 21)
        {
            return 5;
        }
        else if (statScore == 22 || statScore == 23)
        {
            return 6;
        }
        else if (statScore == 24 || statScore == 25)
        {
            return 7;
        }
        else if (statScore == 26 || statScore == 27)
        {
            return 8;
        }
        else if (statScore == 28 || statScore == 29)
        {
            return 9;
        }
        else if (statScore == 30 || statScore == 31)
        {
            return 10;
        }
        else
        {
            Debug.Log("CalculateModifier() returned 0 as a last resort, please check score value. Score value: " + statScore.ToString());
            return 0;
        }

    }

    private IEnumerator FadeDamageMask(float delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);
        damageMaskAnimator.SetBool("Mask", false);
    }

    public GlobalGameData GlobalDataMasterScript() {
        return GameObject.Find("GlobalDataMaster").GetComponent<GlobalGameData>();
    }

    private IEnumerator RefreshStaminaRoutine()
    {
        while (currentStamina < staminaLength)
        {
            if (GlobalDataMasterScript().IsGamePaused() == false)
            {
                yield return new WaitForSecondsRealtime(0.1f);
                currentStamina += 0.1f;
            }
        }
    }

}
