using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform m_target;
    [SerializeField]//This is border of game map
    private Vector2 m_cameraBorder = new Vector2(-12.5f, 12.0f);
    [SerializeField]
    private float m_smoothTime = 0.5f;
    //This is border of camera position
    private Vector2 m_cameraClampPosition;
    private Camera m_camera;
    private float m_velocity;
    

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
    }

    private void Start()
    {
        //With 0.5 size in orthographic mode of camera 1000 pixels == 1.0m
        //To get real size of camera in meters:(m_camera.orthographicSize * 2.0f) * (Screen.width / 1000f);
        float realSize = (m_camera.orthographicSize * 2.0f) * (Screen.width / 1000f);
        
        m_cameraClampPosition = new Vector2(Mathf.Clamp(m_cameraBorder.x + (realSize * 0.5f), float.MinValue, 0.0f)
                                           ,Mathf.Clamp(m_cameraBorder.y - (realSize * 0.5f), 0.0f, float.MaxValue));

        //Enable camera movement if there is more than 1 meter from the edges of the camera to the map border
        enabled = m_cameraClampPosition.x < -1.0f || m_cameraClampPosition.y > 1.0f;

        if(enabled)//Set camera in start position
        {
            Vector3 position = transform.position;
            position.x = GetTargetPosition();
            transform.position = position;
        }
    }

    private void LateUpdate()
    {
        Vector3 position = transform.position;
        float targetPosition = GetTargetPosition();
        position.x = Mathf.SmoothDamp(position.x, targetPosition, ref m_velocity, m_smoothTime);
        transform.position = position;
    }

    private float GetTargetPosition()
    {
        return Mathf.Clamp(m_target.position.x, m_cameraClampPosition.x, m_cameraClampPosition.y);
    }
}
