using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class ItemHolderSpot : MonoBehaviour
{
    public GameObject currentHeldObject;
    public GameObject torchPrefab;
    public Transform spotToAttach;
    public SteamVR_Action_Boolean grip = SteamVR_Input.GetBooleanAction("GrabGrip");
    public bool requiresSpecificTag;
    public bool createNewTorchOnStartup;
    public string specificTag;
    public GameObject activeLidCollider;
    private AudioSource aSource;
    public AudioClip lidThudSound;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<AudioSource>() != null) {
            aSource = gameObject.GetComponent<AudioSource>();
        }
        if (createNewTorchOnStartup && currentHeldObject == null)
        {
            GameObject newTorch = GameObject.Instantiate(torchPrefab);
            currentHeldObject = newTorch;
        }
        if (currentHeldObject != null) {
            currentHeldObject.GetComponent<PooleyInteractable>().currentItemHolderSpot = this.gameObject;
            currentHeldObject.GetComponent<Rigidbody>().isKinematic = true;
            currentHeldObject.GetComponent<Rigidbody>().detectCollisions = false;
            currentHeldObject.transform.position = spotToAttach.position;
            currentHeldObject.transform.rotation = spotToAttach.rotation;
            if (activeLidCollider != null)
            {
                activeLidCollider.GetComponent<Collider>().enabled = true;
            }
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentHeldObject != null) {
            if (currentHeldObject.CompareTag("BarrelLid")){
                currentHeldObject.transform.position = spotToAttach.transform.position;
                currentHeldObject.transform.rotation = spotToAttach.transform.rotation;
            }
        }
    }

    protected virtual void HandHoverUpdate(Hand hand)
    {
        if (GripFromEitherHand(hand))
        {
            if (hand.currentAttachedObject != null && currentHeldObject == null && (!requiresSpecificTag || (requiresSpecificTag && hand.currentAttachedObject.CompareTag(specificTag))))
            {
                GameObject targetedObject = hand.currentAttachedObject;
                hand.DetachObject(targetedObject);
                targetedObject.GetComponent<Rigidbody>().isKinematic = true;
                targetedObject.GetComponent<Rigidbody>().detectCollisions = false;
                currentHeldObject = targetedObject;
                targetedObject.transform.position = spotToAttach.position;
                targetedObject.transform.rotation = spotToAttach.rotation;
                if (activeLidCollider != null) {
                    activeLidCollider.GetComponent<Collider>().enabled = true;
                }
                if (currentHeldObject.CompareTag("BarrelLid")) {
                    aSource.PlayOneShot(lidThudSound);
                    currentHeldObject.GetComponent<BarrelLidInfo>().myBarrel = gameObject;
                }
            }
            else if (hand.currentAttachedObject == null && currentHeldObject != null) {
                PooleyInteractable pInteractable = currentHeldObject.GetComponent<PooleyInteractable>();
                pInteractable.currentItemHolderSpot = null;
                currentHeldObject.GetComponent<Rigidbody>().isKinematic = false;
                currentHeldObject.GetComponent<Rigidbody>().detectCollisions = true;
                hand.AttachObject(currentHeldObject, GrabTypes.Grip, pInteractable.attachmentFlags, pInteractable.attachmentOffset);
                currentHeldObject = null;
                hand.HideGrabHint();
                if (activeLidCollider != null)
                {
                    activeLidCollider.GetComponent<Collider>().enabled = false;
                }
            }  
        }
    }
    protected virtual void OnHandHoverBegin(Hand hand)
    {
        if (hand.currentAttachedObject != null)
        {
            hand.currentAttachedObject.GetComponent<PooleyInteractable>().currentItemHolderSpot = gameObject;
        }
    }
    protected virtual void OnHandHoverEnd(Hand hand)
    {
        if (hand.currentAttachedObject != null)
        {
            hand.currentAttachedObject.GetComponent<PooleyInteractable>().currentItemHolderSpot = null;
        }
    }

    protected bool GripFromEitherHand(Hand hand) {
        if (grip != null && grip.GetStateDown(hand.GetComponent<SteamVR_Behaviour_Pose>().inputSource)) {
            return true;
        }
        else return false;
    }
}
