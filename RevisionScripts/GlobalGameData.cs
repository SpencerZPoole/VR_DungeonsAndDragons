using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameData : MonoBehaviour
{
    private bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // GET METHODS
    public bool IsGamePaused() { return isPaused; }
    // SET METHODS
    public void SetIsPaused(bool paused) {
        isPaused = paused;
    }
    public void ToggleIsPaused() {
        isPaused = !isPaused;
    }


}
