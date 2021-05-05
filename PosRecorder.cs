using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosRecorder : MonoBehaviour
{
    public DashController dController;
    private GameObject[] playAreas;
    public GameObject player;
    private GameObject playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.Find("VRCamera");  //get the VRcamera object
        playAreas = GameObject.FindGameObjectsWithTag("Dashable");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (dController.posRecordDelay == false)
        {
            //Debug.Log("Recording position " + transform.position.ToString());

            dController.posRecordDelay = true;
            StartCoroutine(RecordPosition());
        }
    }

    

    public IEnumerator RecordPosition()
    {
        dController.posRecordDelay = true;

        GameObject closestArea = FindClosestPlayArea(transform.position);
        Vector3 newPos = new Vector3(ClosestValidPointInPlayArea(closestArea, transform.position).x, closestArea.transform.position.y - 0.1f, ClosestValidPointInPlayArea(closestArea, transform.position).z);

       
        Vector3 globalCameraPosition = playerCamera.transform.position;  //get the global position of VRcamera
        Vector3 globalPlayerPosition = player.transform.position;
        Vector3 globalOffsetCameraPlayer = new Vector3(globalCameraPosition.x - globalPlayerPosition.x, 0, globalCameraPosition.z - globalPlayerPosition.z);
        Vector3 newRigPosition = new Vector3(newPos.x - globalOffsetCameraPlayer.x, player.transform.position.y, newPos.z - globalOffsetCameraPlayer.z);
        dController.recordPos = newPos;


       
        yield return new WaitForSecondsRealtime(1);
        dController.posRecordDelay = false;
    }

    private GameObject FindClosestPlayArea(Vector3 point)
    {
        float closestDistance = 5000f;
        GameObject closestPlayArea = null;
        foreach (GameObject playArea in playAreas)
        {
            if (playArea.activeInHierarchy)
            {
                closestDistance = Vector3.Distance(point, playArea.GetComponent<Collider>().ClosestPointOnBounds(point));
                closestPlayArea = playArea;
                break;
            }
        }

        foreach (GameObject playArea in playAreas)
        {
            if (playArea.activeInHierarchy)
            {
                if (Vector3.Distance(point, playArea.GetComponent<Collider>().ClosestPointOnBounds(point)) < closestDistance)
                {
                    closestDistance = Vector3.Distance(point, playArea.GetComponent<Collider>().ClosestPointOnBounds(point));
                    closestPlayArea = playArea;
                }
            }
        }
        return closestPlayArea;
    }

    private Vector3 ClosestValidPointInPlayArea(GameObject playArea, Vector3 point)
    {
        Vector3 closestPointInPlayArea = playArea.GetComponent<Collider>().ClosestPointOnBounds(point);
        return closestPointInPlayArea;
    }
}
