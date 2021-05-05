using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class FadeTest : MonoBehaviour
{
    public SphereCollider myCol;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT VR");

        SteamVR_Fade.Start(Color.black, 0);
        
    }

    private void OnTriggerStay(Collider other)
    {
        SteamVR_Fade.Start(Color.black, 0);
        
    }

    private void OnTriggerExit(Collider other)
    {
        
        SteamVR_Fade.Start(Color.clear, 1.2f);
    }
}
