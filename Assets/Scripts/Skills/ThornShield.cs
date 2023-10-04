using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornShield : PassiveSkillBase
{
	ThornShieldData _data;

	public int Damage { get; protected set; }

	public override void Init()
	{
		base.Init();

		_data = LoadData<ThornShieldData>();

		Managers.Stat.onTakeDamage += Execute;

		Managers.Stat.AddMaxHp(Owner.playerIndex, _data.MaxHp);
	}

	public override void Reset()
	{
		base.Reset();
		
		Damage = (int)MathF.Ceiling(Managers.Stat.GetMaxHp(Owner.playerIndex) * 0.04f);
		PresentNumber = Damage;
	}

	void Execute(int playerIndex, int damage)
	{
		if(Owner.playerIndex == playerIndex)
		{
			float x = Owner.Target.transform.position.x - Owner.transform.position.x >= 0 ? 1 : -1;

			IsEnabled = true;
			PlayEffect(_data.Effect.name, 1, x, Vector2.right * 3f);
			Managers.Stat.GiveDamage(1 - Owner.playerIndex, Damage);
		}
	}
}
