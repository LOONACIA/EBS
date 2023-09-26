using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable, CreateAssetMenu(fileName = nameof(CharactorMovementData), menuName = "ScriptableObjects/Charactors/" + nameof(CharactorMovementData))]
public class CharactorMovementData : ScriptableObject
{
	[Header ("Movement")]
	[SerializeField, Range(0f, 100f)]
	float _maxSpeed = 10f;

	[SerializeField, Range(0f, 100f)]
	float _maxAcceleration = 10f;


	[Header("Jump")]
	[SerializeField, Range(0f, 100f)]
	float _jumpHeight = 10f;

	[SerializeField, Range(0.2f, 1.25f)]
	float _timeToJumpApex;

	[SerializeField]
	public float _gravMultiplier;


	public float MaxSpeed => _maxSpeed;
	public float MaxAcceleration => _maxAcceleration;
	public float JumpHeight => _jumpHeight;
	public float TimeToJumpApex => _timeToJumpApex;
	public float GravMultiplier => _gravMultiplier;
}
