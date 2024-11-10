using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class aims to manage hand-tracking and head-tracking, with functions related to them
public class InputManager : MonoBehaviour
{
    public static InputManager Instance {get; private set;}

    public Vector3 lefthandPosition {get; private set;}
    public Vector3 righthandPosition {get; private set;}

    public Collider lefthandCollider { get; private set; }
    public Collider righthandCollider { get; private set; }

    public GameObject lefthandSphere;
    public GameObject righthandSphere;

    public Camera sceneCamera; // Head's Data

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        lefthandCollider = lefthandSphere.GetComponent<Collider>();
        righthandCollider = righthandSphere.GetComponent<Collider>();
    }
    // Update is called once per frame
    void Update()
    {
        lefthandPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        righthandPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        lefthandSphere.transform.position = lefthandPosition;
        righthandSphere.transform.position = righthandPosition;
    }

    public Vector3 GetHeadPosition()
    {
        return sceneCamera.transform.position;
    }
}
