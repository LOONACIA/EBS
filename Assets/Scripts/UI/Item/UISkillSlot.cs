using UnityEngine;
using UnityEngine.UI;

public partial class UISkillSlot : UIBase
{
	private enum Elements
	{
		Icon,
		Dim,
		CooldownIndicator
	}

	private static readonly Color s_SelectedColor = new(1, 0, 0, 0.5f);

	private static readonly Color s_UnselectedColor = new(1, 1, 1, 0.5f);

	private static readonly float s_SelectedScale = 1.1f;

	private SkillInfo _info;

	private ISkill _skill;

	private Image _border;

	public SkillInfo SkillInfo => _info;

	protected override void Awake()
	{
		base.Awake();

		_border = GetComponent<Image>();
	}

	private void Update()
	{
		if (_skill != null)
		{
			CheckCooldown();
		}
	}

	private void OnEnable()
	{
		IsEnabled = true;
		if (_border != null)
		{
			_border.color = s_UnselectedColor;
		}
		Get<Image>((int)Elements.Dim).gameObject.SetActive(false);
	}

	public override void Init()
	{
		Bind<Image, Elements>();

		if (_info != null)
		{
			SetSkillInfo();
		}
	}

	public void SetSkill(ISkill skill)
	{
		_skill = skill;
	}

	public void SetInfo(SkillInfo info)
	{
		_info = info;
		SetSkillInfo();
	}

	private void SetSkillInfo()
	{
		var icon = Get<Image>((int)Elements.Icon);
		if (_info != null)
		{
			icon.sprite = _info.Sprite;
		}
	}
}
