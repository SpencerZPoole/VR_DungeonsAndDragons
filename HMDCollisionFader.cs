using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HMDCollisionFader : MonoBehaviour
{
    private SphereCollider sCol;
    public DashController dController;

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
        if (ThisOrAnyParentContainsTag("Player", other.gameObject) == false && other.transform.CompareTag("Dashable") == false && other.transform.name != "VRCamera" && other.transform.CompareTag("enemy") == false && other.transform.name.ToLower().Contains("chest") == false && other.transform.name.ToLower().Contains("barrel") == false) {
           // Debug.Log("HMD Trigger Enter " + other.transform.name);
            SteamVR_Fade.Start(Color.black, 0);
            if (other.transform.CompareTag("Wall") || other.transform.CompareTag("Gate")) {
                dController.ResetPlayerPosition();
            }
        } 
    }

    private void OnTriggerStay(Collider other)
    {
        if (ThisOrAnyParentContainsTag("Player", other.gameObject) == false && other.transform.CompareTag("Dashable") == false && other.transform.name != "VRCamera" && other.transform.CompareTag("enemy") == false && other.transform.name.ToLower().Contains("chest") == false && other.transform.name.ToLower().Contains("barrel") == false)
        {
           // Debug.Log("HMD Trigger Stay " + other.transform.name);
            SteamVR_Fade.Start(Color.black, 0);
            if (other.transform.CompareTag("Wall") || other.transform.CompareTag("Gate"))
            {
                dController.ResetPlayerPosition();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SteamVR_Fade.Start(Color.clear, 1.2f);
        //Debug.Log("HMD Trigger Exit");
    }

    private bool ThisOrAnyParentContainsTag(string tagToFind, GameObject startingObject)
    {
        if (startingObject.CompareTag(tagToFind)) {
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
}
