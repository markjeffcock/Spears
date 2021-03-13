using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour

{
    public List<GameObject> controllerPrefabs;
    public InputDeviceCharacteristics controllerCharacteristics;
    public bool showController = false;
    public GameObject handModelPrefab;
    private InputDevice targetDevice;

    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;


    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();
    }

    void TryInitialize()
    {

        List<InputDevice> devices = new List<InputDevice>();
        // This line is not required anymore---> InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        // InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices); // changed this line
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices); // <--- to this

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);

            }
            else
            {
                Debug.LogError("Did not find corresponding controller");
                // here you can instantiate a "default" controller model, similar to the hand model (if you like)
            }

            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    void UpdateHandAnimation()
    {

        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        // Here you missed to toggle off the animation or basically set a default value
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        // Here you missed to toggle of the animation or basically set a default value
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }

    }

    // ---> closing curly bracket erased here "{"


    // Update is called once per frame
    void Update()
    //---> !!! Here was missing an opening curly bracket "{" or set wrong
    {

        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        // ---> here was missing an "else" statement
        else
        {
            if (showController)
            {
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);
                UpdateHandAnimation();
            }

        }
    }
    //--> missing closing curly brackets
}