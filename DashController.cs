using System.Collections;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;
public class DashController : MonoBehaviour
{
    public SteamVR_Behaviour_Pose pose;

    //public SteamVR_Action_Boolean interactWithUI = SteamVR_Input.__actions_default_in_InteractUI;
    private SteamVR_Action_Boolean dash = SteamVR_Input.GetBooleanAction("DpadUpLeft");
    private SteamVR_Action_Boolean touchingDash = SteamVR_Input.GetBooleanAction("TouchingDpadUpL");

    [SerializeField]
    private float minDashRange = 0.5f;
    [SerializeField]
    private float maxDashRange = 40f;
    [SerializeField]
    private float _timeStartedLerping;
    [SerializeField]
    private Animator maskAnimator;
    public float speed;
    public GameObject dashCircle;
    private Vector3 dashPoint;
    private Vector3 lastValidCirclePos;
    private Vector3 nullVector;
    public Material blueCircleMaterial;
    public Material redCircleMaterial;
    public Transform cameraRigRoot;
    public SteamVR_LaserPointer laserPointerScript;
    private bool dashing;
    public CharacterSheet myCharacterSheet;
    private float sizeSpeedModifier;
    private bool dashOkay;
    private GameObject[] playAreas;
    public bool useDebugLaserPointer;
    public Player player;
    public bool posRecordDelay;
    public Vector3 recordPos;
    public Transform bodyCollider;
    public bool isPaused;
    public PauseCanvasToggle guiHandler;

    private void Start()
    {
        posRecordDelay = false;
        playAreas = GameObject.FindGameObjectsWithTag("Dashable");
        dashing = false;
        dashOkay = false;
        isPaused = false;
        nullVector = new Vector3(0, 0, 0);
        lastValidCirclePos = nullVector;
        if (myCharacterSheet.GetSize().ToLower() == "medium")
        {
            sizeSpeedModifier = 0.5f;
            maxDashRange = 6;
        }
        else if (myCharacterSheet.GetSize().ToLower() == "small") {
            sizeSpeedModifier = 0.75f;
            maxDashRange = 4;
        }



    }

    private void Update()
    {
        if (touchingDash != null && touchingDash.GetState(pose.inputSource))
        {
            if (useDebugLaserPointer)
            {
                laserPointerScript.PointerSetActive(true);
            }
            
            TryDash();
        }
        else{
            if (dashCircle.activeSelf == true){
                dashCircle.SetActive(false);
            }
            if (laserPointerScript.active){
                laserPointerScript.PointerSetActive(false);
            }
            
            
        }
        
    }

    private void TryDash()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit) && !isPaused)
        {
            if (hit.distance > maxDashRange)
            {
                Vector3 edgePoint = ray.GetPoint(maxDashRange);
                Ray validateEdgeRay = new Ray(edgePoint, Vector3.down);
                if (Physics.Raycast(validateEdgeRay, out hit)) {
                    if (hit.transform.CompareTag("Dashable"))
                    {
                        TurnDashCircleBlue();
                        dashCircle.transform.position = new Vector3(hit.point.x, hit.point.y - 0.1f, hit.point.z);
                        dashPoint = dashCircle.transform.position;
                        lastValidCirclePos = dashCircle.transform.position;
                        dashOkay = true;
                    }
                    else if (hit.transform.CompareTag("Floor"))
                    {
                        TurnDashCircleBlue();
                        GameObject closestArea = FindClosestPlayArea(hit.point);
                        Vector3 newValidPos = new Vector3(ClosestValidPointInPlayArea(closestArea, hit.point).x, closestArea.transform.position.y - 0.1f, ClosestValidPointInPlayArea(closestArea, hit.point).z);
                        dashCircle.transform.position = newValidPos;
                        lastValidCirclePos = newValidPos;
                        dashOkay = true;
                    }
                    else {
                        dashCircle.SetActive(false);
                        laserPointerScript.PointerSetActive(false);
                        dashOkay = false;

                    }
                }
            }
            else if (hit.distance > minDashRange && hit.distance < maxDashRange) {

                if (hit.transform.CompareTag("Dashable"))
                {
                    TurnDashCircleBlue();
                    dashCircle.transform.position = new Vector3(hit.point.x, hit.point.y - 0.1f, hit.point.z);
                    dashPoint = dashCircle.transform.position;
                    lastValidCirclePos = dashCircle.transform.position;
                    dashOkay = true;
                }
                else if (hit.transform.CompareTag("Floor"))
                {
                    TurnDashCircleBlue();
                    GameObject closestArea = FindClosestPlayArea(hit.point);
                    Vector3 newValidPos = new Vector3(ClosestValidPointInPlayArea(closestArea, hit.point).x, closestArea.transform.position.y - 0.1f, ClosestValidPointInPlayArea(closestArea, hit.point).z);
                    dashCircle.transform.position = newValidPos;
                    lastValidCirclePos = newValidPos;
                    dashOkay = true;
                }
                else {
                    Ray validateRay = new Ray(hit.point + new Vector3(0,0.1f,0), Vector3.down);
                    if (Physics.Raycast(validateRay, out hit))
                    {
                        if (hit.transform.CompareTag("Dashable"))
                        {
                            TurnDashCircleBlue();
                            dashCircle.transform.position = new Vector3(hit.point.x, hit.point.y - 0.1f, hit.point.z);
                            dashPoint = dashCircle.transform.position;
                            lastValidCirclePos = dashCircle.transform.position;
                            dashOkay = true;
                        }
                        else if (hit.transform.CompareTag("Floor"))
                        {
                            TurnDashCircleBlue();
                            GameObject closestArea = FindClosestPlayArea(hit.point);
                            Vector3 newValidPos = new Vector3(ClosestValidPointInPlayArea(closestArea, hit.point).x, closestArea.transform.position.y - 0.1f, ClosestValidPointInPlayArea(closestArea, hit.point).z);
                            dashCircle.transform.position = newValidPos;
                            lastValidCirclePos = newValidPos;
                            dashOkay = true;
                        }
                        else {
                            dashCircle.SetActive(false);
                            laserPointerScript.PointerSetActive(false);
                            dashOkay = false;
                        }
                    }
                }
            }
            else
            {
                dashCircle.SetActive(false);
                laserPointerScript.PointerSetActive(false);
                dashOkay = false;
            }
            if (dash != null && dash.GetState(pose.inputSource) && dashing == false && dashOkay && lastValidCirclePos != nullVector) 
            {
                StartCoroutine(DoDash(lastValidCirclePos));
            }
            
        }
    }

    private void TurnDashCircleBlue()
    {
        dashCircle.SetActive(true);
        dashCircle.GetComponent<MeshRenderer>().material = blueCircleMaterial;
        laserPointerScript.color = Color.blue;
    }

    

    public void ResetPlayerPosition() {
        StopAllCoroutines();
        dashing = false;
        Vector3 moddedEndPoint = recordPos;
        Vector3 playerFeetOffset = player.trackingOriginTransform.position - player.feetPositionGuess;
        player.trackingOriginTransform.position = moddedEndPoint + playerFeetOffset;
        if (maskAnimator != null) maskAnimator.SetBool("Mask", false);
        posRecordDelay = false;
    }


    private IEnumerator DoDash(Vector3 endPoint)
    {
        if (guiHandler.inventoryOpen){
            guiHandler.CloseInventory();
        }
        if (maskAnimator != null)
            maskAnimator.SetBool("Mask", true);
        dashing = true;
        lastValidCirclePos = nullVector;
        yield return new WaitForSeconds(0.1f);
        _timeStartedLerping = Time.time;
        Vector3 startPos = player.trackingOriginTransform.position;
        Vector3 moddedEndPoint = new Vector3(endPoint.x, endPoint.y, endPoint.z);
        Vector3 playerFeetOffset = player.trackingOriginTransform.position - player.feetPositionGuess;
        while (player.trackingOriginTransform.position != moddedEndPoint + playerFeetOffset)
        {
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / (Vector3.Distance(startPos, moddedEndPoint + playerFeetOffset) * sizeSpeedModifier);
            player.trackingOriginTransform.position = Vector3.Lerp(startPos, moddedEndPoint + playerFeetOffset, percentageComplete);
            yield return null;
        }

        if (maskAnimator != null)
            maskAnimator.SetBool("Mask", false);
        dashing = false;
    }

    private GameObject FindClosestPlayArea(Vector3 point) {
        float closestDistance = 5000f;
        GameObject closestPlayArea = null;
        foreach (GameObject playArea in playAreas) {
            if (playArea.activeInHierarchy) {
                closestDistance = Vector3.Distance(point, playArea.GetComponent<Collider>().ClosestPointOnBounds(point));
                closestPlayArea = playArea;
                break;
            }
        }
        
        foreach(GameObject playArea in playAreas) {
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

    private Vector3 ClosestValidPointInPlayArea(GameObject playArea, Vector3 point) {
        Vector3 closestPointInPlayArea = playArea.GetComponent<Collider>().ClosestPointOnBounds(point);
        return closestPointInPlayArea;
    }
}