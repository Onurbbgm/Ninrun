using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class LockCameraY : CinemachineExtension
{
    [Tooltip("Lock the camera's Y position to this value")]
    public float m_YPosition = 6.32f;
    [Tooltip("Lock the camera's X position to this value")]
    public float m_XPosition = 0f;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            //if (Input.GetKeyDown("a"))
            //{
            //    transform.eulerAngles += new Vector3(0f, -5f, 0f);
            //}
            //if (Input.GetKeyDown("d"))
            //{
            //    transform.eulerAngles += new Vector3(0f, +5f, 0f);
            //}
            var pos = state.RawPosition;
            pos.y = m_YPosition;
            pos.x = m_XPosition;
            state.RawPosition = pos;
        }
    }
}
