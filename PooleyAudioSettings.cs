using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PooleyAudioSettings : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<AudioSource>().time = Random.Range(0.0f, this.GetComponent<AudioSource>().clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
