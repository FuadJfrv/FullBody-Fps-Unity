using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	private CharacterController _characterController;

	[SerializeField] private float walkSpeed = 5f;
    
	[Space(25)]
	[SerializeField] private LayerMask groundedLayers;
	[SerializeField] private float groundedOffset;
	[SerializeField] private float groundedRadius;
	private float _verticalVelocity;
	private bool _grounded;

	private Animator _animator;
	private Vector2 _inputVector;

	
	private bool _punchCooledDown = true;
	private const float PunchCooldownAmount = 0.85f;
    
	private void Awake()
	{
		Application.targetFrameRate = 120;
		_characterController = GetComponent<CharacterController>();
		_animator = GetComponent<Animator>();
	}

	private void Update()
	{
		GroundedCheck();
		Gravity();
		Move(_inputVector); 
	}

	private void OnWalk(InputValue inputValue)
	{
		_inputVector = inputValue.Get<Vector2>();
	}


	private void Move(Vector2 input)
	{
		Vector3 movementVector = (transform.forward * input.y + transform.right * input.x).normalized;
		movementVector += Vector3.up * _verticalVelocity;
		_characterController.Move(movementVector * (Time.deltaTime * walkSpeed));

		_animator.SetBool("isWalking", Mathf.Abs(_characterController.velocity.magnitude) > 0.1f);
	}

	private void GroundedCheck()
	{
		Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
		_grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundedLayers, QueryTriggerInteraction.Ignore);
	}

	private void Gravity()
	{
		if (_grounded)
		{
			_verticalVelocity = 0;
		}
		else
		{
			_verticalVelocity += -9.81f * Time.deltaTime ;
		}

	}

	private void OnDrawGizmos()
	{
		Gizmos.color = _grounded ? Color.green : Color.red;
        
		Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundedRadius);
	}
	
	
	//Punching
	private void OnPunch()
	{
		if (!_punchCooledDown) return;
		int rand = Random.Range(0, 2);
		_animator.SetTrigger(rand == 0 ? "punchLeft" : "punchRight");
		StartCoroutine(DoPunchCooldown());
	}

	private IEnumerator DoPunchCooldown()
	{
		_punchCooledDown = false;
		yield return new WaitForSeconds(PunchCooldownAmount);
		_punchCooledDown = true;
	}
}
