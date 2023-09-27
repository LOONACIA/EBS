using UnityEngine;

public class ActiveSkillData : ScriptableSkillData
{
	[SerializeField] private int _priority;
	[SerializeField] private SkillType _type;
	[SerializeField] private bool _isRestrictMoving;
	[SerializeField] private float _beforeDelay;
	[SerializeField] private float _afterDelay;
	[SerializeField] private int _requireMP;


	[Header("OverlapBox")]
	[Header("Check")]
	[SerializeField] private Vector2 _checkBoxCenter;
	[SerializeField] private Vector2 _checkBoxSize;

	[Header("HitBox")]
	[SerializeField] private Vector2 _hitBoxCenter;
	[SerializeField] private Vector2 _hitBoxSize;

	[SerializeField] private Vector2 _offset;

	
	[SerializeField] private int _damage;


	public int Priority => _priority;
	public SkillType Type => _type;
	public bool IsRestrictMoving => _isRestrictMoving;
	public float BeforeDelay => _beforeDelay;
	public float AfterDelay => _afterDelay;
	public int RequireMP => _requireMP;


	public Vector2 CheckBoxCenter => _checkBoxCenter;
	public Vector2 CheckBoxSize => _checkBoxSize;

	public Vector2 HitBoxCenter => _hitBoxCenter;
	public Vector2 HitBoxSize => _hitBoxSize;
	public Vector2 Offset => _offset;

	
	public int Damage => _damage;

}
