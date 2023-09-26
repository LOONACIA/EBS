using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CharactorMovement : MonoBehaviour
{
	[SerializeField]
	public CharactorMovementData ChractorMovementData;

	public Vector2 LookDirction;

	public Vector2 PlayerInput { 
		get { return _playerInput; }
		set
		{
			_playerInput = Vector2.ClampMagnitude(value, 1f);
		} 
	}
	Vector2 _playerInput;

	Vector3 _velocity;

	private void FixedUpdate()
	{
		Vector3 desiredVelocity = new Vector3(PlayerInput.x, 0, 0) * ChractorMovementData.MaxSpeed;
		float maxSpeedChange = ChractorMovementData.MaxAcceleration * Time.deltaTime;

		_velocity.x = Mathf.MoveTowards(_velocity.x, desiredVelocity.x, maxSpeedChange);

		Vector3 displacement = _velocity * Time.deltaTime;
		transform.localPosition += displacement;

		LookMoveDirction(displacement);
	}

	private void LookMoveDirction(Vector3 moveDirction)
	{
		LookDirction = new Vector2(Mathf.Abs(transform.localScale.x) * (moveDirction.x > 0 ? 1 : -1), 0) ; 
		transform.localScale = new Vector3(LookDirction.x, transform.localScale.y, transform.localScale.z);
	}
}
