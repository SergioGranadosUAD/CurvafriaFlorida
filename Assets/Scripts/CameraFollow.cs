using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private CinemachineVirtualCamera m_camera;
    public CinemachineVirtualCamera Camera
    {
        get
        {
            if(m_camera == null)
            {
                m_camera = GetComponent<CinemachineVirtualCamera>();
            }
            return m_camera;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCameraTarget(Transform playerTransform)
    {
        Camera.Follow = playerTransform;
        Camera.LookAt = playerTransform;
    }
}
