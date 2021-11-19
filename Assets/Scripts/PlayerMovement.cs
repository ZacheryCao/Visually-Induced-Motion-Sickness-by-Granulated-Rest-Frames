using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3.0F;
    public float rotateSpeed = 3.0F, accelTime = 2.0f;
    public GameObject Cages;
    private Camera camera;
    [SerializeField]
    private SteamVR_Action_Vector2 move, rotate;
    private CharacterController m_CharacterController;
    private float rotTime, rotDegrees, tranlateTime, translateDistance, curSpeed;


    private void Start()
    {
        SteamVR.Initialize();
        camera = FindObjectOfType<Camera>();
        transform.rotation = Quaternion.RotateTowards(transform.rotation, camera.transform.rotation, 360);
        m_CharacterController = GetComponent<CharacterController>();
        curSpeed = 0;
        translateDistance = 0;
    }
    void Update()
    {
        if (!m_CharacterController.isGrounded)
        {
            m_CharacterController.Move(Vector3.down * 9.8f);
        }
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, forward * 10, Color.green);
        // Rotate around y - axis
        float currentAngle = transform.rotation.eulerAngles.y;
        if(Mathf.Abs(rotate.axis.x)> 0.01f)
        {
            transform.rotation = Quaternion.AngleAxis(currentAngle + rotate.axis.x * rotateSpeed * Time.deltaTime, Vector3.up);
            rotTime += Time.deltaTime;
            rotDegrees += Mathf.Abs(rotate.axis.x) * rotateSpeed * Time.deltaTime;
        }
        if (Mathf.Abs(Input.GetAxis("RightJoystickX")) > 0.01f)
        {
            transform.rotation = Quaternion.AngleAxis(currentAngle + Input.GetAxis("RightJoystickX") * rotateSpeed * Time.deltaTime, Vector3.up);
            rotTime += Time.deltaTime;
            rotDegrees += Mathf.Abs(rotate.axis.x) * rotateSpeed * Time.deltaTime;
        }
        // Move forward / backward
        if (Mathf.Abs(move.axis.y) > 0.1f)
        {
            //curSpeed += speed / accelTime * Time.deltaTime;
            //curSpeed = Mathf.Min(curSpeed, speed);
            curSpeed = speed * Mathf.Abs(move.axis.y);
            m_CharacterController.Move(forward * curSpeed * Time.deltaTime);
            tranlateTime += Time.deltaTime;
            translateDistance += Mathf.Abs(curSpeed * Time.deltaTime);
        }
        else
        {
            resetSpeed();
        }
        if (Mathf.Abs(Input.GetAxis("LeftJoystickY")) > 0.1f)
        {
            //curSpeed += speed / accelTime * Time.deltaTime;
            //curSpeed = Mathf.Min(curSpeed, speed);
            curSpeed = speed * Mathf.Abs(Input.GetAxis("LeftJoystickY"));
            m_CharacterController.Move(forward * curSpeed * Time.deltaTime);
            tranlateTime += Time.deltaTime;
            translateDistance += Mathf.Abs(curSpeed * Time.deltaTime);
        }
        else
        {
            resetSpeed();
        }
        Cages.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z);
    }

    public void resetSpeed()
    {
        curSpeed = 0;
    }

    public void ResetTransitionValues()
    {
        rotTime = rotDegrees = tranlateTime = translateDistance = 0;
    }


    public List<string> GetTransitionValues()
    {
        float AverageSpeed = translateDistance / tranlateTime;
        return new List<string>(new string[] { AverageSpeed.ToString(), rotDegrees.ToString(), rotTime.ToString()});
    }
}