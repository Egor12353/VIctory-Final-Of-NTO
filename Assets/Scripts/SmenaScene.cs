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
    
    private string scene = "First Game";

    private string scene2 = "3 GAME";
    private void OnTriggerEnter(Collider other)
    {
        print("Smena");
        if (other.CompareTag("Casseta"))
        {

            LoadSceneByName(scene);
        }
        if (other.CompareTag("Casseta2"))
        {
            LoadSceneByName(scene2);
        }
        if (other.CompareTag("Casseta3"))
        {
            Application.Quit();
        }
        if (other.CompareTag("Casseta4"))
        {
            Application.Quit();
        }
    }
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
