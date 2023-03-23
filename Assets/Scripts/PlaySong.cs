using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaySong : MonoBehaviour
{
    private AudioSource source;
    public TMP_Text text;
    private MeshRenderer meshRenderer;
    public Material whiteMaterial;
    public Material yellowMaterial;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LStickHead")
        {
            HapticFeedbackManager.singleton.TriggerVibration(40, 2, ((int)other.gameObject.GetComponent<TrackSpeed>().speed) * 100, OVRInput.Controller.LTouch);
            if (!source.isPlaying)
            {
                source.Play();
                text.text = "Stop";
                meshRenderer.material = yellowMaterial;
            }
            else
            {
                source.Stop();
                text.text = "Play";
                meshRenderer.material = whiteMaterial;

            }
        }
        else if (other.tag == "RStickHead")
        {
            HapticFeedbackManager.singleton.TriggerVibration(40, 2, ((int)other.gameObject.GetComponent<TrackSpeed>().speed) * 100, OVRInput.Controller.RTouch);
            if (!source.isPlaying)
            {
                source.Play();
                text.text = "Stop";
                meshRenderer.material = yellowMaterial;
            }
            else
            {
                source.Stop();
                text.text = "Play";
                meshRenderer.material = whiteMaterial;

            }
        }

        
    }
}
