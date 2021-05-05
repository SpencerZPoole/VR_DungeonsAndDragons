using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem {
    [RequireComponent(typeof(Interactable))]
    [RequireComponent(typeof(Rigidbody))]
    public class BarrelLidInfo : MonoBehaviour
    {
        public GameObject myBarrel;
        public AudioClip lidThudSound;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected virtual void OnAttachedToHand(Hand hand) {
            if (myBarrel != null) {
                myBarrel.GetComponent<AudioSource>().PlayOneShot(lidThudSound);
                myBarrel = null;
            }
        }

    }
}


