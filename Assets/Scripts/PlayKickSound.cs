//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayKickSound : MonoBehaviour
//{
//    private AudioSource source;

//    // Start is called before the first frame update
//    void Start()
//    {
//        source = GetComponent<AudioSource>();    
//    }

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.KeypadEnter))
//        {
//            source.Play();
//        }

//        if (Input.GetKeyDown(KeyCode.Return))
//        {
//            source.Play();
//        }
//    }
//}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayKickSound : MonoBehaviour
{
    private AudioSource source;
    // private Light spotlight;
    // private GameObject spotLightsParent;
    private Light[] childLights;
    private int lightsCount = 0;
    // private bool lightEnabled = false;
    //이유는 모르겠으나 검정, 흰색 조명은 티가 잘 나지 않음 (랜덤으로 black, White가 선택되었을 때 조명이 꺼진 것으로 인식 기대)
    private Color[] colors = { Color.yellow, Color.gray, Color.magenta, Color.cyan, Color.red, Color.black, Color.blue, Color.green };
    private Queue<int> lightsOffOrder = new Queue<int>();
    //모든 조명이 드럼 세트를 비출 때의 Rotation 값
    private double[,] childLightsDefaultRotationAngle = new double[7, 3] { { 46.345, 213.832, 991.177 }, { 46.345, 204.652, 991.177 }, { 46.345, 194.359, 991.177 }, { 46.345, 180.498, 991.177 }, { 46.345, 168.827, 991.177 }, { 46.345, 158.508, 991.177 }, { 46.345, 140.823, 991.177 } };
    // private float[,] childLightsDefaultRotationAngle = new float[7, 3] {{46.345, 213.832, 991.177}, {46.345, 204.652, 991.177}, {46.345, 194.359, 991.177}, {46.345, 180.498, 991.177}, {46.345, 168.827, 991.177}, {46.345, 158.508, 991.177}, {46.345, 140.823, 991.177}};
    private int colorIndex = 0;
    // private bool haloSwitch = true;
    private int haloSwitch = 1;
    private int totalIteration = 15;    //15초에 걸쳐 이동
    private int LightsMoveSequence = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Kick Start() 호출됨");
        source = GetComponent<AudioSource>();

        GameObject spotLightsParent = GameObject.FindWithTag("SpotLightsParent");
        childLights = spotLightsParent.GetComponentsInChildren<Light>();
        lightsCount = childLights.Length;

        //지우기
        GameObject AreaLightObject = GameObject.Find("Area Light (3)");
        // AreaLightObject.active();

        try
        {
            for (int i = 0; i < lightsCount; i++)
            {
                childLights[i].color = Color.yellow;

                var halo = childLights[i].GetComponent("Halo");
                halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
            }
        }

        //Most likely to 'Index Out Of Range' error
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }

        AllLightsOff();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            source.Play();

            LightingEffects_4();

        }

        //전체 조명 소등, 수직 아래 방향으로 이동 후 가장자리 조명부터 가운데로 1초 간격 점등
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("LightingEffects_1() 호출됨");
            LightingEffects_1();
        }

        //조명 방향 수직으로 이동 후 원위치
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("LightingEffects_2() 호출됨");
            LightingEffects_2();
        }

        //조명 수직 이동 후 홀수, 짝수 번 교차로 켜짐 (10회 반복)
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("LightingEffects_3() 호출됨");
            LightingEffects_3();
        }

        //조명의 색 랜덤하게 변경 (조명 수직방향으로 이동 후 실행)
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("LightingEffects_4() 호출됨");
            LightingEffects_4();
        }

        //모든 조명 점등
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("AllLightsOn() 호출됨");
            AllLightsOn();
        }

        //CancelInvoke 후 모든 조명 소등
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("AllLightsOff() 호출됨");
            AllLightsOff();
        }

        //모든 조명 원위치 (LightingEffects_2에서 호출)
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Debug.Log("AllLightsInPosition() 호출됨");
            AllLightsInPosition();
        }

        //모든 조명 수직방향 이동 (LightingEffects_1에서 호출)
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Debug.Log("AllLightsInVerticalPosition() 호출됨");
            AllLightsInVerticalPosition();
        }

        //Color 배열의 순서대로 조명의 색 변경
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Debug.Log("SerialColorChange() 호출됨");
            SerialColorChange();
        }

        //Light의 Halo 홀수/짝수 번째 점등 및 소등
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("HaloControl() 호출됨");
            HaloControl();
        }
    }

    //모든 조명 점등
    void AllLightsOn()
    {
        try
        {
            for (int i = 0; i < lightsCount; i++)
            {
                childLights[i].enabled = true;
            }
        }

        //Most likely to 'Index Out Of Range' error
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }
    }

    //CancelInvoke 후 모든 조명 소등
    void AllLightsOff()
    {
        if (IsInvoking())
        {
            CancelInvoke();
        }

        try
        {
            for (int i = 0; i < lightsCount; i++)
            {
                childLights[i].enabled = false;
            }
        }

        //Most likely to 'Index Out Of Range' error
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }
    }

    //Light의 Halo 홀수/짝수 번째 점등 및 소등
    void HaloControl()
    {
        try
        {
            //일단 모두 점등
            for (int i = 0; i < lightsCount; i++)
            {
                var halo = childLights[i].GetComponent("Halo");
                halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
            }

            for (int i = 0; i < lightsCount; i++)
            {
                if (haloSwitch == 0)
                {      //모두 점등
                    var halo = childLights[i].GetComponent("Halo");
                    halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
                }

                else if (haloSwitch == 1)
                {
                    if (i % 2 == 1)
                    {        //짝수만 점등
                        var halo = childLights[i].GetComponent("Halo");
                        halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                    }
                }
                else if (haloSwitch == 2)
                { //홀수만 점등
                    if (i % 2 == 0)
                    {
                        var halo = childLights[i].GetComponent("Halo");
                        halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                    }
                }
                else if (haloSwitch == 3)
                { //모두 소등
                    var halo = childLights[i].GetComponent("Halo");
                    halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
                }
            }
        }

        //Most likely to 'Index Out Of Range' error
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }

        finally
        {
            haloSwitch = (haloSwitch + 1) % 4;
        }
    }

    //Color 배열의 순서대로 조명의 색 변경
    void SerialColorChange()
    {
        Debug.Log("colorIndex: " + colorIndex);

        for (int i = 0; i < lightsCount; i++)
        {
            childLights[i].color = colors[colorIndex];
        }
        colorIndex = (colorIndex + 1) % colors.Length;
        AllLightsOn();
    }

    void DelayedLightsOn()
    {
        //Lights on in pairs
        for (int i = 0; i < 2; i++)
        {
            //Run code only if queueu is not empty
            if (lightsOffOrder.Count > 0)
            {
                int index = lightsOffOrder.Dequeue();
                childLights[index].enabled = true;
            }
        }
    }

    //모든 조명 수직방향 이동 (LightingEffects_1에서 호출)
    void AllLightsInVerticalPosition()
    {
        StopCoroutine("LightsVerticalMove");
        for (int i = 0; i < lightsCount; i++)
        {
            childLights[i].transform.localEulerAngles = new Vector3(-270, 0, 0);
        }
        AllLightsOn();
    }

    //모든 조명 원위치 (LightingEffects_2에서 호출)
    void AllLightsInPosition()
    {
        StopCoroutine("LightsToOriginalPositionMove");
        for (int i = 0; i < lightsCount; i++)
        {
            childLights[i].transform.localEulerAngles = new Vector3((float)childLightsDefaultRotationAngle[i, 0], (float)childLightsDefaultRotationAngle[i, 1], (float)childLightsDefaultRotationAngle[i, 2]);
        }
        AllLightsOn();
    }

    //현재 점등 상태의 반대로 동작 (LightingEffects_3에서 호출)
    void LightsReverse()
    {
        try
        {
            Debug.Log("LightsReverse");

            for (int i = 0; i < lightsCount; i++)
            {
                childLights[i].enabled = !childLights[i].enabled;
            }
        }

        //Most likely to 'Index Out Of Range' error
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }
    }

    //모든 조명 0.3초 간격 수직방향 이동
    IEnumerator LightsVerticalMove(int sequence, int totalIteration)
    {
        // Debug.Log("LightsVerticalMove 실행 중");

        for (int i = 0; i < lightsCount; i++)
        {
            float x = (float)((childLightsDefaultRotationAngle[i, 0] * sequence / (float)totalIteration));
            float y = (float)((childLightsDefaultRotationAngle[i, 1] * sequence / (float)totalIteration));
            float z = (float)((childLightsDefaultRotationAngle[i, 2] * sequence / (float)totalIteration));

            childLights[i].transform.localEulerAngles = new Vector3(x, y, z);
            AllLightsOn();

            Debug.Log("i: " + i + " x: " + x + " y: " + y + " z: " + z);
        }

        yield return new WaitForSeconds(0.3f);  //wait for 0.3 seconds
    }

    //전체 조명 소등, 수직 아래 방향으로 이동 후 가장자리 조명부터 가운데로 1초 간격 점등
    void LightingEffects_1()
    {
        AllLightsInVerticalPosition();
        AllLightsOff();

        try
        {
            //count == 7
            //(0, 6), (1, 5), (2, 4), (3, 3)
            int iteration = (lightsCount + 1) / 2;

            for (int i = 0; i < iteration; i++)
            {
                lightsOffOrder.Enqueue(i);
                lightsOffOrder.Enqueue(lightsCount - i - 1);

                Invoke("DelayedLightsOn", (i + 1) * 1.0f);
            }
        }

        //Most likely to 'Index Out Of Range' error
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }
    }

    //모든 조명 15초에 걸쳐 수직방향 이동 (Invoke로 구현)
    void DelayedLightsMove()
    {
        Debug.Log("DelayedLightsMove 호출됨");

        for (int i = 0; i < lightsCount; i++)
        {
            float x = (float)((childLightsDefaultRotationAngle[i, 0] * LightsMoveSequence / (float)totalIteration));
            float y = (float)((childLightsDefaultRotationAngle[i, 1] * LightsMoveSequence / (float)totalIteration));
            float z = (float)((childLightsDefaultRotationAngle[i, 2] * LightsMoveSequence / (float)totalIteration));

            childLights[i].transform.localEulerAngles = new Vector3(x, y, z);
            AllLightsOn();

            Debug.Log("i: " + i + " x: " + x + " y: " + y + " z: " + z);
        }
        LightsMoveSequence = (LightsMoveSequence + 1) % totalIteration;
    }

    //조명 방향 수직으로 이동 후 원위치
    void LightingEffects_2()
    {
        AllLightsOff();
        // int totalIteration = 15;    //15초에 걸쳐 이동

        // //5초에 걸쳐 이동 (0.3초 간격) -> 15번
        // for(int i=0; i<totalIteration; i++) {
        //     StartCoroutine(LightsVerticalMove(i, totalIteration));
        // }

        for (int i = 0; i < totalIteration; i++)
        {
            Invoke("DelayedLightsMove", (i + 1) * 1.0f);
        }

        // AllLightsOn();
    }

    //조명 수직 이동 후 홀수, 짝수 번 교차로 켜짐 (10회 반복)
    void LightingEffects_3()
    {
        AllLightsOff();
        AllLightsInVerticalPosition();

        //8개이면 (0, 2, 4, 6), (1, 3, 5, 7)
        //7개이면 (0, 2, 4, 6), (1, 3, 5)   #(int) mod2 + 나머지 = 원래 값
        int oddCount = (int)lightsCount / 2;
        int evenCount = lightsCount - oddCount;
        int repeatTime = 10;

        for (int i = 0; i < oddCount; i++)
        {
            childLights[i * 2 + 1].enabled = true;
        }

        for (int i = 0; i < evenCount; i++)
        {
            childLights[i * 2].enabled = false;
        }

        for (int j = 0; j < repeatTime; j++)
        {
            Invoke("LightsReverse", (j + 1) * 0.5f);
        }

        // //전체 반복 1000회
        // for(int i=0; i<1000; i++) {
        //     Debug.Log("i: " + i);

        //     for(int j=0; j<lightsCount; j++) {
        //         childLights[j].enabled = !childLights[j].enabled;
        //     }
        // }
    }


    //조명의 색 랜덤하게 변경 (조명 수직방향으로 이동 후 실행)
    void LightingEffects_4()
    {
        // AllLightsInVerticalPosition();
        AllLightsOff();
        int colorChoice = UnityEngine.Random.Range(0, colors.Length);

        try
        {
            for (int i = 0; i < lightsCount; i++)
            {
                childLights[i].color = colors[colorChoice];
                childLights[i].enabled = true;
                //var halo = childLights[i].GetComponent("Halo");
                //halo.GetType().GetProperty("enabled").SetValue(halo, true, null);

                SerializedObject halo = new SerializedObject(childLights[i].GetComponent("Halo"));
                halo.FindProperty("m_Color").colorValue = colors[colorChoice];
                halo.ApplyModifiedProperties();
            }
        }

        //Most likely to 'Index Out Of Range' error
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }

        AllLightsOn();
    }
}
