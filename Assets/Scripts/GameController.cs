using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Valve.VR;
using LogSystem;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private int loops = 1;
    [SerializeField]
    private GameObject exitSign;
    public float time;

    [SerializeField]
    private SteamVR_Action_Boolean Quit, Submit;
    private IEnumerator coroutine;
    private bool exit = false, quitHit = false;
    private float quitHoldTime = 0;
    private LogSicknessLevel gameLog;
    private void Start()
    {
        exitSign.SetActive(false);
        gameLog = GetComponent<LogSicknessLevel>();
        loops = (loops-1) * 2 + 1;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (loops == 0)
            {
                other.gameObject.GetComponent<PlayerMovement>().enabled = false;
                exit = true;
                exitSign.SetActive(true);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (loops <= 0) loops = 0; else loops--;
            Debug.Log("Other Collider:" + other.name);
        }
    }

    private IEnumerator QuitGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Application.Quit();
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (exit && (Submit.stateDown||Input.GetAxis("RightTrigger")>0.1f))
        {
            Application.OpenURL("https://uncg.qualtrics.com/jfe/form/SV_bK4DzgGD7VZmS7Y");
            Application.Quit();
        }
        else if ((Quit.state || Input.GetAxis("LeftTrigger")>0.1f) && !quitHit)
        {
            if (quitHoldTime > 2)
            {
                gameLog.LogEarlyQuit();
                coroutine = QuitGame(0.1f);
                StartCoroutine(coroutine);
                quitHit = true;
            }
            else
            {
                quitHoldTime += Time.deltaTime;
            }
        }else if (!Quit.state)
        {
            quitHoldTime = 0;
            quitHit = false;
        }
    }
}
