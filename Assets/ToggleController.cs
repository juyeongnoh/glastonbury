using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleController : MonoBehaviour
{
    private bool isStick;
    public GameObject Lcontroller;
    public GameObject LStick;
    public GameObject Rcontroller;
    public GameObject RStick;

    // Start is called before the first frame update
    void Start()
    {
        isStick = true;
        //controller = GameObject.Find("OVRControllerPrefab");
        //LStick = GameObject.Find("LStick");

        LStick.SetActive(true);
        RStick.SetActive(true);
        Lcontroller.SetActive(false);
        Rcontroller.SetActive(false);
        // set controller to stick
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Start))
        {
            // toggle
            isStick = !isStick;
            LStick.SetActive(isStick);
            RStick.SetActive(isStick);
            Lcontroller.SetActive(!isStick);
            Rcontroller.SetActive(!isStick);
        }
    }
}
