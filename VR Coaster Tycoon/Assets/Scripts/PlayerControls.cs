using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.XR;

public class PlayerControls : MonoBehaviour
{
    // Start is called before the first frame update
    private InputDevice left;
    private InputDevice right;
    public OVRPlayerController controller;

    public GameObject UserInterface;
    public float flySpeed;
    private bool fly = false;
    private Vector3 height = new Vector3(0, 0, 0);
    void Start()
    {
        TryInitialize();
    }

    void TryInitialize()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);
        //Debug.Log("got here");


        if (devices.Count > 1)
        {
            left = devices[1];
            right = devices[2];
            Debug.Log(left.name + right.name);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(!left.isValid || !right.isValid)
        {
            TryInitialize();

        }
        left.TryGetFeatureValue(CommonUsages.trigger, out float LTrigval);
        left.TryGetFeatureValue(CommonUsages.grip, out float LGripval);
        right.TryGetFeatureValue(CommonUsages.trigger, out float RTrigval);
        left.TryGetFeatureValue(CommonUsages.grip, out float RGripval);
        if (RTrigval > 0.8f &&  LTrigval > 0.8f && LGripval > 0.8f && RGripval > 0.8f && !fly)
        {
            Debug.Log("fly mode");
            controller.GravityModifier = 0;
            //Debug.Log(controller.GravityModifier);
            fly = true;
        } else if (RTrigval > 0.8f && LTrigval > 0.8f && LGripval > 0.8f && RGripval > 0.8f && fly)
        {
            Debug.Log("Fly disable");
            controller.GravityModifier = 1;
            fly = false;
        }

        if (fly)
        {
            if(right.TryGetFeatureValue(CommonUsages.trigger, out float RTrigval2) && RTrigval2 > 0.1f)
            {
                controller.JumpForce = RTrigval2 * flySpeed * Time.deltaTime;
                controller.Jump();
            }
            if(left.TryGetFeatureValue(CommonUsages.trigger, out float LTrigval2) && LTrigval2 > 0.1f)
            {
                controller.JumpForce = -LTrigval2 * flySpeed * Time.deltaTime;
                controller.Jump();
                //transform.position = Vector3.MoveTowards(transform.position, transform.position + height, flySpeed * Time.deltaTime);
            }
            //transform.position = Vector3.MoveTowards(transform.position, player.position, flySpeed);
            Debug.Log(transform.position);
        }

        if(left.TryGetFeatureValue(CommonUsages.primaryButton, out bool Lpress) && Lpress)
        {
            if (UserInterface.activeSelf)
            {
                UserInterface.SetActive(false);
                wait();
            }
            else
            {
                UserInterface.SetActive(true);
                wait();
            }
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3f);
    }
}


