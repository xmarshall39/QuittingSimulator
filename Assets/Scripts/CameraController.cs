using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 defaultLocation;
    Vector3 defaultRotation;
    float currentProgress;


    public GameObject target;

    [Range(0, 1)]
    public float defaultProgress;

    public float zoomSpeed;
    public float rotationStrength;
    public float minDistanceFromTarget = 2f;
    public float maxDistanceFromTarget = 30f;

    static void RotateAround(Transform transform, Vector3 pivotPoint, Vector3 axis, float angle)
    {
        Quaternion rot = Quaternion.AngleAxis(angle, axis);
        transform.position = rot * (transform.position - pivotPoint) + pivotPoint;
        transform.rotation = rot * transform.rotation;
    }

    void Start()
    {
        defaultLocation = transform.position;
        defaultRotation = transform.rotation.eulerAngles;
        currentProgress = defaultProgress;
        UpdateZoom();
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            AddZoom(-Input.GetAxis("Mouse ScrollWheel"));
        }

        if (Input.GetMouseButton(0))
        {
#if UNITY_WEBGL
            float rotateX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationStrength * .15f;
#else
            float rotateX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationStrength;
#endif
            CameraController.RotateAround(transform, target.transform.position, transform.up, rotateX);
#if UNITY_WEBGL
            float rotateY = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotationStrength * 0.10f;
#else
            float rotateY = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotationStrength * 0.75f;
#endif
            CameraController.RotateAround(transform, target.transform.position, transform.right, rotateY);
        }

        transform.LookAt(target.transform);
    }

    void AddZoom(float amount)
    {
        float delta = Time.deltaTime * amount * zoomSpeed;
        currentProgress += delta;
        currentProgress = Mathf.Clamp01(currentProgress);
        UpdateZoom();
    }

    void UpdateZoom()
    {
        Vector3 targetToCamNorm = (transform.position - target.transform.position).normalized;

        Vector3 closestPointToTarget = (targetToCamNorm * minDistanceFromTarget) + target.transform.position;
        Vector3 farthestPointFromTarget = (targetToCamNorm * maxDistanceFromTarget) + target.transform.position;

        Vector3 progressPosition = Vector3.Lerp(closestPointToTarget, farthestPointFromTarget, currentProgress);
        transform.position = progressPosition;
    }
}
