using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFilter : MonoBehaviour
{
    public float[] samples;
    public static bool isRecording = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        Debug.Log("OnAudioFilterRead");

        if (isRecording)
        {
            float[] tmp = new float[data.Length + samples.Length];
            samples.CopyTo(tmp, 0);
            data.CopyTo(tmp, samples.Length);
            samples = tmp;
        }
    }

    public float[] GetSamples()
    {
        return samples;
    }

    public void ClearSamples()
    {
        samples = new float[0];
    }
}