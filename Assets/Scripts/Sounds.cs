using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{

    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        Player.ballTouch += Son;
    }

    private void OnDisable()
    {
        Player.ballTouch -= Son;
    }
    private void Son()
    {
        source.Play();
    }
}