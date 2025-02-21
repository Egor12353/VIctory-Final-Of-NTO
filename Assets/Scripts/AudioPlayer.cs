using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource audio2;
    


    private void Update()
    {
        
            audio2.Play();
        
        
    }
}
