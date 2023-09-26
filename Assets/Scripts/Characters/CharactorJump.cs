using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharactorJump : MonoBehaviour
{
	public Vector3 velocity;
	
	CharacterGround _charaterGround;
	CharactorMovement _charactorMovement;
	CharactorMovementData _charactorMovementData;
	Rigidbody2D _body;

	bool _desiredJump;

	private void Awake()
	{
		_charaterGround = GetComponent<CharacterGround>();
		_charactorMovement = GetComponent<CharactorMovement>();
		_charactorMovementData = _charactorMovement.ChractorMovementData;
		_body = GetComponent<Rigidbody2D>();
	}

	public void OnJump(Vector2 lookDir)
	{
		if (_charaterGround.GetOnGround())
		{
			_body.AddForce(Vector2.up * _charactorMovementData.JumpPower, ForceMode2D.Impulse);
		}
	}

	private void setPhysics()
	{
		Vector2 newGravity = new Vector2(0, (-2 * _charactorMovementData.JumpHeight) / (_charactorMovementData.TimeToJumpApex * _charactorMovementData.TimeToJumpApex));
		_body.gravityScale = (newGravity.y / Physics2D.gravity.y) * _charactorMovementData.GravMultiplier;
	}

	void Update()
	{
		//test
		if (Input.GetKeyDown(KeyCode.Space))
		{
			OnJump(Vector2.zero);
		}
	}

}
