using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;
using Unity.VisualScripting;

public class CameraControlTrigger : MonoBehaviour
{
    public Movement playerMovement;
    private Player playerScript;
    public CustomInspectorObjects customInspectorObjects;

    private Collider2D coll;
    private bool shouldPanInStay;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
        playerScript = playerMovement.GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("DashingPlayer"))
        {

            shouldPanInStay = false;

            if(customInspectorObjects.panCameraOnContact && playerMovement.FacingDirection == customInspectorObjects.playerFacingDirectionForPan)
            {
                //pan the camera
                PanCamera();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("DashingPlayer"))
        {
            if(customInspectorObjects.panCameraOnContact && ((playerMovement.FacingDirection == customInspectorObjects.playerFacingDirectionForPan && !playerScript.LedgeClimbState.isTurning) || (playerMovement.FacingDirection != customInspectorObjects.playerFacingDirectionForPan && playerScript.LedgeClimbState.isTurning)) && shouldPanInStay && (playerScript.StateMachine.CurrentState != playerScript.InAirState && playerScript.StateMachine.CurrentState != playerScript.WallJumpState))
            {
                //pan the camera
                Invoke("PanCamera", 0.15f);
                shouldPanInStay = false;
            }
            else if(customInspectorObjects.panCameraOnContact && ((playerMovement.FacingDirection != customInspectorObjects.playerFacingDirectionForPan && !playerScript.LedgeClimbState.isTurning) || (playerMovement.FacingDirection == customInspectorObjects.playerFacingDirectionForPan && playerScript.LedgeClimbState.isTurning)) && !shouldPanInStay && (playerScript.StateMachine.CurrentState != playerScript.InAirState && playerScript.StateMachine.CurrentState != playerScript.WallJumpState))
            {
                //pan the camera
                Invoke("PanCameraBack", 0.15f);

                shouldPanInStay = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("DashingPlayer"))
        {

            Vector2 exitDirection = (collision.transform.position - coll.bounds.center).normalized;

            if(customInspectorObjects.swapCamerasHorizontal && customInspectorObjects.cameraOnLeft != null && customInspectorObjects.cameraOnRight != null)
            {
                //swap cameras
                CameraManager.instance.SwapCameraHorizontal(customInspectorObjects.cameraOnLeft, customInspectorObjects.cameraOnRight, exitDirection);
            }

            if(customInspectorObjects.swapCamerasVertical && customInspectorObjects.cameraOnBottom != null && customInspectorObjects.cameraOnTop != null)
            {
                //swap cameras
                CameraManager.instance.SwapCameraVertical(customInspectorObjects.cameraOnBottom, customInspectorObjects.cameraOnTop, exitDirection);
            }

            if(customInspectorObjects.panCameraOnContact)
            {
                //pan the camera
                PanCameraBack();
            }
        }
        
    }

    private void PanCamera()
    {
        CameraManager.instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, false);   
    }

    private void PanCameraBack()
    {
        CameraManager.instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, true);
    }
}

[System.Serializable]
public class CustomInspectorObjects
{
    public bool swapCamerasHorizontal = false;
    public bool swapCamerasVertical = false;
    public bool panCameraOnContact = false;

    [HideInInspector] public CinemachineVirtualCamera cameraOnLeft;
    [HideInInspector] public CinemachineVirtualCamera cameraOnRight;
    [HideInInspector] public CinemachineVirtualCamera cameraOnBottom;
    [HideInInspector] public CinemachineVirtualCamera cameraOnTop;

    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;
    [HideInInspector] public int playerFacingDirectionForPan = 1;
}

public enum PanDirection
{
    Up,
    Down,
    Left,
    Right
}

#if UNITY_EDITOR

[CustomEditor(typeof(CameraControlTrigger))]
public class MyScriptEditor : Editor
{
    CameraControlTrigger cameraControlTrigger;

    private void OnEnable()
    {
        cameraControlTrigger = (CameraControlTrigger)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(cameraControlTrigger.customInspectorObjects.swapCamerasHorizontal)
        {
            cameraControlTrigger.customInspectorObjects.cameraOnLeft = EditorGUILayout.ObjectField("Camera on Left", cameraControlTrigger.customInspectorObjects.cameraOnLeft,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;

            cameraControlTrigger.customInspectorObjects.cameraOnRight = EditorGUILayout.ObjectField("Camera on Right", cameraControlTrigger.customInspectorObjects.cameraOnRight,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
        }

        if(cameraControlTrigger.customInspectorObjects.swapCamerasVertical)
        {
            cameraControlTrigger.customInspectorObjects.cameraOnBottom = EditorGUILayout.ObjectField("Camera on Bottom", cameraControlTrigger.customInspectorObjects.cameraOnBottom,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;

            cameraControlTrigger.customInspectorObjects.cameraOnTop = EditorGUILayout.ObjectField("Camera on Top", cameraControlTrigger.customInspectorObjects.cameraOnTop,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
        }

        if(cameraControlTrigger.customInspectorObjects.panCameraOnContact)
        {
            cameraControlTrigger.customInspectorObjects.panDirection = (PanDirection) EditorGUILayout.EnumPopup("Camera Pan Direction",
                cameraControlTrigger.customInspectorObjects.panDirection);

            cameraControlTrigger.customInspectorObjects.panDistance = EditorGUILayout.FloatField("Pan Distance", cameraControlTrigger.customInspectorObjects.panDistance);
            cameraControlTrigger.customInspectorObjects.panTime = EditorGUILayout.FloatField("Pan Time", cameraControlTrigger.customInspectorObjects.panTime);
            cameraControlTrigger.customInspectorObjects.playerFacingDirectionForPan = EditorGUILayout.IntField("Player Facing Direction For Pan", cameraControlTrigger.customInspectorObjects.playerFacingDirectionForPan);
        }

        if(GUI.changed)
        {
            EditorUtility.SetDirty(cameraControlTrigger);
        }
    }
}
#endif
