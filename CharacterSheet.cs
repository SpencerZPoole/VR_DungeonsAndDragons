using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CharacterSheet : MonoBehaviour
{
    public bool isPlayerCharacter;
    public bool isZombie;
    public bool isRat;
    public FloatingCombatText myCombatText;
    public bool playerIsLockpicking;
    public float currentHealth;
    public float MaxHealth() {
        if (this.gameObject.name.ToLower().Contains("dummy")) return 5000;
        if (isZombie) return 16;
        if (playerLevel == 1)
        {
            if (playerClass.ToLower() == "fighter")
            {
                return 10 + ConstitutionModifier();
            }
            else {
                Debug.Log("maxHealth() didn't recognize playerClass '" + playerClass + "'. Please check. Returning default '13'.");
                return 13;
            }
        }
        else {
            Debug.Log("maxHealth() didn't receive '1' as player level. Please check. Returning default '13'.");
            return 13;
        }
    }
    public string playerRace = "human";
    public string playerClass = "fighter";
    private int ArmorClass() { return 10 + armorBonus + shieldBonus + SizeModifier() + DexterityModifier(); }
    public int strengthScore = 18;
    private int StrengthModifier() { return CalculateModifier(strengthScore); }
    public int dexterityScore = 16;
    private int DexterityModifier() { return CalculateModifier(dexterityScore); }
    public int constitutionScore = 16;
    private int ConstitutionModifier() { return CalculateModifier(constitutionScore); }
    public int intelligenceScore = 10;
    private int IntelligenceModifier() { return CalculateModifier(intelligenceScore); }
    public int wisdomScore = 13;
    private int WisdomModifier() { return CalculateModifier(wisdomScore); }
    public int charismaScore = 13;
    private int CharismaModifier() { return CalculateModifier(charismaScore); }
    public int playerLevel = 1;
    private int SkillPointsAtFirstLevel() {
        if (playerClass.ToLower() == "fighter")
        {
            return (2 + IntelligenceModifier()) * 4;
        }
        else {
            Debug.Log("skillPointsAtFirstLevel() failed to recognize playerClass '" + playerClass + "'. Please check. Returning default '0'.");
            return 0;
        }
    }
    public int armorBonus = 4;
    public int shieldBonus = 0;
    private int BaseAttackBonus() {
        if (playerLevel == 1)
        {
            if (isZombie) return 2;
            if (playerClass.ToLower() == "fighter")
            {
                return 1;
            }
            
            else
            {
                Debug.Log("BaseAttackBonus() didn't recognize playerClass '" + playerClass + "'. Please check. Returning default '0'.");
                return 0;
            }
        }
        else
        {
            Debug.Log("BaseAttackBonus() didn't receive '1' as player level. Please check. Returning default '0'.");
            return 0;
        }
    }
    private string Size() {
        if (playerRace.ToLower() == "human" || playerRace.ToLower() == "half-orc" || playerRace.ToLower() == "elf" || playerRace.ToLower() == "half-elf")
        {
            return "medium";
        }
        else if (playerRace.ToLower() == "gnome" || playerRace.ToLower() == "halfling" || playerRace.ToLower() == "dwarf") {
            return "small";
        }
        else {
            Debug.Log("size() did not recognize playerRace. Please check. Returning default 'medium'. playerRace = " + playerRace);
            return "medium";
        }
    }
    private int SizeModifier() {
        if (Size().ToLower() == "small")
        {
            return 1;
        }
        else if (Size().ToLower() == "medium")
        {
            return 0;
        }
        else if (Size().ToLower() == "large")
        {
            return -1;
        }
        else {
            Debug.Log("sizeModifier() could not match the player's size. Returning 0. Please check. size = " + Size());
            return 0;
        }
    }
    private int GrappleModifier() {
        if (Size().ToLower() == "small")
        {
            return -4;
        }
        else if (Size().ToLower() == "medium")
        {
            return 0;
        }
        else if (Size().ToLower() == "large")
        {
            return +4;
        }
        else
        {
            Debug.Log("grappleModifier() could not match the player's size. Returning 0. Please check. size = " + Size());
            return 0;
        }
    }
    public Weapon equippedWeaponRightHand;
    public Weapon equippedWeaponLeftHand;
    private int RangePenalty() { return 0; } //will implement later if we do ranged attacks
    public int openLockRanks = 4;
    public int spotRanks;
    public int listenRanks;
    public int moveSilentlyRanks;
    public int hideRanks;
    public GameObject currentLock;
    public float staminaLength = 6;
    public float currentStamina;
    private bool StaminaFull() {
        if (currentStamina >= staminaLength) return true;
        else return false;
    }
    public bool isPaused;
    public string entityName;
    public int currentGold;
    public string currentDungeon;
    private PauseCanvasToggle guiManager;
    [SerializeField]
    public Animator damageMaskAnimator;
    private AudioSource myAudioSource;
    public AudioClip[] damagedSounds;

    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.GetComponent<AudioSource>() != null) myAudioSource = this.gameObject.GetComponent<AudioSource>();
    	guiManager = GameObject.Find("GUIParent").GetComponent<PauseCanvasToggle>();
        currentStamina = staminaLength;
        currentDungeon = "Demo Dungeon"; //Always the same for the demo build. Will be dynamic later on.
        isPaused = false;
        currentLock = null;
        playerIsLockpicking = false;
        currentHealth = MaxHealth(); //make sure player starts with full health. This will happen every time we start the game at the moment.
        //for (int x = 0; x < 50; x++) {
        //    MakeBasicAttack(true, this);
        //}                                          uncomment this loop to simulate 50 basic attacks and log the results to the console
        
        if (isPlayerCharacter){
            entityName = "Jimbob";
            currentGold = 0;
        }
        else if (isZombie){
            entityName = "zombie";
            currentGold = 2;
            //Use current gold on enemies for future drop spawning/kill reward purposes.
        }
    } 

    // Update is called once per frame
    void Update()
    {
        if(currentStamina > staminaLength){
            currentStamina = staminaLength;
        }
        //if(Input.GetKeyDown(KeyCode.O)){
        //    StaminaTest();
        //}
    }

    public string GetSize() { return Size(); }
    public int CalculateModifier(int statScore) {
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
        else {
            Debug.Log("CalculateModifier() returned 0 as a last resort, please check score value. Score value: " + statScore.ToString());
            return 0;
        }
    }

    public void MakeBasicAttack(Weapon weapon, bool melee, CharacterSheet opponentCharacterSheet)
    {
        int attackRoll = RollAD20();
        if (attackRoll >= weapon.CriticalRangeMin())
        {
            int criticalRoll;
            if (melee)
            {
                criticalRoll = MakeMeleeAttackRoll(RollAD20());
            }
            else
            {
                criticalRoll = MakeRangedAttackRoll(RollAD20());
            }
            if (criticalRoll >= opponentCharacterSheet.GetArmorClass())
            {
                int damage = RollDamage(true, weapon);
                opponentCharacterSheet.myCombatText.DoAttackResultText("Critical! " + damage.ToString());
                opponentCharacterSheet.SetCurrentHealth(-damage, entityName);
                //Debug.Log("Critical! Damage = " + damage.ToString());
            }
            else
            {
                int damage = RollDamage(false, weapon);
                opponentCharacterSheet.myCombatText.DoAttackResultText(damage.ToString());
                opponentCharacterSheet.SetCurrentHealth(-damage, entityName);
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
            if (attackRoll >= opponentCharacterSheet.GetArmorClass())
            {
                int damage = RollDamage(false, weapon);
                opponentCharacterSheet.myCombatText.DoAttackResultText(damage.ToString());
                opponentCharacterSheet.SetCurrentHealth(-damage, entityName);
                //Debug.Log("Damage = " + damage.ToString());
            }
            else {
                opponentCharacterSheet.myCombatText.DoAttackResultText("Miss!");
                //Debug.Log("Miss - rolled " + attackRoll.ToString());
            }

        }
    }

    public int MakeMeleeAttackRoll(int baseRoll) {
        return baseRoll + BaseAttackBonus() + StrengthModifier() + SizeModifier();
    }

    public int MakeRangedAttackRoll(int baseRoll)
    {
        return baseRoll + BaseAttackBonus() + DexterityModifier() + SizeModifier() + RangePenalty();
    }

    public int RollAD20() {
        return Random.Range(1, 21);
    }
    public int GetArmorClass() {
        return ArmorClass();
    }

    public int RollDamage(bool critical, Weapon weapon) {
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
        else {
            return MakeSingleDamageRoll(weapon);
        }
    }

    public int MakeSingleDamageRoll(Weapon weapon) {
        int damageRoll;
        if (weapon.Size() == "small")
        {
            damageRoll = weapon.DamageSmall();
        }
        else {
            damageRoll = weapon.DamageMedium();
        }
        if (weapon.ThrownWeapon() || weapon.LightMeleeWeapon() || weapon.OneHandedMeleeWeapon() || weapon.WeaponClass() == "natural")
        {
            damageRoll += StrengthModifier();
        }
        else if (weapon.TwoHandedMeleeWeapon()) {
            damageRoll += Mathf.FloorToInt(StrengthModifier() * 1.5f);
        }
        return damageRoll;
    }

    public int MakeOpenLockCheck() {
        return RollAD20() + DexterityModifier() + openLockRanks;
    }

    public int MakeMoveSilentlyCheck()
    {
        return RollAD20() + DexterityModifier() + moveSilentlyRanks;
    }
    public int MakeSpotCheck()
    {
        return RollAD20() + WisdomModifier() + spotRanks;
    }

    public int MakeListenCheck()
    {
        return RollAD20() + WisdomModifier() + listenRanks;
    }

    public int MakeHideCheck()
    {
        return RollAD20() + DexterityModifier() + hideRanks;
    }


    public void OnTryAttackMessage(Weapon weapon, CharacterSheet defenderSheet) {
        if (StaminaFull() && !isPaused) {
            currentStamina = 0;
            StartCoroutine(RefreshStaminaRoutine());
            MakeBasicAttack(weapon, true, defenderSheet);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isPlayerCharacter && !isPaused && currentHealth > 0)
        {
            if (collision.gameObject.GetComponent<PooleyInteractable>() != null)
            {
                PooleyInteractable pInter = collision.gameObject.GetComponent<PooleyInteractable>();
                if (pInter.IsEquipped() && pInter.isWeapon) {
                    if (isZombie) this.gameObject.GetComponent<ZombieBehavior>().StartCombat();
                    if (isRat) this.gameObject.GetComponent<DireRatBehavior>().StartCombat();
                    pInter.GetPlayerSheet().OnTryAttackMessage(collision.gameObject.GetComponent<Weapon>(), this);
                }
            }
        }
    }

    public void SetCurrentHealth(int amount, string damageSource) {
        currentHealth += amount;
        if(amount < 0) myAudioSource.PlayOneShot(damagedSounds[Random.Range(0, damagedSounds.Length)]);
        if (isPlayerCharacter && damageMaskAnimator != null){
            damageMaskAnimator.SetBool("Mask", true);
            StartCoroutine(FadeDamageMask(0.1f));
        }

        if (currentHealth <= 0) {
        	if (isPlayerCharacter){
        		guiManager.GameOver(damageSource);
        	}
            if (isZombie) {
                this.gameObject.GetComponent<ZombieBehavior>().DoDead();
                this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                this.enabled = false;
            }
            if (isRat)
            {
                this.gameObject.GetComponent<DireRatBehavior>().DoDead();
                this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                this.enabled = false;
            }
        }
    }

    private IEnumerator RefreshStaminaRoutine() {
        while (currentStamina < staminaLength) {
            if (!isPaused){
                yield return new WaitForSecondsRealtime(0.1f);
                currentStamina += 0.1f;
            }
        }
    }

    private IEnumerator FadeDamageMask(float delayTime){
        yield return new WaitForSecondsRealtime(delayTime);
        damageMaskAnimator.SetBool("Mask", false);
    }


    //Stamina refill test for debugging purposes.
    //public void StaminaTest(){
    //    currentStamina = 0;
    //    StartCoroutine(RefreshStaminaRoutine());
    //}

}
