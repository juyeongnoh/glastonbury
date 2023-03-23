using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Unity.VisualScripting.Member;
using System;

public class MetronomeBeat4Button : MonoBehaviour
{
    private Metronome metronome;

    // 메트로놈 GameObject 가져오기
    public GameObject gameObject;

    // 현재 버튼이 아닌 다른 버튼 GameObject
    public GameObject anotherButton;
    private MeshRenderer anotherMeshRenderer;

    // UI에 뜨는 텍스트
    public TMP_Text text;

    // 현재 오브젝트의 MeshRenderer
    private MeshRenderer meshRenderer;

    public Material whiteMaterial;
    public Material yellowMaterial;

    // Start is called before the first frame update
    void Start()
    {
        metronome = gameObject.GetComponent<Metronome>();
        meshRenderer = GetComponent<MeshRenderer>();
        anotherMeshRenderer = anotherButton.GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LStickHead")
        {
            HapticFeedbackManager.singleton.TriggerVibration(40, 2, ((int)other.gameObject.GetComponent<TrackSpeed>().speed) * 100, OVRInput.Controller.LTouch);
            metronome.beatPerBar = 4;
            text.text = "Current BPM: " + Convert.ToString(metronome.bpm) + " (" + Convert.ToString(metronome.beatPerBar) + "/" + Convert.ToString(metronome.beatUnit) + ")";
            anotherMeshRenderer.material = whiteMaterial;
            meshRenderer.material = yellowMaterial;

        }
        else if (other.tag == "RStickHead")
        {
            HapticFeedbackManager.singleton.TriggerVibration(40, 2, ((int)other.gameObject.GetComponent<TrackSpeed>().speed) * 100, OVRInput.Controller.RTouch);
            metronome.beatPerBar = 4;
            text.text = "Current BPM: " + Convert.ToString(metronome.bpm) + " (" + Convert.ToString(metronome.beatPerBar) + "/" + Convert.ToString(metronome.beatUnit) + ")";
            anotherMeshRenderer.material = whiteMaterial;
            meshRenderer.material = yellowMaterial;
        }

    }
}
