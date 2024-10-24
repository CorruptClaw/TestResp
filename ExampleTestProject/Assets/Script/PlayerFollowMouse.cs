using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFollowMouse : MonoBehaviour
{

    public float maxRotationAngle;
    public float rotationSpeed;
    public float frontLineLenght = 2f;

    private Vector2 mousePosition;
    private Camera mainCamera;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        mainCamera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {

        mousePosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane));
        Vector2 direction = mouseWorldPosition - transform.position;
        float currentAngle = transform.eulerAngles.z;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);
        float clampedAngle = Mathf.Clamp(angleDifference, -maxRotationAngle, maxRotationAngle);
        float newAngle = Mathf.LerpAngle(currentAngle, currentAngle + clampedAngle, Time.deltaTime * rotationSpeed);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, newAngle));

    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;

        Vector3 startPoint = transform.position;

        Vector3 pointDir = transform.right * frontLineLenght;

        Gizmos.DrawLine(startPoint, startPoint + pointDir);

    }

}
