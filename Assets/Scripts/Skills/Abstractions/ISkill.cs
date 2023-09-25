using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
	/// <summary>
	/// ��ų�� ������ ĳ����
	/// </summary>
	Character Owner { get; set; }

	/// <summary>
	/// ��ų�� Ÿ��
	/// </summary>
	SkillType Type { get; }

	/// <summary>
	/// ��ų�� �켱����
	/// </summary>
	int Priority { get; }

	/// <summary>
	/// ��ų�� ĳ������ �������� �����ϴ��� ����
	/// </summary>
	bool IsRestricteMoving { get; }

	/// <summary>
	/// ��ų�� ��Ÿ��
	/// </summary>
	float Cooldown { get; }

	/// <summary>
	/// ��ų�� �� ������
	/// </summary>
	float BeforeDelay { get; }

	/// <summary>
	/// ��ų�� ��� �ð�
	/// </summary>
	float Duration { get; }

	/// <summary>
	/// ��ų�� �� ������
	/// </summary>
	float AfterDelay { get; }

	/// <summary>
	/// ��ų �ʱ�ȭ �� ȣ��
	/// </summary>
	void Init();

	/// <summary>
	/// ��ų�� ����� �� �ִ��� ���θ� ��ȯ
	/// </summary>
	/// <returns>��ų�� ����� �� �ִ��� ����</returns>
	bool CheckCanUse();

	/// <summary>
	/// ��ų�� ����
	/// </summary>
	void Execute();

	/// <summary>
	/// ����� �׸�
	/// </summary>
	/// <param name="character"></param>
	void OnDrawGizmos(Transform character);
}