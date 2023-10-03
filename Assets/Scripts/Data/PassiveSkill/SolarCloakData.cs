using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SolarCloakData), menuName = "ScriptableObjects/PassiveSkills/" + nameof(SolarCloakData))]
public class SolarCloakData : PassiveSkillData
{
	[Header("HitBox")]
	[SerializeField] private Vector2 _hitBoxCenter;
	[SerializeField] private Vector2 _hitBoxSize;

	[SerializeField] private int _damage;
	[SerializeField] GameObject _defaultEffect;
	[SerializeField] private int _maxHp;


	public Vector2 HitBoxCenter => _hitBoxCenter;
	public Vector2 HitBoxSize => _hitBoxSize;
	public int Damage => _damage;
	public GameObject DefaultEffect => _defaultEffect;
	public int MaxHp => _maxHp;


}
