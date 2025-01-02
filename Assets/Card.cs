using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class Card : MonoBehaviour
{

    public bool uncoverd;
    AudioSource audioSource;
    public AudioClip cardSound;

    internal void TurnCard()
    {

        uncoverd = !uncoverd;
        transform.Rotate(new Vector3(0, 180, 0));
        audioSource.PlayOneShot(cardSound, 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
