using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Unity.VisualScripting.Member;

public class Metronome : MonoBehaviour
{
    public bool isPlaying = false;
    public int cnt = 0;
    public int beatPerBar = 3;
    public int beatUnit = 4;
    public int bpm = 120;
    public TMP_Text text;
    private AudioSource metronome;

    void Start()
    {
        try
        {
            metronome = GetComponent<AudioSource>();
            Debug.Log("Metronome File Loaded");
        }
        catch (System.Exception e)
        {
            Debug.Log("error loading metronome file");
        }
        text.text = "Current BPM: " + Convert.ToString(bpm);
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            Debug.Log("Metronome Start Button Clicked");
            if (!metronome.isPlaying && !isPlaying)
            {
                PlayMetronome();
                isPlaying = true;
                
            }
            else
            {
                StopMetronome();
                isPlaying = false;
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            bpm += 5;
            text.text = "Current BPM: " + Convert.ToString(bpm) + " (" + Convert.ToString(beatPerBar) + "/" + Convert.ToString(beatUnit) + ")";
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            bpm -= 5;
            text.text = "Current BPM: " + Convert.ToString(bpm) + " (" + Convert.ToString(beatPerBar) + "/" + Convert.ToString(beatUnit) + ")";

        }
    }

    void PlayMetronome()
    {
        if (bpm < 1)
        {
            bpm = 120;
        }
        else if (bpm > 300)
        {
            bpm = 300;
        }

        Debug.Log("bpm: " + bpm);
        float beatTime = 60f / bpm;
        float repeatTime = (beatTime * 4) / beatUnit;

        InvokeRepeating("PlayBeat", 0, repeatTime);
    }

    void PlayBeat()
    {
        if (cnt == 0)
        {
            metronome.pitch = 1.5f;
            metronome.Play();
        }
        else
        {
            metronome.pitch = 1.0f;
            metronome.Play();
        }

        cnt = (cnt + 1) % beatPerBar;
    }

    void StopMetronome()
    {
        CancelInvoke("PlayBeat");
        cnt = 0;
    }
}