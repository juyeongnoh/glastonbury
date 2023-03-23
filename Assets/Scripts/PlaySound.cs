using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private AudioSource source;
    private Animation animation;

    public ParticleSystem particleSystem;
    public ParticleSystem specialEffect1;
    public ParticleSystem specialEffect2;
    public ParticleSystem specialEffect3;
    public ParticleSystem specialEffect4;
    public ParticleSystem specialEffect5;
    public ParticleSystem specialEffect6;
    public ParticleSystem specialEffect7;
    public GameObject drum;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        animation = drum.GetComponent<Animation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        animation.Play();
        if (other.tag == "LStickHead")
        {
            HapticFeedbackManager.singleton.TriggerVibration(40, 2, ((int)other.gameObject.GetComponent<TrackSpeed>().speed) * 100, OVRInput.Controller.LTouch);
            source.pitch = Random.Range(0.98f, 1.02f);
            source.volume = other.gameObject.GetComponent<TrackSpeed>().speed;
            source.Play();
            animation.Play();
            particleSystem.Play();
            specialEffect1.Play();
            specialEffect2.Play();
            specialEffect3.Play();
            specialEffect4.Play();
            specialEffect5.Play();
            specialEffect6.Play();
            specialEffect7.Play();
            
        }
        else if (other.tag == "RStickHead")
        {
            HapticFeedbackManager.singleton.TriggerVibration(40, 2, ((int)other.gameObject.GetComponent<TrackSpeed>().speed) * 100, OVRInput.Controller.RTouch);
            source.pitch = Random.Range(0.98f, 1.02f);
            source.volume = other.gameObject.GetComponent<TrackSpeed>().speed;
            source.Play();
            animation.Play();
            particleSystem.Play();
            specialEffect1.Play();
            specialEffect2.Play();
            specialEffect3.Play();
            specialEffect4.Play();
            specialEffect5.Play();
            specialEffect6.Play();
            specialEffect7.Play();
            
        }
    }
}
