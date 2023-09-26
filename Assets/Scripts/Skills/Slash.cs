using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : SkillBase, IActiveSkill
{
	private SlashData _data;

	public override void Init()
	{
		base.Init();

		_data = Managers.Resource.Load<SlashData>("Data/SlashData");

		Id                = _data.Id;
		Type              = _data.Type;
		Priority          = _data.Priority;
		IsRestrictMoving = _data.IsRestrictMoving;

		Cooldown          = _data.Cooldown;
		BeforeDelay       = _data.BeforeDelay;
		Duration          = _data.Duration;
		AfterDelay        = _data.AfterDelay;
		RequireMP		  = _data.RequireMP;
	}

	public override void Execute()
	{
		if (_data == null){ Debug.LogWarning($"Fail load Data/SlashData"); return;  }
		
		base.Execute();
		Owner.StartCoroutine(ExecuteCo());
	}

	IEnumerator ExecuteCo()
	{
		// 선딜
		yield return new WaitForSeconds(BeforeDelay);

		GameObject effect = null;
		// 애니메이션 재생
		if (_data.Effect != null)
		{
			effect = Managers.Resource.Instantiate("Skills/"+_data.Effect.name);
			effect.transform.position = Owner.transform.position;
		}

		// 실제 피해

		float x = Owner.transform.localScale.x < 0 ? -1 : 1;
		Vector2 centerInWorld = (Vector2)Owner.transform.position + new Vector2(x * _data.HitBoxCenter.x, _data.HitBoxCenter.y);
		var boxes = Physics2D.OverlapBoxAll(centerInWorld, _data.HitBoxSize, 0);
		foreach (var box in boxes)
		{
			if (!box.TryGetComponent<Character>(out var character) || character == Owner)
			{
				continue;
			}

			character.TakeDamage(1);
		}


		// 후딜
		yield return new WaitForSeconds(AfterDelay);

		Managers.Resource.Release(effect);
	}

	public override bool CheckCanUse()
	{
		bool isEnemyInBox = CheckEnemyInBox(_data.CheckBoxCenter, _data.CheckBoxSize);
		return isEnemyInBox;
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Vector3 hitboxPos = Owner.transform.position;
		// if ( gameObject != null){
		// 	hitboxPos = transform.position;
		// }

		//Debug.Log( $"OnDrawGizmos() | hitboxPos {hitboxPos}  hitBoxCenter {_data.HitBoxCenter}  hitBoxSize {_data.HitBoxSize} " );

		//Gizmos.DrawCube(hitboxPos + (Vector3)_data.HitBoxCenter, (Vector3)_data.HitBoxSize);
	}
}

