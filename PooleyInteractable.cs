//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Basic throwable object
//
//=============================================================================

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    [RequireComponent(typeof(Rigidbody))]
    public class PooleyInteractable : MonoBehaviour
    {
        [EnumFlags]
        [Tooltip("The flags used to attach this object to the hand.")]
        public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.TurnOnKinematic;

        [Tooltip("The local point which acts as a positional and rotational offset to use while held")]
        public Transform attachmentOffset;

        [Tooltip("How fast must this object be moving to attach due to a trigger hold instead of a trigger press? (-1 to disable)")]
        public float catchingSpeedThreshold = -1;

        public ReleaseStyle releaseVelocityStyle = ReleaseStyle.GetFromHand;

        [Tooltip("The time offset used when releasing the object with the RawFromHand option")]
        public float releaseVelocityTimeOffset = -0.011f;

        public float scaleReleaseVelocity = 1.1f;

        [Tooltip("The release velocity magnitude representing the end of the scale release velocity curve. (-1 to disable)")]
        public float scaleReleaseVelocityThreshold = -1.0f;
        [Tooltip("Use this curve to ease into the scaled release velocity based on the magnitude of the measured release velocity. This allows greater differentiation between a drop, toss, and throw.")]
        public AnimationCurve scaleReleaseVelocityCurve = AnimationCurve.EaseInOut(0.0f, 0.1f, 1.0f, 1.0f);

        [Tooltip("When detaching the object, should it return to its original parent?")]
        public bool restoreOriginalParent = false;



        protected VelocityEstimator velocityEstimator;
        public bool handIgnoreMe;
        protected bool attached = false;
        protected float attachTime;
        protected Vector3 attachPosition;
        protected Quaternion attachRotation;
        protected Transform attachEaseInTransform;
        public SteamVR_Action_Boolean grip = SteamVR_Input.GetBooleanAction("GrabGrip");
        private SteamVR_Action_Vibration hapticSig = SteamVR_Input.GetActionFromPath<SteamVR_Action_Vibration>("/actions/default/out/Haptic");
        public UnityEvent onPickUp;
        public UnityEvent onDetachFromHand;
        public HandEvent onHeldUpdate;
        public GameObject currentItemHolderSpot;
        protected RigidbodyInterpolation hadInterpolation = RigidbodyInterpolation.None;
        public Rigidbody parentContainerRigidbody;
        public bool parentKinematicWhileOpening;
        private bool parentContainerKinematicDefaultState;
        private Hand leftHand;
        private Hand rightHand;
        private GameObject leftHandAttachmentGO;
        private GameObject rightHandAttachmentGO;
        protected new Rigidbody rigidbody;
        [HideInInspector]
        public Interactable interactable;
        public bool defaultItemToNonKinematic;
        public bool equippable;
        public bool isWeapon;
        private GameObject playerBodyColliderGO;
        public Vector3 lockpickRot;
        public Vector3 lockpickStartPos;
        CharacterSheet playerSheet;
        private GameObject playerCamera;
        private Hand myCurrentHand;
        private bool equipped;
        private bool pulseDelaying;
        public bool sheatheMe;
        public bool isPaused;
        private AudioSource playerAudioSource;
        public AudioClip coinSound;

        //-------------------------------------------------
        protected virtual void Awake()
        {
            sheatheMe = false;
            pulseDelaying = false;
            isPaused = false;
            playerSheet = GameObject.Find("Player").GetComponent<CharacterSheet>();
            velocityEstimator = GetComponent<VelocityEstimator>();
            interactable = GetComponent<Interactable>();
            currentItemHolderSpot = null;
            lockpickRot = this.transform.localEulerAngles;
            lockpickStartPos = this.transform.localPosition;
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.maxAngularVelocity = 50.0f;
            if (gameObject.CompareTag("Handle") && parentContainerRigidbody != null)
            {
                parentContainerKinematicDefaultState = parentContainerRigidbody.isKinematic;
            }

            if (attachmentOffset != null)
            {
                // remove?
                //interactable.handFollowTransform = attachmentOffset;
            }

        }

        private void FixedUpdate()
        {

        }

        private void Start()
        {
            playerCamera = GameObject.Find("VRCamera");
            playerBodyColliderGO = GameObject.Find("BodyCollider");
            leftHand = GameObject.Find("LeftHand").GetComponent<Hand>();
            rightHand = GameObject.Find("RightHand").GetComponent<Hand>();
            leftHandAttachmentGO = leftHand.transform.Find("ObjectAttachmentPoint").gameObject;
            rightHandAttachmentGO = rightHand.transform.Find("ObjectAttachmentPoint").gameObject;
            playerAudioSource = GameObject.Find("Player").GetComponent<AudioSource>();
        }

        //-------------------------------------------------
        protected virtual void OnHandHoverBegin(Hand hand)
        {
            bool showHint = false;

            // "Catch" the throwable by holding down the interaction button instead of pressing it.
            // Only do this if the throwable is moving faster than the prescribed threshold speed,
            // and if it isn't attached to another hand
            if (!attached && catchingSpeedThreshold != -1 && !isPaused)
            {
                float catchingThreshold = catchingSpeedThreshold * SteamVR_Utils.GetLossyScale(Player.instance.trackingOriginTransform);

                GrabTypes bestGrabType = hand.GetBestGrabbingType();

                if (bestGrabType != GrabTypes.None)
                {
                    if (rigidbody.velocity.magnitude >= catchingThreshold)
                    {
                        if (defaultItemToNonKinematic)
                        {
                            rigidbody.isKinematic = false;
                        }
                        if (currentItemHolderSpot != null)
                        {
                            currentItemHolderSpot.GetComponent<ItemHolderSpot>().currentHeldObject = null;
                            currentItemHolderSpot = null;
                        }
                        hand.AttachObject(gameObject, bestGrabType, attachmentFlags);
                        showHint = false;
                    }
                }
            }

            if (showHint)
            {
                hand.ShowGrabHint();
            }
        }


        //-------------------------------------------------
        protected virtual void OnHandHoverEnd(Hand hand)
        {
            hand.HideGrabHint();
        }


        //-------------------------------------------------
        protected virtual void HandHoverUpdate(Hand hand)
        {
            //GrabTypes startingGrabType = hand.GetGrabStarting();

            if (!handIgnoreMe && GripFromEitherHand(hand) && !attached && hand.currentAttachedObjectInfo.HasValue == false && !isPaused)
            {
                if (gameObject.CompareTag("Handle") && parentKinematicWhileOpening) {
                    parentContainerRigidbody.isKinematic = true;
                }
                if (currentItemHolderSpot != null)
                {
                    currentItemHolderSpot.GetComponent<ItemHolderSpot>().currentHeldObject = null;
                    currentItemHolderSpot = null;
                }
                if (defaultItemToNonKinematic)
                {
                    rigidbody.isKinematic = false;
                }
                if (gameObject.CompareTag("lockpickturner") || gameObject.CompareTag("lockpickblocker")) {
                    transform.parent = null;
                }

                hand.AttachObject(gameObject, GrabTypes.Grip, attachmentFlags, attachmentOffset);
                hand.HideGrabHint();

            }
            else if (!handIgnoreMe && interactable.attachedToHand != null && !isPaused) {
                if (GripFromEitherHand(interactable.attachedToHand.otherHand) && attached && interactable.attachedToHand.otherHand.currentAttachedObject == null)
                {
                    interactable.attachedToHand.DetachObject(gameObject);
                    if (currentItemHolderSpot != null)
                    {
                        currentItemHolderSpot.GetComponent<ItemHolderSpot>().currentHeldObject = null;
                        currentItemHolderSpot = null;
                    }
                    if (defaultItemToNonKinematic)
                    {
                        rigidbody.isKinematic = false;
                    }
                    if (gameObject.CompareTag("lockpickturner") || gameObject.CompareTag("lockpickblocker"))
                    {
                        transform.parent = null;
                    }

                    hand.AttachObject(gameObject, GrabTypes.Grip, attachmentFlags, attachmentOffset);
                    hand.HideGrabHint();

                }

            }


        }

        //-------------------------------------------------
        protected virtual void OnAttachedToHand(Hand hand)
        {
            if (gameObject.name.ToLower().Contains("gold")) {
                playerSheet.currentGold += 1;
                if (coinSound != null) playerAudioSource.PlayOneShot(coinSound);
                hand.DetachObject(gameObject);
                GameObject.Destroy(gameObject);
            }
            SetCorrectItemPositionAndRotation(hand);
            myCurrentHand = hand;
            equipped = true;
            if (this.gameObject.GetComponent<Weapon>() != null) {
                if (hand == leftHand) playerSheet.equippedWeaponLeftHand = this.gameObject.GetComponent<Weapon>();
                else playerSheet.equippedWeaponRightHand = this.gameObject.GetComponent<Weapon>();
            }
            transform.gameObject.layer = 2;
            hadInterpolation = this.rigidbody.interpolation;
            attached = true;
            onPickUp.Invoke();
            rigidbody.interpolation = RigidbodyInterpolation.None;
            if (velocityEstimator != null)
                velocityEstimator.BeginEstimatingVelocity();
            attachTime = Time.time;
            attachPosition = transform.position;
            attachRotation = transform.rotation;

        }


        //-------------------------------------------------
        protected virtual void OnDetachedFromHand(Hand hand)
        {
            attached = false;
            //myCurrentHand = null;
            equipped = false;
            if (this.gameObject.GetComponent<Weapon>() != null)
            {
                if (hand == leftHand) playerSheet.equippedWeaponLeftHand = null;
                else playerSheet.equippedWeaponRightHand = null;
            }
            transform.gameObject.layer = 0;
            onDetachFromHand.Invoke();
            rigidbody.interpolation = hadInterpolation;

            Vector3 velocity;
            Vector3 angularVelocity;

            GetReleaseVelocities(hand, out velocity, out angularVelocity);

            rigidbody.velocity = velocity;
            rigidbody.angularVelocity = angularVelocity;
            if (defaultItemToNonKinematic && currentItemHolderSpot == null)
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
            }
            if (gameObject.CompareTag("Handle") && parentKinematicWhileOpening)
            {
                parentContainerRigidbody.isKinematic = parentContainerKinematicDefaultState;
            }
            else if (gameObject.CompareTag("lockpickturner") || gameObject.CompareTag("lockpickblocker")) {
                hand.HoverUnlock(null);
                if (playerSheet.playerIsLockpicking)
                {
                    playerSheet.playerIsLockpicking = false;
                    if (playerSheet.currentLock != null) {
                        playerSheet.currentLock.GetComponent<LockScript>().pickingMe = false;
                        playerSheet.currentLock = null;
                    }
                    hand.otherHand.DetachObject(hand.otherHand.currentAttachedObject);

                }
                playerSheet.playerIsLockpicking = false;
                if (gameObject.GetComponent<HingeJoint>() != null)
                {
                    Destroy(gameObject.GetComponent<HingeJoint>());
                }
                transform.GetComponent<Rigidbody>().isKinematic = true;
                transform.parent = leftHand.gameObject.transform;
                transform.localEulerAngles = lockpickRot;
                transform.localPosition = lockpickStartPos;

            }


        }


        public virtual void GetReleaseVelocities(Hand hand, out Vector3 velocity, out Vector3 angularVelocity)
        {
            if (hand.noSteamVRFallbackCamera && releaseVelocityStyle != ReleaseStyle.NoChange)
                releaseVelocityStyle = ReleaseStyle.ShortEstimation; // only type that works with fallback hand is short estimation.

            switch (releaseVelocityStyle)
            {
                case ReleaseStyle.ShortEstimation:
                    if (velocityEstimator != null)
                    {
                        velocityEstimator.FinishEstimatingVelocity();
                        velocity = velocityEstimator.GetVelocityEstimate();
                        angularVelocity = velocityEstimator.GetAngularVelocityEstimate();
                    }
                    else
                    {
                        Debug.LogWarning("[SteamVR Interaction System] Throwable: No Velocity Estimator component on object but release style set to short estimation. Please add one or change the release style.");

                        velocity = rigidbody.velocity;
                        angularVelocity = rigidbody.angularVelocity;
                    }
                    break;
                case ReleaseStyle.AdvancedEstimation:
                    hand.GetEstimatedPeakVelocities(out velocity, out angularVelocity);
                    break;
                case ReleaseStyle.GetFromHand:
                    velocity = hand.GetTrackedObjectVelocity(releaseVelocityTimeOffset);
                    angularVelocity = hand.GetTrackedObjectAngularVelocity(releaseVelocityTimeOffset);
                    break;
                default:
                case ReleaseStyle.NoChange:
                    velocity = rigidbody.velocity;
                    angularVelocity = rigidbody.angularVelocity;
                    break;
            }

            if (releaseVelocityStyle != ReleaseStyle.NoChange)
            {
                float scaleFactor = 1.0f;
                if (scaleReleaseVelocityThreshold > 0)
                {
                    scaleFactor = Mathf.Clamp01(scaleReleaseVelocityCurve.Evaluate(velocity.magnitude / scaleReleaseVelocityThreshold));
                }

                velocity *= (scaleFactor * scaleReleaseVelocity);
            }
        }

        //-------------------------------------------------
        protected virtual void HandAttachedUpdate(Hand hand)
        {
            if (GripFromEitherHand(hand) && attached && !hand.IsStillHovering(hand.GetComponent<PooleyHandBehaviour>().lastHoveredInteractable))
            {
                if (currentItemHolderSpot == null && !sheatheMe) {
                    hand.DetachObject(gameObject, restoreOriginalParent);
                }

                // Uncomment to detach ourselves late in the frame.
                // This is so that any vehicles the player is attached to
                // have a chance to finish updating themselves.
                // If we detach now, our position could be behind what it
                // will be at the end of the frame, and the object may appear
                // to teleport behind the hand when the player releases it.
                //StartCoroutine( LateDetach( hand ) );
            }
            if (equippable) {
                if (Vector3.Distance(gameObject.transform.position, playerCamera.transform.position) >= 1.2f) {
                    StartCoroutine(ReattachItem(hand));
                }

            }

            if (onHeldUpdate != null)
                onHeldUpdate.Invoke(hand);
        }


        //-------------------------------------------------
        protected virtual IEnumerator LateDetach(Hand hand)
        {
            yield return new WaitForEndOfFrame();

            hand.DetachObject(gameObject, restoreOriginalParent);
        }


        //-------------------------------------------------
        protected virtual void OnHandFocusAcquired(Hand hand)
        {
            gameObject.SetActive(true);

            if (velocityEstimator != null)
                velocityEstimator.BeginEstimatingVelocity();
        }

        //-------------------------------------------------
        protected virtual void OnHandFocusLost(Hand hand)
        {
            gameObject.SetActive(false);

            if (velocityEstimator != null)
                velocityEstimator.FinishEstimatingVelocity();
        }
        protected bool GripFromEitherHand(Hand hand)
        {
            if (grip != null && grip.GetStateDown(hand.GetComponent<SteamVR_Behaviour_Pose>().inputSource))
            {
                return true;
            }
            else return false;
        }

        private void SetCorrectItemPositionAndRotation(Hand currentHand) {
            Vector3 torchAttachmentVectorLeft = new Vector3(0.0349f, -0.0009f, -0.1847f);
            Quaternion torchAttachmentRotationLeft = Quaternion.Euler(175.4f, -104.1f, -95.8f);
            Vector3 torchAttachmentVectorRight = new Vector3(0.0101f, -0.0103f, -0.1856f);
            Quaternion torchAttachmentRotationRight = Quaternion.Euler(-162.2f, -84.7f, -95.8f);

            Vector3 barrelLidAttachmentVectorLeft = new Vector3(0.055f, 0.0101f, -0.1028f);
            Quaternion barrelLidAttachmentRotationLeft = Quaternion.Euler(-202.11f, 85.97f, 0.0f);
            Vector3 barrelLidAttachmentVectorRight = new Vector3(-0.0517f, 0.007f, -0.0983f);
            Quaternion barrelLidAttachmentRotationRight = Quaternion.Euler(4.4f, 85.96999f, -1.5258f);

            Vector3 greatswordAttachmentVectorLeft = new Vector3(0.1598f, -0.0044f, -0.1456f);
            Quaternion greatswordAttachmentRotationLeft = Quaternion.Euler(-0.24f, -96.2f, 2.6f);
            Vector3 greatswordAttachmentVectorRight = new Vector3(0.1588f, -0.0091f, -0.1496f);
            Quaternion greatswordAttachmentRotationRight = Quaternion.Euler(-0.24f, -88.1f, 2.6f);

            Vector3 handleAttachmentVectorLeft = new Vector3(0.0f, -0.008f, -0.098f);
            Quaternion handleAttachmentRotationLeft = Quaternion.Euler(-114.5f, 95.8f, -0.3f);
            Vector3 handleAttachmentVectorRight = new Vector3(0.0f, 0.0f, -0.07f);
            Quaternion handleAttachmentRotationRight = Quaternion.Euler(-400.2f, -607.3f, 192.6f);

            Vector3 lockpickTurnerAttachmentVectorRight = new Vector3(-0.0212f, -0.0828f, -0.0466f);
            Quaternion lockpickTurnerAttachmentRotationRight = Quaternion.Euler(54.374f, -177.352f, -171.408f);
            Vector3 lockpickTurnerAttachmentVectorLeft = new Vector3(0.0143f, -0.0787f, -0.0357f);
            Quaternion lockpickTurnerAttachmentRotationLeft = Quaternion.Euler(114.9f, 0f, 0f);

            Vector3 lockpickBlockerAttachmentVectorRight = new Vector3(-0.0178f, -0.0479f, -0.0208f);
            Quaternion lockpickBlockerAttachmentRotationRight = Quaternion.Euler(125.626f, 2.647995f, 8.591995f);
            Vector3 lockpickBlockerAttachmentVectorLeft = new Vector3(0.0145f, -0.0452f, -0.0287f);
            Quaternion lockpickBlockerAttachmentRotationLeft = Quaternion.Euler(50.872f, -161.758f, -164.938f);

            Vector3 daggerAttachmentVectorRight = new Vector3(-0.1454f, 0.0201f, -0.0827f);
            Quaternion daggerAttachmentRotationRight = Quaternion.Euler(11.961f, -261.368f, -178.026f);
            Vector3 daggerAttachmentVectorLeft = new Vector3(-0.1492f, 0.0037f, -0.1397f);
            Quaternion daggerAttachmentRotationLeft = Quaternion.Euler(3.253f, 77.93301f, -175.364f);

            if (currentHand == leftHand) {
                leftHandAttachmentGO.transform.localRotation = Quaternion.identity;
                leftHandAttachmentGO.transform.localPosition = new Vector3(0, 0, 0);
            }
            else if (currentHand == rightHand) {
                rightHandAttachmentGO.transform.localRotation = Quaternion.identity;
                rightHandAttachmentGO.transform.localPosition = new Vector3(0, 0, 0);
            }

            if (transform.gameObject.CompareTag("Torch")) {
                if (currentHand == leftHand)
                {
                    leftHandAttachmentGO.transform.localRotation = torchAttachmentRotationLeft;
                    leftHandAttachmentGO.transform.localPosition = torchAttachmentVectorLeft;
                }
                else if (currentHand == rightHand) {
                    rightHandAttachmentGO.transform.localRotation = torchAttachmentRotationRight;
                    rightHandAttachmentGO.transform.localPosition = torchAttachmentVectorRight;
                }
            }
            else if (transform.gameObject.CompareTag("BarrelLid"))
            {
                if (currentHand == leftHand)
                {
                    leftHandAttachmentGO.transform.localRotation = barrelLidAttachmentRotationLeft;
                    leftHandAttachmentGO.transform.localPosition = barrelLidAttachmentVectorLeft;
                }
                else if (currentHand == rightHand)
                {
                    rightHandAttachmentGO.transform.localRotation = barrelLidAttachmentRotationRight;
                    rightHandAttachmentGO.transform.localPosition = barrelLidAttachmentVectorRight;
                }
            }
            else if (transform.gameObject.CompareTag("Greatsword"))
            {
                if (currentHand == leftHand)
                {
                    leftHandAttachmentGO.transform.localRotation = greatswordAttachmentRotationLeft;
                    leftHandAttachmentGO.transform.localPosition = greatswordAttachmentVectorLeft;
                }
                else if (currentHand == rightHand)
                {
                    rightHandAttachmentGO.transform.localRotation = greatswordAttachmentRotationRight;
                    rightHandAttachmentGO.transform.localPosition = greatswordAttachmentVectorRight;
                }
            }
            else if (transform.gameObject.CompareTag("Handle"))
            {
                if (currentHand == leftHand)
                {
                    leftHandAttachmentGO.transform.localRotation = handleAttachmentRotationLeft;
                    leftHandAttachmentGO.transform.localPosition = handleAttachmentVectorLeft;
                }
                else if (currentHand == rightHand)
                {
                    rightHandAttachmentGO.transform.localRotation = handleAttachmentRotationRight;
                    rightHandAttachmentGO.transform.localPosition = handleAttachmentVectorRight;
                }
            }
            else if (transform.gameObject.CompareTag("lockpickturner"))
            {
                if (currentHand == rightHand)
                {
                    rightHandAttachmentGO.transform.localRotation = lockpickTurnerAttachmentRotationRight;
                    rightHandAttachmentGO.transform.localPosition = lockpickTurnerAttachmentVectorRight;
                }
                else if (currentHand == leftHand)
                {
                    leftHandAttachmentGO.transform.localRotation = lockpickTurnerAttachmentRotationLeft;
                    leftHandAttachmentGO.transform.localPosition = lockpickTurnerAttachmentVectorLeft;
                }
            }
            else if (transform.gameObject.CompareTag("lockpickblocker"))
            {
                if (currentHand == rightHand)
                {
                    rightHandAttachmentGO.transform.localRotation = lockpickBlockerAttachmentRotationRight;
                    rightHandAttachmentGO.transform.localPosition = lockpickBlockerAttachmentVectorRight;
                }
                else if (currentHand == leftHand)
                {
                    leftHandAttachmentGO.transform.localRotation = lockpickBlockerAttachmentRotationLeft;
                    leftHandAttachmentGO.transform.localPosition = lockpickBlockerAttachmentVectorLeft;
                }
            }
            else if (transform.gameObject.CompareTag("dagger"))
            {
                if (currentHand == rightHand)
                {
                    rightHandAttachmentGO.transform.localRotation = daggerAttachmentRotationRight;
                    rightHandAttachmentGO.transform.localPosition = daggerAttachmentVectorRight;
                }
                else if (currentHand == leftHand)
                {
                    leftHandAttachmentGO.transform.localRotation = daggerAttachmentRotationLeft;
                    leftHandAttachmentGO.transform.localPosition = daggerAttachmentVectorLeft;
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (equipped && myCurrentHand != null) {
                if (!ThisOrAnyParentContainsTag("Player", collision.gameObject) && !pulseDelaying && !collision.gameObject.name.ToLower().Contains("torus")) {
                    float amplitude = collision.relativeVelocity.magnitude * 0.11f;
                    if (amplitude > 0.95f) amplitude = 0.95f;
                    hapticSig.Execute(0f, 0.1f, 200, amplitude, myCurrentHand.GetComponent<SteamVR_Behaviour_Pose>().inputSource);
                    StartCoroutine(PulseDelay(1f));
                }
            }
        }

        private IEnumerator ReattachItem(Hand currentHand) {

            currentHand.DetachObject(gameObject, restoreOriginalParent);

            if (defaultItemToNonKinematic)
            {
                rigidbody.isKinematic = false;
            }
            Debug.Log("Reattaching item");
            yield return new WaitForSeconds(0.1f);
            currentHand.AttachObject(gameObject, GrabTypes.Grip, attachmentFlags, attachmentOffset);
            currentHand.HideGrabHint();
        }

        private IEnumerator PulseDelay(float delayTime) {
            pulseDelaying = true;
            yield return new WaitForSecondsRealtime(delayTime);
            pulseDelaying = false;
        }

        private bool ThisOrAnyParentContainsTag(string tagToFind, GameObject startingObject)
        {
            if (startingObject.CompareTag(tagToFind))
            {
                return true;
            }
            Transform parent = startingObject.transform.parent;
            while (parent != null)
            {
                if (parent.CompareTag(tagToFind))
                {
                    return true;
                }
                parent = parent.transform.parent;
            }
            return false;
        }

        public CharacterSheet GetPlayerSheet() {
            return playerSheet;
        }

        public bool IsEquipped() {
            return equipped;
        }
    }

    public enum ReleaseStyle
    {
        NoChange,
        GetFromHand,
        ShortEstimation,
        AdvancedEstimation,
    }
}
