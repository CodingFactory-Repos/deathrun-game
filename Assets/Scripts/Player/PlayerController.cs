using System;
using UnityEngine;
using SocketIOClient;
using CandyCoded.env;
using System.IO;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
	private Vector2 movementInput;
	public float moveSpeed = 5f;
	public float dashSpeed = 8f;
	public float dashDuration = 0.2f;
	public float dashCooldown = 5f;

	private bool isDashing = false;
	private float dashTimeLeft = 0f;
	private float dashCooldownTimeLeft = 0f;

	private Camera mainCamera;
	public Vector3 cameraOffset = new Vector3(0, 0, -10);
	public Animator animator;

	private Vector2 previousMovement = Vector2.zero;
	private Player player;
	private float cameraHeight;
	private float cameraWidth;
	public Vector2 minCameraPosition;
	public Vector2 maxCameraPosition;
	private string socketUrl;
	private SocketIOUnity clientSocket;

	async void Start()
	{

		clientSocket = SocketManager.Instance.ClientSocket;
		player = GetComponent<Player>();
		if (player == null)
		{
			Debug.LogError("Player component missing!");
		}

		animator = GetComponent<Animator>();
		if (animator == null)
		{
			Debug.LogError("Animator component missing on the player!");
		}

		mainCamera = Camera.main;
		if (mainCamera == null)
		{
			Debug.LogError("No Main Camera found in the scene. Please ensure a camera is tagged as 'MainCamera'.");
		}
		else
		{
			cameraHeight = 2f * mainCamera.orthographicSize;
			cameraWidth = cameraHeight * mainCamera.aspect;
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
		UpdateCameraPosition();
	}

	void MovePlayer(Vector2 movement)
	{
		float currentSpeed = isDashing ? dashSpeed : moveSpeed;
		movement.x = (float)Math.Round(movement.x, 2);
		movement.y = (float)Math.Round(movement.y, 2);

		Vector3 movementVector = new Vector3(movement.x, movement.y, 0f) * currentSpeed * Time.fixedDeltaTime;
		transform.position += movementVector;

		SendData("players:move", transform.position.ToString());

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
			Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);

			Vector3 targetCameraPosition = new Vector3(mainCamera.transform.position.x, playerPosition.y + cameraOffset.y, mainCamera.transform.position.z);
			targetCameraPosition.y = Mathf.Clamp(targetCameraPosition.y, minCameraPosition.y + cameraHeight / 2, maxCameraPosition.y - cameraHeight / 2);

			mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPosition, Time.deltaTime * 5f);
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
			if (clientSocket != null)
			{
				await clientSocket.EmitAsync(channel, data);
			}

		}
		catch (Exception e)
		{
			UnityEngine.Debug.LogError("Socket send error: " + e.Message);
		}
	}

	void OnApplicationQuit()
	{
		if (clientSocket != null)
		{
			clientSocket.Dispose();
		}
	}
}
