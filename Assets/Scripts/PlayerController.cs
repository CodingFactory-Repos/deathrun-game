using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SocketIOClient;

public class PlayerMovement : MonoBehaviour
{
    private SocketIOUnity clientSocket;
    private Vector2 movementInput;
    public float moveSpeed = 5f;
    public float dashSpeed = 8f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 5f;

    private bool isDashing = false;
    private float dashTimeLeft = 0f;
    private float dashCooldownTimeLeft = 0f;

    public Camera mainCamera;
    public Vector3 cameraOffset = new Vector3(0, 0, -10);
    public Animator animator;

    private Vector2 previousMovement = Vector2.zero;

    async void Start()
    {
        try
        {
            var uri = new Uri("http://localhost:11100");
            clientSocket = new SocketIOUnity(uri, new SocketIOOptions
            {
                Query = new Dictionary<string, string>
                {
                    { "token", "UNITY" }
                },
                Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
            });

            await clientSocket.ConnectAsync();

            animator = GetComponent<Animator>();
        }
        catch (Exception e)
        {
            Debug.Log("Socket connection error: " + e.Message);
        }
    }

    void Update()
    {
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimeLeft <= 0)
        {
            StartDash();
        }

        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                EndDash();
            }
        }
        else if (dashCooldownTimeLeft > 0)
        {
            dashCooldownTimeLeft -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (movementInput != Vector2.zero)
        {
            MovePlayer(movementInput);
        }
    }

    private void LateUpdate()
    {
        UpdateCameraPosition();  // Move to LateUpdate for better camera syncing
    }

    void MovePlayer(Vector2 movement)
    {
        float currentSpeed = isDashing ? dashSpeed : moveSpeed;
        movement.x = (float)Math.Round(movement.x, 2);
        movement.y = (float)Math.Round(movement.y, 2);

        Vector3 movementVector = new Vector3(movement.x, movement.y, 0f) * currentSpeed * Time.fixedDeltaTime;
        transform.position += movementVector;

        Debug.Log("Player is moving to position: " + transform.position);
        SendData("playerMovement", transform.position.ToString());

        if (animator != null)
        {
            UpdateAnimatorParameters(movement, previousMovement);
        }

        previousMovement = movement;
    }

    void UpdateAnimatorParameters(Vector2 movement, Vector2 previousMovement)
    {
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
    }

    void UpdateCameraPosition()
    {
        if (mainCamera != null)
        {
            mainCamera.transform.position = transform.position + cameraOffset;
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        dashCooldownTimeLeft = dashCooldown;
    }

    void EndDash()
    {
        isDashing = false;
        Debug.Log("Dash ended");
    }

    async void SendData(string channel, string data)
    {
        try
        {
            await clientSocket.EmitAsync("playerMovement", data);
            Debug.Log("Data sent: " + $"{channel}|{data}");
        }
        catch (Exception e)
        {
            Debug.Log("Socket send error: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        if (clientSocket != null)
        {
            clientSocket.Dispose();  // Properly close the connection
        }
    }
}
