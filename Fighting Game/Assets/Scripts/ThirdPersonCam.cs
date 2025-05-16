using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;

    public CinemachineCamera thirdPersonCam;
    public CinemachineCamera combatCam;

    public CameraStyle currentStyle;
    public enum CameraStyle
    {
        Basic,
        Combat
    }

    private const int highPriority = 20;
    private const int lowPriority = 10;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        // switch styles
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyle(CameraStyle.Combat);

        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // roate player object
        if (currentStyle == CameraStyle.Basic)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        else if (currentStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        thirdPersonCam.Priority = highPriority;
        combatCam.Priority = lowPriority;

        if (newStyle == CameraStyle.Basic)
        {
            thirdPersonCam.Priority = highPriority;
            combatCam.Priority = lowPriority;
            Debug.Log("3rd person");

        }

        if (newStyle == CameraStyle.Combat)
        {
            thirdPersonCam.Priority = lowPriority;
            combatCam.Priority = highPriority;
            Debug.Log("combat");
           
        }

        currentStyle = newStyle;
    }
}
