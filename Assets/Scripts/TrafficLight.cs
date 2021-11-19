using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TrafficLight : MonoBehaviour
{
    public Material[] TrafficLightMaterial;
    public GameObject question;
    private bool greenLight;
    private IEnumerator coroutine;
    [SerializeField]
    private SteamVR_Action_Boolean Submit;
    private PlayerMovement playerMove;

    private void Start()
    {
        SteamVR.Initialize();
        TurnOnLight(0);
        greenLight = true;
        question.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && greenLight)
        {
            playerMove = other.gameObject.GetComponent<PlayerMovement>();
            TurnOnLight(1);
            coroutine = WaitAndSwitch(0.5f);
            StartCoroutine(coroutine);
            greenLight = false;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Destroy(gameObject);
            greenLight = true;
            TurnOnLight(0);
            question.SetActive(false);
        }
    }

    void TurnOnLight(int light)
    {
        switch (light)
        {
            case 0:
                TrafficLightMaterial[0].SetColor("_Color", Color.green);
                TrafficLightMaterial[1].SetColor("_Color", new Color(0.25f,0.25f,0,1));
                TrafficLightMaterial[2].SetColor("_Color", new Color(0.25f, 0, 0, 1));
                break;
            case 1:
                TrafficLightMaterial[0].SetColor("_Color", new Color(0, 0.25f, 0, 1));
                TrafficLightMaterial[1].SetColor("_Color", new Color(1, 1, 0, 1));
                TrafficLightMaterial[2].SetColor("_Color", new Color(0.25f, 0, 0, 1));
                break;
            case 2:
                Debug.Log("Red");
                TrafficLightMaterial[0].SetColor("_Color", new Color(0, 0.25f, 0, 1));
                TrafficLightMaterial[1].SetColor("_Color", new Color(0.25f, 0.25f, 0, 1));
                TrafficLightMaterial[2].SetColor("_Color", new Color(1, 0, 0, 1));
                break;
            default:
                break;
        }


    }

    void TurnOnGreenLight()
    {
        if ((Submit.stateDown|| Input.GetAxis("RightTrigger") > 0.1f) && !greenLight)
        {
            TurnOnLight(0);
            playerMove.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TurnOnGreenLight();
    }

    private IEnumerator WaitAndSwitch(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        TurnOnLight(2);
        question.SetActive(true);
        playerMove.resetSpeed();
        playerMove.enabled = false;
        FileListener.fileOpen = true;
    }
}
