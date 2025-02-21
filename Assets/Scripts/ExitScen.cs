using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ExitScen : MonoBehaviour
{

    [SerializeField]
    private SteamVR_Action_Boolean exitScen;

    [SerializeField]
    private string scene = "Load Scene";
    private void Update()
    {
        if (exitScen.GetState(SteamVR_Input_Sources.RightHand))
        {
            LoadSceneByName(scene);
        }
    }

    
    
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


}
