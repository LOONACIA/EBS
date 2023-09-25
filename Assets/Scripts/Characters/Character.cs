using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
	public List<ISkill> CanUseSkills = new List<ISkill>();
	public ISkill CurrentSkill;

	[SerializeField]
	private int _hp;

	[SerializeField]
	private GameObject _target;

	private BehaviorTree _moveBehavior;

	private List<ISkill> _skills = new()
	{
		//new Slash(),
		new DummyFireballSkill(),
		new DummyFireballSkill1(),
		new DummyFireballSkill2(),
	};

	private bool _hasCooldowmSkill;
	private bool _canMove;

	private void Awake()
	{
		_moveBehavior = GetComponent<BehaviorTree>();
	}

	private void Start()
	{
		StartCoroutine(CheckSkills());

		// test
		foreach (var skill in _skills)
		{ 
			skill.Owner = this;	
		}
	}

	private void Update()
	{
		SetMoveBTVariables();

		if (Input.GetKeyDown(KeyCode.Space))
		{
			CurrentSkill = _skills.FirstOrDefault();
			CurrentSkill.Init();
			CurrentSkill.Owner = this;
			CurrentSkill.Execute();
		}
	}

	//private void OnDrawGizmos()
	//{
	//	foreach (var skill in _skills)
	//	{
	//		skill.OnDrawGizmos(transform);
	//	}
	//}

	public void UseSkill()
	{
		
	}

	public void TakeDamage(int damage)
	{
		_hp -= damage;
	}

	private IEnumerator CheckSkills()
	{
		while (true)
		{
			CanUseSkills.Clear();
			_hasCooldowmSkill = false;

			foreach (var skill in _skills)
			{
				// TODO : 쿨다운 체크
				if (skill.IsCoolReady)
					_hasCooldowmSkill = true;

				if (skill.CheckCanUse() && CanUseSkills.Contains(skill) == false)
					CanUseSkills.Add(skill);

				if (CurrentSkill != null && CurrentSkill.IsActing == false)
					CurrentSkill = null;
			}

			yield return new WaitForSeconds(0.2f);
		}
	}

	private void SetMoveBTVariables()
	{
		var direction = _target.transform.position - transform.position;
		var distance = direction.magnitude;

		if (CanUseSkills != null && CanUseSkills.Count > 0)
			_moveBehavior.SetVariableValue("CanUseSkill", true);
		else
			_moveBehavior.SetVariableValue("CanUseSkill", false);

		if (CurrentSkill != null)
		{
			_moveBehavior.SetVariableValue("IsActing", true);


			if (CurrentSkill.IsRestricteMoving == true)
				_moveBehavior.SetVariableValue("CanMove", false);
			else
				_moveBehavior.SetVariableValue("CanMove", true);
		}
		else
		{ 
			_moveBehavior.SetVariableValue("IsActing", false);
			_moveBehavior.SetVariableValue("CanMove", true);
		}

		_moveBehavior.SetVariableValue("Direction", direction);
		_moveBehavior.SetVariableValue("Distance", distance);
		_moveBehavior.SetVariableValue("HasCooldownSkill", _hasCooldowmSkill);
	}

	public List<ISkill> GetHighPrioritySkill()
	{

		int highPriority = int.MaxValue;
		List<ISkill> _tmpSkills = new List<ISkill>();
		
		foreach( ISkill skill in CanUseSkills)
		{
			if (skill.Priority < highPriority)
			{
				highPriority = skill.Priority;
			}
		}

		foreach( ISkill skill in CanUseSkills)
		{
			if (skill.Priority == highPriority)
			{
				_tmpSkills.Add(skill);
			}
		}
		return _tmpSkills;
	}
}
