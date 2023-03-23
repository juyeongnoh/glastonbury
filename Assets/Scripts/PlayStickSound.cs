using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStickSound : MonoBehaviour
{
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "RStickBody")
        {
            HapticFeedbackManager.singleton.TriggerVibration(40, 2, ((int)other.gameObject.GetComponent<TrackSpeed>().speed) * 100, OVRInput.Controller.LTouch);
            HapticFeedbackManager.singleton.TriggerVibration(40, 2, ((int)other.gameObject.GetComponent<TrackSpeed>().speed) * 100, OVRInput.Controller.RTouch);
        }
        source.volume = other.gameObject.GetComponent<TrackSpeed>().speed;
        source.Play();
    }
}
