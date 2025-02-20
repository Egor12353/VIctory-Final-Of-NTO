using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SmenaScene : MonoBehaviour
{
    [SerializeField]
    private Rigidbody trigger;
    [SerializeField]
    private Rigidbody TV;
    [SerializeField]
    private string scene;
    private void OnTriggerEnter(Collider other)
    {
        print("Smena");
        if (other.CompareTag("Casseta"))
            
            LoadSceneByName(scene);
    }
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
