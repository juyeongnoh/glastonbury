using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System;
using UnityEngine.Profiling;

public class AudioRecorder : MonoBehaviour
{
    // audioSource[0]  ==  kick
    // audioSource[1]  ==  snare
    // audioSource[2]  ==  tom1
    // audioSource[3]  ==  tom2
    // audioSource[4]  ==  tom3
    // audioSource[5]  ==  hihat_closed
    // audioSource[6]  ==  hihat_opened
    // audioSource[7]  ==  crash1
    // audioSource[8]  ==  crash2
    // audioSource[9]  ==  ride_border
    // audioSource[10] ==  ride_center

    private AudioSource[] drumSources = new AudioSource[11];
    private float[][] drumSamples = new float[11][];

    // 현재 버튼의 MeshRenderer
    private MeshRenderer meshRenderer;

    public Material whiteMaterial;
    public Material redMaterial;

    string path;

    void Start()
    {
        drumSources[0] = GameObject.Find("Kick").GetComponent<AudioSource>();
        drumSources[1] = GameObject.Find("Snare").GetComponent<AudioSource>();
        drumSources[2] = GameObject.Find("Tom1").GetComponent<AudioSource>();
        drumSources[3] = GameObject.Find("Tom2").GetComponent<AudioSource>();
        drumSources[4] = GameObject.Find("Tom3").GetComponent<AudioSource>();
        drumSources[5] = GameObject.Find("HiHatClosed").GetComponent<AudioSource>();
        drumSources[6] = GameObject.Find("HiHatOpened").GetComponent<AudioSource>();
        drumSources[7] = GameObject.Find("Crash1").GetComponent<AudioSource>();
        drumSources[8] = GameObject.Find("Crash2").GetComponent<AudioSource>();
        drumSources[9] = GameObject.Find("RideBorder").GetComponent<AudioSource>();
        drumSources[10] = GameObject.Find("RideCenter").GetComponent<AudioSource>();

        meshRenderer = GetComponent<MeshRenderer>();

        path = Application.persistentDataPath + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".wav";
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LStickHead")
        {
            HapticFeedbackManager.singleton.TriggerVibration(40, 2, ((int)other.gameObject.GetComponent<TrackSpeed>().speed) * 100, OVRInput.Controller.LTouch);
            if (!AudioFilter.isRecording)
            {
                meshRenderer.material = redMaterial;
                RecordStart();
            }
            else
            {
                meshRenderer.material = whiteMaterial;
                RecordStop();
            }
        }
        else if (other.tag == "RStickHead")
        {
            HapticFeedbackManager.singleton.TriggerVibration(40, 2, ((int)other.gameObject.GetComponent<TrackSpeed>().speed) * 100, OVRInput.Controller.RTouch);

            if (!AudioFilter.isRecording)
            {
                meshRenderer.material = redMaterial;
                RecordStart();
            }
            else
            {
                meshRenderer.material = whiteMaterial;
                RecordStop();
            }
        }

    }

    void RecordStart()
    {
        for(int i = 0; i < 11; i++)
            drumSources[i].GetComponent<AudioFilter>().ClearSamples();

        AudioFilter.isRecording = true;
    }

    void RecordStop()
    {
        int sampleRate = 44100;

        AudioFilter.isRecording = false;

        for(int i = 0; i < 11; i++)
            drumSamples[i] = drumSources[i].GetComponent<AudioFilter>().GetSamples();

        int maxSamplesLength = new int[] {
            drumSamples[0].Length,
            drumSamples[1].Length,
            drumSamples[2].Length,
            drumSamples[3].Length,
            drumSamples[4].Length,
            drumSamples[5].Length,
            drumSamples[6].Length,
            drumSamples[7].Length,
            drumSamples[8].Length,
            drumSamples[9].Length,
            drumSamples[10].Length}.Max();

        for(int i = 0; i < 11; i++)
            Array.Resize(ref drumSamples[i], maxSamplesLength);


        float[] mixSamples = new float[maxSamplesLength];

        for (int i = 0; i < mixSamples.Length; i++)
        {
            mixSamples[i] =
                drumSamples[0][i] +
                drumSamples[1][i] +
                drumSamples[2][i] +
                drumSamples[3][i] +
                drumSamples[4][i] +
                drumSamples[5][i] +
                drumSamples[6][i] +
                drumSamples[7][i] +
                drumSamples[8][i] +
                drumSamples[9][i] +
                drumSamples[10][i];
        }

        saveFile(path, mixSamples, sampleRate);
    }

    void saveFile(String path, float[] samples, int sampleRate)
    {
        int headerSize = 44;
        byte[] header = new byte[headerSize];

        // RIFF header
        header[0] = (byte)'R';
        header[1] = (byte)'I';
        header[2] = (byte)'F';
        header[3] = (byte)'F';
        header[4] = (byte)(samples.Length * 2 + headerSize - 8);
        header[5] = (byte)((samples.Length * 2 + headerSize - 8) >> 8);
        header[6] = (byte)((samples.Length * 2 + headerSize - 8) >> 16);
        header[7] = (byte)((samples.Length * 2 + headerSize - 8) >> 24);
        header[8] = (byte)'W';
        header[9] = (byte)'A';
        header[10] = (byte)'V';
        header[11] = (byte)'E';

        // fmt header
        header[12] = (byte)'f';
        header[13] = (byte)'m';
        header[14] = (byte)'t';
        header[15] = (byte)' ';
        header[16] = 16; // fmt chunk size
        header[17] = 0;
        header[18] = 0;
        header[19] = 0;
        header[20] = 1; // compression code (1 = PCM)
        header[21] = 0;
        header[22] = 2; // number of channels
        header[23] = 0;
        header[24] = (byte)(sampleRate & 0xff);
        header[25] = (byte)((sampleRate >> 8) & 0xff);
        header[26] = (byte)((sampleRate >> 16) & 0xff);
        header[27] = (byte)((sampleRate >> 24) & 0xff);
        header[28] = (byte)((sampleRate * 4) & 0xff); // byte rate
        header[29] = (byte)(((sampleRate * 4) >> 8) & 0xff);
        header[30] = (byte)(((sampleRate * 4) >> 16) & 0xff);
        header[31] = (byte)(((sampleRate * 4) >> 24) & 0xff);
        header[32] = 4; // block align
        header[33] = 0;
        header[34] = 16; // bits per sample
        header[35] = 0;

        // data header
        header[36] = (byte)'d';
        header[37] = (byte)'a';
        header[38] = (byte)'t';
        header[39] = (byte)'a';
        header[40] = (byte)(samples.Length * 2);
        header[41] = (byte)((samples.Length * 2) >> 8);
        header[42] = (byte)((samples.Length * 2) >> 16);
        header[43] = (byte)((samples.Length * 2) >> 24);

        // save header and samples to file
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            fs.Write(header, 0, header.Length);
            for (int i = 0; i < samples.Length; i++)
            {
                short sample = (short)(samples[i] * 32767);
                fs.WriteByte((byte)(sample & 0xff));
                fs.WriteByte((byte)((sample >> 8) & 0xff));
            }
        }
    }
}