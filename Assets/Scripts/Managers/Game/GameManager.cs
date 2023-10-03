using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region public Variables
    public static GameManager Instance;
    public enum GameState
    {
        Title,
        PickSkill,
        PreRound,
        Battle,
        RoundOver,
        GameOver
    }
    public GameState State { get; private set; }
    public int CurrentRound { get; private set; } = 1;
	//public Player Player1 { get; private set; }
	//public Player Player2 { get; private set; }
	public bool IsPlayer1Win { get; set; } = true;

	public int Player1Life
	{
		get
		{
			return _player1Life;
		}
		private set
		{
			_player1Life = value;
			player1LifeTxt.text = "LIFE : " + _player1Life;
			if (value < 0)
			{
				_isPlayer1Defeat = true;
				ChangeState(GameState.GameOver);
			}
		}
	}
	public int Player2Life
	{
		get
		{
			return _player2Life;
		}
		private set
		{
			_player2Life = value;
			player2LifeTxt.text = "LIFE : " + _player2Life;
			if (value < 0)
			{
				_isPlayer1Defeat = false;
				ChangeState(GameState.GameOver);
			}
		}
	}

	public static SkillManager Skill => Instance._skill;
	
	public static GameUIManager UI => Instance._ui;
	#endregion


	#region private Variables
	private Character _roundWinner;
	
	[SerializeField]
	private SkillSelectorInput _player1Input;
	
	[SerializeField]
	private SkillSelectorInput _player2Input;

	private SkillSelector _selector;
	private GameUIManager _ui = new();
	private Character _currentPicker;
	[SerializeField]
	private List<int> _firstPickCountList;
	[SerializeField]
	private List<int> _otherPickCountList;
	private int _pickCount;
	private int _pickCountIndex;

	private SkillManager _skill = new();
	private int _skillPickCount = 9;

	[SerializeField] private int totalRounds;
	private Character player1;
	private Character player2;
	[SerializeField] private Transform[] spawnPoints = new Transform[2];
	private int _player1Life;
	private int _player2Life;
	//private int[] roundDamage = {0, 0, 4, 8, 12, 20, 30, 30, 30, 30};
	private bool _isPlayer1Defeat = false;
	private bool _isPlayer1Pick = false;
	int _player1RoundHP;
	int _player2RoundHP;

	int Player1RoundHP
	{
		get
		{
			return _player1RoundHP;
		}
		set
		{

		}
	}
	int Player2RoundHP
	{
		get
		{
			return _player2RoundHP;
		}
		set
		{

		}
	}

	GameObject canvas;
	public Slider player1RoundHPUI;
	public Slider player2RoundHPUI;
	TextMeshProUGUI roundText;
	TextMeshProUGUI timerText;
	TextMeshProUGUI player1LifeTxt;
	TextMeshProUGUI player2LifeTxt;

	float timer;

	#endregion


	#region public Method
	public void ChangeState(GameState _state)
    {
        State = _state;
        switch (_state)
        {
            case GameState.Title:
                OnTitle();
                break;
            case GameState.PickSkill:
                OnPickSkill();
                break;
            case GameState.PreRound:
                OnPreRound();
                break;
            case GameState.Battle:
                OnBattle();
                break;
            case GameState.RoundOver:
                OnRoundOver();
                break;
            case GameState.GameOver:
                OnGameOver();
                break;
        }
    }

	public void SetRoundWinner(Character winner)
	{
		_roundWinner = winner;
		IsPlayer1Win = _roundWinner == player1;
	}

	public void ShowRoundWinnerUI(float closeTime)
	{
		if (State != GameState.GameOver)
		{
			var winnerUI = Managers.UI.ShowPopupUI<UIRoundWinner>();
			winnerUI.SetWinner(_roundWinner);

			IEnumerator WaitUIClose()
			{
				yield return new WaitForSeconds(closeTime);
				Managers.UI.ClosePopupUI(winnerUI);
			}

			StartCoroutine(WaitUIClose());
		}
	}

	#endregion


	#region private Method
	private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
		#endregion

		_skill.Init();

		PreparePlayer();

		SetBattleUI();
        ChangeState(GameState.Title);
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			_ui.ShowMenu(
				exit: () =>
				{
#if UNITY_EDITOR
					UnityEditor.EditorApplication.isPlaying = false;
#else
					Application.Quit(),
#endif
				}
				);
		}
		
		if(State == GameState.Battle)
		{
			timer -= Time.deltaTime;
			timerText.text = (int)timer + "";
			if (timer <= 0)
			{
				ChangeState(GameState.RoundOver);
				timer = 0;
			}
		}

		
	}

	void SetBattleUI()
	{
		canvas = GameObject.Find("BattleCanvas");
		player1RoundHPUI = GameObject.Find("Player1RoundHP").GetComponent<Slider>();
		player2RoundHPUI = GameObject.Find("Player2RoundHP").GetComponent<Slider>();
		roundText = GameObject.Find("RoundTxt").GetComponent<TextMeshProUGUI>();
		timerText = GameObject.Find("TimerTxt").GetComponent<TextMeshProUGUI>();
		player1LifeTxt = GameObject.Find("Player1LifeTxt").GetComponent<TextMeshProUGUI>();
		player2LifeTxt = GameObject.Find("Player2LifeTxt").GetComponent<TextMeshProUGUI>();
		Player1Life = 10;
		Player2Life = 10;


		canvas.SetActive(false);
	}

	public void StartGame()
	{
		CurrentRound = 1;
		_skill.Init();
		_skill.SetCharacters(player1, player2);
		canvas.SetActive(true);
		ChangeState(GameState.PickSkill);
	}

	private void PreparePlayer()
	{
		player1 = GameObject.Find("Player 1").GetOrAddComponent<Character>();
		player2 = GameObject.Find("Player 2").GetOrAddComponent<Character>();
		
		_ui.SetSkillPresenter(player1);
		_ui.SetSkillPresenter(player2);

		_ui.ShowSkillList(player1, player2);
	}

	private void OnTitle()
    {
        // something must do at title
        InitPlayerStartingPoint();
		_ui.ShowTitle(StartGame);
    }

    private void OnPickSkill()
    {
	    canvas.SetActive(false);
	    InitPlayerStartingPoint();
	    
	    // TODO: 라운드별 승자 처리
	    Character winner = _roundWinner;
	    Character loser = winner == player1 ? player2 : player1;
		// if round1, player1 is first
		// else, last round's loser is first
		_currentPicker = CurrentRound == 1 ? player1 : loser;
		_isPlayer1Pick = CurrentRound == 1 || loser == player1;

		var list = CurrentRound == 1 ? _firstPickCountList : _otherPickCountList;
		_pickCountIndex = 0;
		_pickCount = list[_pickCountIndex];

		var skillPool = _skill.GeneratePool(_skillPickCount);
		_selector = new(skillPool)
		{
			Input = _isPlayer1Pick ? _player1Input : _player2Input
		};
		_selector.SkillSelected += PickSkill;
		_ui.ShowSkillSelector(_selector);
	}

	private Character GetWinner()
	{
		// TODO: 승자 처리
		return player1;
	}

	private void PickSkill(SkillInfo skillInfo)
	{
		if (!_skill.TryFindSkillTypeById(skillInfo.Id, out var skillType))
		{
			Debug.LogError($"Undefined skill type. ID: {skillInfo.Id}, Name: {skillInfo.Name}");
			return;
		}

		if (_currentPicker.gameObject.AddComponent(skillType) is not ISkill skill)
		{
			Debug.LogError($"Can't add skill to {_currentPicker}. Id: {skillInfo.Id}, Name: {skillInfo.Name}");
			return;
		}
		
		skill.Init();
		skill.Init(_currentPicker);
		_currentPicker.AddSkill(skill);
		if (--_pickCount > 0 && _selector.CanSelect)
		{
			return;
		}

		_currentPicker = _isPlayer1Pick ? player2 : player1;
		_selector.Input = _isPlayer1Pick ? _player2Input : _player1Input;
		_isPlayer1Pick = !_isPlayer1Pick;

		var list = CurrentRound == 1 ? _firstPickCountList : _otherPickCountList;
		if (_pickCountIndex < list.Count - 1 && _selector.CanSelect)
		{
			_pickCount = list[++_pickCountIndex];
		}
		else
		{
			// pick end
			_ui.HideSkillSelector();
			_selector.SkillSelected -= PickSkill;
			_selector = null;
			ChangeState(GameState.PreRound);
		}
	}

	private void OnPreRound()
    {
		// reset something
		canvas.SetActive(true);

		// do something

		// ui refresh??
		roundText.text = "Round" + CurrentRound;
		timerText.text = (int)timer + ""; 

        ChangeState(GameState.Battle);
    }

    private void OnBattle()
    {
		// change something
		timer = 99f;
		StartBattle();
    }

    private void OnRoundOver()
    {
	    InitPlayerStartingPoint();
		CalculateRoundDamage();

		// reset something

		player1RoundHPUI.value = 100 + CurrentRound * 20;
		player2RoundHPUI.value = 100 + CurrentRound * 20;

		CurrentRound++;

		ChangeState(GameState.PickSkill);
	}


	private void OnGameOver()
    {
	    var winner = _isPlayer1Defeat ? player2 : player1;
	    var gameEndUI = Managers.UI.ShowPopupUI<UIGameEnd>();
	    gameEndUI.SetWinner(winner);
        // winner ui??
		//if (_isPlayer1Defeat)
		{
			// player2 win
		}
		//else
		{
			// player1 win
		}
        // idea : how about save dealing amount of each skills, for each player skills they have
        
        // go to title, or quit <= maybe uimanager should do
    }

	private void InitPlayerStartingPoint()
	{
		player1.gameObject.SetActive(true);
		player2.gameObject.SetActive(true);

		player1.GetComponent<BehaviorTree>().enabled = false;
		player2.GetComponent<BehaviorTree>().enabled = false;

		player1.GetComponent<CharacterMovement>().PlayerInput = Vector2.zero;
		player2.GetComponent<CharacterMovement>().PlayerInput = Vector2.zero;
		foreach (var skill in player1.Skills)
		{
			skill.Init();
		}
		
		foreach (var skill in player2.Skills)
		{
			skill.Init();
		}
		
		player1.transform.position = spawnPoints[0].position;
		player2.transform.position = spawnPoints[1].position;

		// maybe ui guide

		// turn on ai
		Managers.Stat.SoftResetStats();
	}
	
	private void StartBattle()
	{
		player1.BehaviorTree.enabled = true;
		player2.BehaviorTree.enabled = true;
	}

	private void CalculateRoundDamage()
	{
		if (IsPlayer1Win)
		{
			Player2Life -= Mathf.Max(CurrentRound - 1, 1); 
		}
		else
		{
			Player1Life -= Mathf.Max(CurrentRound - 1, 1); 
		}

		
	}

	private void OnValidate()
	{
		if (spawnPoints?.Any() is not true)
		{
			Debug.LogWarning($"{nameof(spawnPoints)} is not assigned.");
		}
	}
	#endregion
}
