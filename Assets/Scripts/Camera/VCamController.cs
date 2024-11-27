using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCamController : MonoBehaviour
{
    #region Fields

    [SerializeField] private CinemachineVirtualCamera virtualCameraR = null;
    [SerializeField] private CinemachineVirtualCamera virtualCameraL = null;
    [SerializeField] private CinemachineVirtualCamera virtualCameraMain = null;

    enum CurrentCamera
    {
        MAIN,
        LEFT,
        RIGHT,
        COUNT
    };

    [SerializeField] private CurrentCamera currentCamera = CurrentCamera.MAIN;

    #endregion


    #region Monobehavior

    private void Start()
    {
        virtualCameraR.enabled = false;
        virtualCameraL.enabled = false;
    }

    private void Update()
    {
        if (GameController.Instance.state == GameState.FreeRoam)
        {
            switch (currentCamera)
            {
                case CurrentCamera.MAIN:
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        virtualCameraMain.enabled = false;
                        virtualCameraR.enabled = true;
                        currentCamera = CurrentCamera.RIGHT;
                    }
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        virtualCameraMain.enabled = false;
                        virtualCameraL.enabled = true;
                        currentCamera = CurrentCamera.LEFT;
                    }


                    break;
                case CurrentCamera.RIGHT:

                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        virtualCameraR.enabled = false;
                        virtualCameraMain.enabled = true;
                        currentCamera = CurrentCamera.MAIN;
                    }
                    break;
                case CurrentCamera.LEFT:

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        virtualCameraL.enabled = false;
                        virtualCameraMain.enabled = true;
                        currentCamera = CurrentCamera.MAIN;
                    }
                    break;
            }



            if (virtualCameraL.enabled)
            {
                PlayerController.Instance.transform.rotation = Quaternion.Euler(0, 90, -90);
            }
            else if (virtualCameraR.enabled)
            {
                PlayerController.Instance.transform.rotation = Quaternion.Euler(0, -90, 90);
            }
            else
            {
                PlayerController.Instance.transform.rotation = Quaternion.identity;
            }
        }
        
    }


    #endregion
}
