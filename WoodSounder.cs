using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSounder : MonoBehaviour
{
    public AudioClip[] thudSounds;
    private AudioSource aSource;
    private bool wait;
    // Start is called before the first frame update
    void Start()
    {
        wait = false;
        aSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject.CompareTag("Handle") && col.relativeVelocity.magnitude >= 0.5f && !wait)
        {
            aSource.volume = 1 * (col.relativeVelocity.magnitude * 0.05f);
            int randomClip = Random.Range(0, thudSounds.Length);
            aSource.pitch = 1 + (Random.Range(-0.350000f, 0.3500000f));
            aSource.PlayOneShot(thudSounds[randomClip]);
            StartCoroutine(SoundDelay());
        }
    }

    private IEnumerator SoundDelay() {
        wait = true;
        yield return new WaitForSecondsRealtime(0.2f);
        wait = false;
    }
}
