using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
public class LockScript : MonoBehaviour
{
    public bool locked;
    public GameObject whatIAmLocking;
    public int myDC;
    public float rollTextDisplayTime;
    public float failFreezeDelayTime;
    public Hand hand;
    public SteamVR_Action_Boolean grip = SteamVR_Input.GetBooleanAction("GrabGrip");
    public GameObject turnerSpot;
    public GameObject blockerSpot;
    public GameObject blockerPick;
    public GameObject turnerPick;
    private Vector3 turnerAttachPosOffset = new Vector3(-7.65f, 11.48f, -0.12f);
    Quaternion turnerAttachRotOffset = Quaternion.Euler(-109.2f, -90f, 0f);
    private Vector3 blockerAttachPosOffset = new Vector3(-9.91f, 1.18f, 2.03f);
    Quaternion blockerAttachRotOffset = Quaternion.Euler(47.611f, 8.301001f, -94.104f);
    private Vector3 chainTurnerAttachPosOffset = new Vector3(-5.47f, -1.92f, 1.48f);
    Quaternion chainTurnerAttachRotOffset = Quaternion.Euler(60.867f, -38.668f, -132.494f);
    private Vector3 chainBlockerAttachPosOffset = new Vector3(-4.85f, 0.93f, 1.01f);
    Quaternion chainBlockerAttachRotOffset = Quaternion.Euler(92.80099f, 110.495f, 9.148987f);
    private CharacterSheet playerSheet;
    private Hand leftHand;
    private Hand rightHand;
    public SteamVR_Behaviour_Pose leftHandPose;
    public SteamVR_Behaviour_Pose rightHandPose;
    private float targetRotationRightHand;
    private float targetRotationLeftHand;
    private bool leftHandInZone;
    private bool rightHandInZone;
    public float zoneSize;
    private bool justEnteredZoneLH;
    private bool justEnteredZoneRH;
    public AudioClip turningSound;
    public AudioClip zoneEnteredSound;
    public AudioClip shakeSound;
    public AudioClip successSound;
    private bool playTurningAudioLH;
    private bool playTurningAudioRH;
    private SteamVR_Action_Vibration hapticSig = SteamVR_Input.GetActionFromPath<SteamVR_Action_Vibration>("/actions/default/out/Haptic");
    private Text rollText;
    private SteamVR_Behaviour_Pose pose;
    private SteamVR_Action_Boolean trigger = SteamVR_Input.GetBooleanAction("Trigger");
    bool currentlyFailing;
    public bool pickingMe;
    public bool useAlternateOffset = false;
    public CharacterJoint[] cJointsToDemolish;
    private JointLimits chestOriginalLimits;
    public Interactable chainPiece1;
    public Interactable chainPiece2;
    public bool isPaused;

    

    [EnumFlags]
    [Tooltip("The flags used to attach this object to the hand.")]
    public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.TurnOnKinematic;

    // Start is called before the first frame update
    void Start()
    {
        pickingMe = false;
        currentlyFailing = false;
        playerSheet = null;
        isPaused = false;
        pose = GameObject.Find("MasterPose").GetComponent<SteamVR_Behaviour_Pose>();
        leftHand = GameObject.Find("LeftHand").GetComponent<Hand>();
        rightHand = GameObject.Find("RightHand").GetComponent<Hand>();
        rollText = this.transform.GetComponentInChildren<Text>(true);
        rollText.gameObject.SetActive(false);
        if (whatIAmLocking != null && locked)
        {
            if (whatIAmLocking.name.ToLower().Contains("chest"))
            {
                chestOriginalLimits = whatIAmLocking.GetComponent<HingeJoint>().limits;
                JointLimits jLimits = whatIAmLocking.GetComponent<HingeJoint>().limits;
                jLimits.min = 0;
                jLimits.max = 0;
                whatIAmLocking.GetComponent<HingeJoint>().limits = jLimits;
            }
        }

    }

    void FixedUpdate()
    {
        if (pickingMe)
        {
            if (currentlyFailing == false && playerSheet != null && playerSheet.playerIsLockpicking == true)
            {
                StartCoroutine(MovedEnoughForAudioLH());
                StartCoroutine(MovedEnoughForAudioRH());
                if (Mathf.Abs(rightHand.transform.localRotation.z - targetRotationRightHand) < zoneSize)
                {
                    if (rightHandInZone == false)
                    {
                        justEnteredZoneRH = true;
                        AudioSource aSource = rightHand.currentAttachedObject.GetComponent<HingeJoint>().connectedBody.gameObject.GetComponent<AudioSource>();
                        aSource.PlayOneShot(zoneEnteredSound);
                    }

                    else justEnteredZoneRH = false;

                    rightHandInZone = true;
                }
                else
                {
                    rightHandInZone = false;
                    justEnteredZoneRH = false;
                }

                if (Mathf.Abs(leftHand.transform.localRotation.z - targetRotationLeftHand) < zoneSize)
                {
                    if (leftHandInZone == false)
                    {
                        justEnteredZoneLH = true;
                        AudioSource aSource = leftHand.currentAttachedObject.GetComponent<HingeJoint>().connectedBody.gameObject.GetComponent<AudioSource>();
                        aSource.PlayOneShot(zoneEnteredSound);
                    }

                    else justEnteredZoneLH = false;

                    leftHandInZone = true;
                }
                else
                {
                    leftHandInZone = false;
                    justEnteredZoneLH = false;
                }


                if (playTurningAudioLH)
                {
                    AudioSource aSource = leftHand.currentAttachedObject.GetComponent<HingeJoint>().connectedBody.gameObject.GetComponent<AudioSource>();
                    aSource.clip = turningSound;
                    if (aSource.isPlaying == false)
                    {
                        aSource.PlayOneShot(turningSound);
                    }
                }

                if (playTurningAudioRH)
                {
                    AudioSource aSource = rightHand.currentAttachedObject.GetComponent<HingeJoint>().connectedBody.gameObject.GetComponent<AudioSource>();
                    aSource.clip = turningSound;
                    if (aSource.isPlaying == false)
                    {
                        aSource.PlayOneShot(turningSound);
                    }
                }


                if (leftHandInZone)
                {
                    hapticSig.Execute(0f, 0.1f, 20, 0.05f, leftHandPose.inputSource);
                }
                if (rightHandInZone)
                {
                    hapticSig.Execute(0f, 0.1f, 20, 0.05f, rightHandPose.inputSource);
                }
                if (leftHandInZone && rightHandInZone && trigger != null && trigger.GetStateDown(pose.inputSource))
                {
                    int openLockRoll = playerSheet.MakeOpenLockCheck();
                    bool rollIsSuccess;
                    if (openLockRoll >= myDC) rollIsSuccess = true;
                    else rollIsSuccess = false;
                    StartCoroutine(DisplayRollResult(rollIsSuccess, openLockRoll, rollTextDisplayTime));
                    if (rollIsSuccess)
                    {
                        AudioSource aSource = rightHand.currentAttachedObject.GetComponent<HingeJoint>().connectedBody.gameObject.GetComponent<AudioSource>();
                        aSource.PlayOneShot(successSound);
                        gameObject.transform.Find("padlock_top").transform.localPosition += new Vector3(0f, 0.01639f, 0f);
                        foreach (CharacterJoint cJoint in cJointsToDemolish) {
                            Destroy(cJoint);
                        }
                        locked = false;
                        gameObject.GetComponent<Rigidbody>().isKinematic = false;
                        rightHand.DetachObject(rightHand.currentAttachedObject);
                        if (whatIAmLocking != null) {
                            if (whatIAmLocking.CompareTag("Gate"))
                            {
                                whatIAmLocking.GetComponent<GateLockBehavior>().SetHinges();
                            }
                            else if (whatIAmLocking.name.ToLower().Contains("chest")) {
                                whatIAmLocking.GetComponent<HingeJoint>().limits = chestOriginalLimits;
                            }
                        }
                        if (chainPiece1 != null && chainPiece2 != null) {
                            chainPiece1.GetComponent<PooleyInteractable>().handIgnoreMe = false;
                            chainPiece2.GetComponent<PooleyInteractable>().handIgnoreMe = false;
                            chainPiece1.highlightOnHover = true;
                            chainPiece2.highlightOnHover = true;
                        }
                    }
                    else
                    {
                        AudioSource aSource = rightHand.currentAttachedObject.GetComponent<HingeJoint>().connectedBody.gameObject.GetComponent<AudioSource>();
                        aSource.PlayOneShot(shakeSound);
                        targetRotationRightHand = Random.Range(-0.6f, 0.6f);
                        targetRotationLeftHand = Random.Range(-0.6f, 0.6f);
                        FailDelay(failFreezeDelayTime);
                        //StartCoroutine(ShakeMe(1, 1, 1));
                    }
                }

            }
            else
            {
                if (leftHandInZone != false) leftHandInZone = false;
                if (rightHandInZone != false) rightHandInZone = false;
                if (justEnteredZoneLH != false) justEnteredZoneLH = false;
                if (justEnteredZoneRH != false) justEnteredZoneRH = false;
            }
        }
       

    }

    protected virtual void HandHoverUpdate(Hand hand) {
        if (GripFromEitherHand(hand)) {
            if (locked && !isPaused && hand.currentAttachedObject != null && (hand.currentAttachedObject.CompareTag("lockpickturner") || hand.currentAttachedObject.CompareTag("lockpickblocker")) && (hand.otherHand.currentAttachedObject == null || hand.otherHand.currentAttachedObject.CompareTag("lockpickblocker") || hand.otherHand.currentAttachedObject.CompareTag("lockpickturner"))) {
                playerSheet = hand.gameObject.GetComponentInParent<CharacterSheet>();
                GameObject heldTool = hand.currentAttachedObject;
                hand.DetachObject(heldTool);
                if (hand.otherHand.currentAttachedObject != null) {
                    hand.otherHand.DetachObject(hand.otherHand.currentAttachedObject);
                }
                blockerPick.transform.parent = blockerSpot.transform;
                turnerPick.transform.parent = turnerSpot.transform;
                if (useAlternateOffset)
                {
                    blockerPick.transform.localPosition = chainBlockerAttachPosOffset;
                    blockerPick.transform.localRotation = chainBlockerAttachRotOffset;
                    turnerPick.transform.localPosition = chainTurnerAttachPosOffset;
                    turnerPick.transform.localRotation = chainTurnerAttachRotOffset;
                }
                else {
                    blockerPick.transform.localPosition = blockerAttachPosOffset;
                    blockerPick.transform.localRotation = blockerAttachRotOffset;
                    turnerPick.transform.localPosition = turnerAttachPosOffset;
                    turnerPick.transform.localRotation = turnerAttachRotOffset;
                }
        
                HingeJoint tJoint = turnerPick.AddComponent<HingeJoint>();
                HingeJoint bJoint = blockerPick.AddComponent<HingeJoint>();
                tJoint.connectedBody = turnerSpot.GetComponent<Rigidbody>();
                bJoint.connectedBody = blockerSpot.GetComponent<Rigidbody>();
                tJoint.axis = new Vector3(0, 1, 0);
                bJoint.axis = new Vector3(0, 1, 0);
                tJoint.anchor = new Vector3(0, 0, -0.015f);
                bJoint.anchor = new Vector3(0, -0.075f, 0);

                hand.AttachObject(heldTool, GrabTypes.Grip, attachmentFlags);
                if (heldTool.CompareTag("lockpickturner"))
                {
                    hand.otherHand.AttachObject(blockerPick, GrabTypes.Grip, attachmentFlags);
                }
                else {
                    hand.otherHand.AttachObject(turnerPick, GrabTypes.Grip, attachmentFlags);
                }
                blockerPick.GetComponent<Rigidbody>().isKinematic = false;
                turnerPick.GetComponent<Rigidbody>().isKinematic = false;

                hand.HoverLock(null);
                hand.otherHand.HoverLock(null);
                playerSheet.playerIsLockpicking = true;
                playerSheet.currentLock = this.gameObject;
                pickingMe = true;
                targetRotationRightHand = Random.Range(-0.6f, 0.6f);
                targetRotationLeftHand = Random.Range(-0.6f, 0.6f);
            }
        }
    }

    protected bool GripFromEitherHand(Hand hand)
    {
        if (grip != null && grip.GetStateDown(hand.GetComponent<SteamVR_Behaviour_Pose>().inputSource))
        {
            return true;
        }
        else return false;
    }

    private IEnumerator MovedEnoughForAudioLH() {
        float startRot = leftHand.transform.localRotation.z;
        yield return new WaitForSeconds(0.1f);
        if (Mathf.Abs(leftHand.transform.localRotation.z - startRot) > 0.015f)
        {
            playTurningAudioLH = true;
        }
        else {
            playTurningAudioLH = false;
        }

    }

    private IEnumerator MovedEnoughForAudioRH()
    {
        float startRot = rightHand.transform.localRotation.z;
        yield return new WaitForSeconds(0.1f);
        if (Mathf.Abs(rightHand.transform.localRotation.z - startRot) > 0.015f)
        {
            playTurningAudioRH = true;
        }
        else
        {
            playTurningAudioRH = false;
        }

    }

    private IEnumerator DisplayRollResult(bool rollIsSuccessful, int rollResult, float displayTime) {
        rollText.gameObject.SetActive(true);
        rollText.text = rollResult.ToString();
        if (rollIsSuccessful) rollText.color = Color.green;
        else rollText.color = Color.red;
        yield return new WaitForSeconds(displayTime);
        rollText.text = "";
        rollText.color = Color.blue;
        rollText.gameObject.SetActive(false);
    }

    private IEnumerator FailDelay(float delayTime) {
        currentlyFailing = true;
        yield return new WaitForSeconds(delayTime);
        currentlyFailing = false;
    }

    private IEnumerator ShakeMe(float shakeTime, float speed, float amount) {
        Vector3 startPos = transform.position;
        float startTime = Time.time;
        while (startTime + shakeTime >= Time.time)
        {
            gameObject.transform.position = new Vector3(Mathf.Sin(Time.time * speed) * amount, Mathf.Sin(Time.time * speed) * amount, Mathf.Sin(Time.time * speed) * amount);
        }
        transform.position = startPos;
        yield return new WaitForSeconds(0.01f);
    }
}
