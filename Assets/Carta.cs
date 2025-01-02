using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class Carta : MonoBehaviour
{
    
    public bool scoperta;
    AudioSource audioSource;
    public AudioClip suonoCarta;
    
    internal void GiraCarta()
    {
       
        scoperta = !scoperta;
        transform.Rotate(new Vector3(0, 180, 0));
        audioSource.PlayOneShot(suonoCarta, 1);
    }
    // Start is called before the first frame update
    void Start()
    {
       audioSource= GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
