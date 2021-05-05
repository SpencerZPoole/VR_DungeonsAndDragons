using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateAudio : MonoBehaviour
{
    private Rigidbody rBody;
    public AudioSource aSource;
    private bool paused;
    public AudioClip hingeSound;
    private bool isChest;
    private HingeJoint hJoint;
    private float lastHingeAngle;
    private bool runningDelay;
    public bool useHingeSound;
    private float initialVolume;
    
    // Start is called before the first frame update
    void Start()
    {
        useHingeSound = false;
        runningDelay = false;
        hJoint = gameObject.GetComponent<HingeJoint>();
        lastHingeAngle = hJoint.angle;
        if (gameObject.name.ToLower().Contains("chest"))
        {
            isChest = true;
        }
        else {
            isChest = false;
        }
        paused = false;
        rBody = gameObject.GetComponent<Rigidbody>();
        initialVolume = aSource.volume;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isChest)
        {
            if (Mathf.Abs(hJoint.angle - lastHingeAngle) >= 0.05f)
            {
                if (paused)
                {
                    aSource.UnPause();
                    paused = false;
                }
                else if (!aSource.isPlaying)
                {
                    aSource.PlayOneShot(hingeSound);
                    aSource.pitch = 1 + (Random.Range(-0.8000f, -0.60000f));
                }
            }
            else if (!runningDelay)
            {
                StartCoroutine(SoundOffDelay());
            }
            lastHingeAngle = hJoint.angle;
        }
        else if(useHingeSound) {
            if (Mathf.Abs(hJoint.angle - lastHingeAngle) >= 0.125f)
            {
                if (paused)
                {
                    aSource.UnPause();
                    paused = false;
                }
                else if (!aSource.isPlaying)
                {
                    aSource.PlayOneShot(hingeSound);
                    aSource.pitch = 1 + (Random.Range(-0.05000f, 0.01000f));
                }
            }
            else if (!runningDelay)
            {
                StartCoroutine(SoundOffDelay());
            }
            lastHingeAngle = hJoint.angle;
        }
    }

    private IEnumerator SoundOffDelay() {
        runningDelay = true;
        float startVolume = initialVolume;
        float fadeTime = 0.2f;
        if (aSource.isPlaying)
        {
            while (aSource.volume > 0)
            {
                aSource.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }
        }
        if (aSource.isPlaying)
        {
            aSource.Pause();
            paused = true;
        }
        aSource.volume = initialVolume;
        runningDelay = false;

    }
}
