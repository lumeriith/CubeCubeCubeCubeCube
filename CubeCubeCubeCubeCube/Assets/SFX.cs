using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour {
    public void Play(string name)
    {
        transform.Find(name).GetComponent<AudioSource>().Play();
    }

    public void Stop(string name)
    {
        transform.Find(name).GetComponent<AudioSource>().Stop();
    }
    public void PlayDelayed(string name, float delay)
    {
        transform.Find(name).GetComponent<AudioSource>().PlayDelayed(delay);
    }


}
