using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSounds : MonoBehaviour
{
    public AudioClip[] swingSounds;
    public AudioClip[] hitSounds;
    public AudioClip[] paddedImpactSounds;
    public AudioClip grindSound;
    private Rigidbody rBody;
    public AudioSource swingSource;
    public AudioSource hitSource;
    private bool wait;
    public bool isDagger;
    public float daggerPitchAdjust;

    // Start is called before the first frame update
    void Start()
    {
        wait = false;
        rBody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rBody.velocity.magnitude >= 6.5f && !swingSource.isPlaying) {
            int randomClip = Random.Range(0, swingSounds.Length);
            if (isDagger) swingSource.pitch = daggerPitchAdjust + ((rBody.velocity.magnitude / 100) * 3);
            else swingSource.pitch = 1 + ((rBody.velocity.magnitude / 100)*3);
            swingSource.PlayOneShot(swingSounds[randomClip]);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (!wait && !col.gameObject.name.ToLower().Contains("chest") && !col.gameObject.name.ToLower().Contains("barrel") && !col.gameObject.name.ToLower().Contains("hand"))
        {
            hitSource.volume = col.relativeVelocity.magnitude * 0.018f;
            if (col.relativeVelocity.magnitude >= 1.5f)
            {
                hitSource.Stop();
                if (isDagger) { hitSource.pitch = daggerPitchAdjust + (Random.Range(-0.250000f, 0.3500000f)); }
                else hitSource.pitch = 1 + (Random.Range(-0.250000f, 0.3500000f));
                if (col.gameObject.name.ToLower().Contains("dummy") || col.gameObject.CompareTag("enemy")) hitSource.PlayOneShot(paddedImpactSounds[Random.Range(0, paddedImpactSounds.Length)]);
                else hitSource.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);
                StartCoroutine(SoundDelay());
            }
            
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }

    private IEnumerator SoundDelay()
    {
        wait = true;
        yield return new WaitForSecondsRealtime(0.2f);
        wait = false;
    }


}
