using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class UISkillSlot
{
	private const float Duration = 0.07f;
	
	private Coroutine _scaleHandler;
	
	public bool IsEnabled { get; private set; }

	public void ShowChoiceEffect()
	{
		Vector3 scale = transform.localScale;
		Vector3 targetScale = Vector3.one * 0.95f;
		System.Action callback = () => _scaleHandler = Utility.Lerp(targetScale, scale, Duration, vector2 => transform.localScale = vector2);
		_scaleHandler = Utility.Lerp(scale, targetScale, Duration, vector => transform.localScale = vector, callback);
	}

	public void Disable()
	{
		Utility.StopCoroutine(_scaleHandler);
		IsEnabled = false;
		Get<Image>((int)Elements.Dim).gameObject.SetActive(true);
	}

	public void Select()
	{
		if (_scaleHandler != null)
		{
			Managers.Instance.StopCoroutine(_scaleHandler);
			_scaleHandler = null;
		}

		if (_border != null)
		{
			_border.color = _selectedColor;
		}
		_scaleHandler = Utility.Lerp(Vector3.one, Vector3.one * s_SelectedScale, 0.1f, vector =>
		{
			try
			{
				transform.localScale = vector;
			}
			catch
			{
				// ignored
			}
		});
	}

	public void Unselect()
	{
		if (_scaleHandler != null)
		{
			Managers.Instance.StopCoroutine(_scaleHandler);
			_scaleHandler = null;
		}

		if (_border != null)
		{
			_border.color = s_UnselectedColor;
		}
		transform.localScale = Vector3.one;
	}
}
