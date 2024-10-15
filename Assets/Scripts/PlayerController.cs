using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Socket clientSocket;
    private byte[] buffer = new byte[1024];

    private Vector2 movementInput;
    public float moveSpeed = 10f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private bool isDashing = false;
    private float dashTimeLeft = 0f;
    private float dashCooldownTimeLeft = 0f;

    public Camera mainCamera;
    public Vector3 cameraOffset = new Vector3(0, 0, -10);

    void Start()
    {
        try
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect("127.0.0.1", 12345);
            Debug.Log("Connected to server");

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }
        catch (SocketException e)
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
            UpdateCameraPosition();
        }
    }

    void MovePlayer(Vector2 movement)
    {
        float currentSpeed = isDashing ? dashSpeed : moveSpeed;
        Vector3 movementVector = new Vector3(movement.x, movement.y, 0f) * currentSpeed * Time.fixedDeltaTime;
        transform.position += movementVector;

        Debug.Log("Player is moving to position: " + transform.position);
        SendData(transform.position.ToString());
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
        Debug.Log("Dash started");
    }

    void EndDash()
    {
        isDashing = false;
        Debug.Log("Dash ended");
    }

    void SendData(string data)
    {
        try
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            clientSocket.Send(dataBytes);
            Debug.Log("Data sent: " + data);
        }
        catch (SocketException e)
        {
            Debug.Log("Socket send error: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        if (clientSocket != null && clientSocket.Connected)
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            Debug.Log("Disconnected from server");
        }
    }
}
