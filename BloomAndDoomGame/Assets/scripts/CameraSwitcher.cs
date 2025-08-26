using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineCamera freelookCam;
    [SerializeField] private CinemachineCamera aimCam;
    [SerializeField] private CinemachineInputAxisController inputAxisController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject crosshairUI;
    [SerializeField] private PlayerControls input;

    private InputAction aimAction;
    private bool isAiming = false;
    private Transform yawTarget;
    private Transform pitchTarget;

    private AimCameraController aimCamController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aimCamController = aimCam.GetComponent<AimCameraController>();

        inputAxisController = freelookCam.GetComponent<CinemachineInputAxisController>();

        input = new PlayerControls();
        input.Enable();
        aimAction = input.Gameplay.Aim;

    }

    // Update is called once per frame
    void Update()
    {
        bool aimPressed = aimAction.IsPressed();
        player.isAiming = aimPressed;

        if (aimPressed && !isAiming)
        {
            EnterAimMode();
        }
        else if (!aimPressed && isAiming)
        {
            ExitAimMode();
        }
    }

    private void ExitAimMode()
    {
        isAiming = false;

        SnapFreeLookBehindPlayer();

        aimCam.Priority = 10;
        freelookCam.Priority = 20;

        inputAxisController.enabled = true;
    }

    private void SnapFreeLookBehindPlayer()
    {
        CinemachineOrbitalFollow orbitalFollow = freelookCam.GetComponent<CinemachineOrbitalFollow>();
        Vector3 forward = aimCam.transform.forward;
        float angle = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        orbitalFollow.HorizontalAxis.Value = angle;
    }

    private void SnapAimCameraToPlayerForward()
    {
        aimCamController.SetYawPitchFromCameraForward(freelookCam.transform);
    }

    private void EnterAimMode()
    {
        isAiming = true;
        
        SnapAimCameraToPlayerForward();

        aimCam.Priority = 20;
        freelookCam.Priority = 10;

        inputAxisController.enabled = false;
    }
}
