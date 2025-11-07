using System;
using TMPro;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject ball;
    public Rigidbody2D ballRigidBody;
    public bool zoomOutCam = false;
    private float zoomZoom;
    private bool poopi = true;

    void Start()
    {
        mainCamera.orthographicSize = 5;
        zoomZoom = 5;

        ballRigidBody = ball.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CamZoomOut();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object hit is an enemy
        if (other.CompareTag("Ball"))
        {
            zoomOutCam = true;
        }
    }
    
    public void CamZoomOut()
    {
        if (zoomOutCam)
        {
            if (zoomZoom <= 8)
            {
                zoomZoom += 0.1f;
            }
            else
            {
                ballRigidBody.bodyType = RigidbodyType2D.Dynamic;
                if(poopi)
                {
                    ballRigidBody.AddForce(new Vector2(-350, 800), ForceMode2D.Force);
                    poopi = false;
                }
                
            }
            mainCamera.orthographicSize = zoomZoom;
        }
    }
}
