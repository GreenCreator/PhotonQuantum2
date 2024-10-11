using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    [Header("Movement Settings")] [SerializeField]
    private float rotationSpeed = 720f;

    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float cameraDistance = 5f;
    [SerializeField] private float cameraHeight = 2f;

    private PlayerStats playerStats;
    private Transform cameraTransform;
    private float pitch;
    private float yaw;
    private Rigidbody rb;
    private bool cursorVisible = true;

    private Dictionary<Enemy, float> attackTimers = new Dictionary<Enemy, float>();

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (photonView.IsMine)
        {
            cameraTransform = Camera.main.transform;
            ToggleCursorVisibility();
        }
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            HandleMovement();
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            HandleCamera();
            if (Input.GetKeyDown(KeyCode.F2))
            {
                ToggleCursorVisibility();
            }
        }
    }

    private void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch = Mathf.Clamp(pitch - mouseY, -30f, 60f);

        Vector3 cameraOffset = new Vector3(0, cameraHeight, -cameraDistance);
        Quaternion cameraRotation = Quaternion.Euler(pitch, yaw, 0f);
        cameraTransform.position = transform.position + cameraRotation * cameraOffset;
        cameraTransform.LookAt(transform.position + Vector3.up * cameraHeight);
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg +
                                cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.MovePosition(transform.position + moveDir.normalized * playerStats.Speed * Time.deltaTime);
        }
    }

    private void ToggleCursorVisibility()
    {
        cursorVisible = !cursorVisible;
        Cursor.lockState = cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = cursorVisible;
    }
}