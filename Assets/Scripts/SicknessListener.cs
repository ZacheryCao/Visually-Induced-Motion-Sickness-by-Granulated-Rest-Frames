using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using Valve.VR;
using LogSystem;

public class SicknessListener : MonoBehaviour
{
    [SerializeField]
    private TMP_Text sicknessLevel;
    [SerializeField]
    private SteamVR_Action_Boolean AButton, BButton, XButton, YButton, Submit;
    [SerializeField]
    private Slider sicknessSlider;
    private PlayerMovement player;
    private IEnumerator coroutine;
    [SerializeField]
    private int Waypoint_No = 0;
    public int SiknessLevel = 0;
    private bool trigger, logReady, quit;
    private LogSicknessLevel sicklog;
    private GameController gamecontroller;


    private void Start()
    {
        sicklog = (LogSicknessLevel)FindObjectOfType(typeof(LogSicknessLevel));
        quit = false;
        SteamVR.Initialize();
        player = (PlayerMovement)FindObjectOfType(typeof(PlayerMovement));
        gamecontroller = (GameController)FindObjectOfType(typeof(GameController));
    }

    private void Update()
    {
        //if (!trigger)
        //{
            if (AButton.stateDown || BButton.stateDown || Input.GetButtonDown("ButtonB"))
            {
                sicknessSlider.value = sicknessSlider.value > 10 ? 10 : sicknessSlider.value + 1;
                sicknessLevel.text = sicknessSlider.value.ToString();
                SiknessLevel = (int)sicknessSlider.value;
            }
            else if (XButton.stateDown || YButton.stateDown || Input.GetButtonDown("ButtonX"))
            {
                sicknessSlider.value = sicknessSlider.value <= 0 ? 0 : sicknessSlider.value - 1;
                sicknessLevel.text = sicknessSlider.value.ToString();
                SiknessLevel = (int)sicknessSlider.value;
            }
            trigger = true;
        //}
        //if(Input.GetAxis("LeftJoystickX") == 0)
        //{
        //    trigger = false;
        //}

        if ((Submit.stateDown||Input.GetAxis("RightTrigger")>0.1f) && logReady)
        {
            logReady = false;
            List<string> list = new List<string>();
            list.Add(Waypoint_No.ToString());
            list.Add(SiknessLevel.ToString());
            list.Add(gamecontroller.time.ToString());
            list.AddRange(player.GetTransitionValues());
            //sicklog.LogPerformance(list);
            player.ResetTransitionValues();
            gamecontroller.time = 0;
            if (SiknessLevel == 10)
            {
                player.enabled = false;
                //Application.OpenURL("https://uncg.qualtrics.com/jfe/form/SV_bK4DzgGD7VZmS7Y");
                coroutine = QuitGame(1.0f);
                StartCoroutine(coroutine);
            }
            else if (SiknessLevel < 10)
            {
                SiknessLevel = 0;
                sicknessSlider.value = 0;
                sicknessLevel.text = "0";
                this.gameObject.SetActive(false);
            }
        }
        if(Submit.state == false||Input.GetAxis("RightTrigger") <= 0.1f)
        {
            logReady = true;
        }

    }

    private void Awake()
    {
        sicknessSlider.value = 0;
        sicknessLevel.text = "0";
    }

    private IEnumerator QuitGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Application.Quit();
    }


}
