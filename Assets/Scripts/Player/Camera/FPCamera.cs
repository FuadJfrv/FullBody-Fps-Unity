using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class FPCamera : MonoBehaviour
{
	private Transform _playerBody;

	[SerializeField] private float mouseSensitivity = 1;

	[SerializeField] private SharedFloat cameraXRotation;
	private float _xRotation;
	private float _yRotation;

	private const float MaxDownwardRotation = 80f;
	private const float MaxUpwardRotation = -80f;

	private float _inputX;
	private float _inputY;
	void Awake()
	{
		_playerBody = FindObjectOfType<PlayerController>().transform;
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		DoFpsCamera();
	}

	private void OnLookX(InputValue input)
	{
		_inputX = input.Get<float>();
	}

	private void OnLookY(InputValue input)
	{
		_inputY = input.Get<float>();
	}

	private void DoFpsCamera()
	{
		float mouseX = _inputX * mouseSensitivity;
		float mouseY = _inputY * mouseSensitivity;

		_xRotation -= mouseY;
		_xRotation = Mathf.Clamp(_xRotation, MaxUpwardRotation, MaxDownwardRotation);
		transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f); //euler gets converted to quaternion

		
		_yRotation += mouseX;
		_playerBody.rotation = Quaternion.Euler(0f, _yRotation, 0f);

		cameraXRotation.Value = _xRotation;
	}

	private void OnEnable()
	{
		_yRotation = _playerBody.eulerAngles.y;
	}

	public void ClearRotations()
	{
		_xRotation = 0;
		cameraXRotation.Value = 0;
		//_yRotation = 0;
	}
}
