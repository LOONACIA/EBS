using UnityEngine;

public class HolyBarrier : PassiveSkillBase
{
    private HolyBarrierData _data;

    bool effectOn;
    GameObject effect;

	public override void Init()
	{
		base.Init();
        _data = LoadData<HolyBarrierData>();

        CurrentCooldown = 0;
	}

  
    private void Update() {
        if( IsEnabled ) return;

        if( CurrentCooldown < Cooldown ){
            CurrentCooldown += Time.deltaTime;
            return;
        }

        EnableHolyBarrier();
    }


    private void EnableHolyBarrier()
    {
        OnEffect();
        IsEnabled = true;

        //Managers.Stat.isYinYangCore[Owner.playerIndex] = true;
        // TODO: Stat에서 콜
    }

    
    public void DisableHolyBarrier() // Call by statmanager Function after Skill Use.
    {
        CurrentCooldown = 0;

        OffEffect();
        IsEnabled = false;
    }


    private void OnEffect()
    {
        if(_data.Effect == null){ return; }

        effectOn = true;

        effect = Managers.Resource.Instantiate("Skills/"+_data.Effect.name, Owner.transform ); // paraent를 character.gameObject로
        
        if(effect){
            effect.transform.localPosition = Vector3.zero;
        }else{
            Debug.LogError($"effect is null. effName :{_data.Effect.name}");
        }

        effect.transform.localScale     = new Vector3( 0.3f, 0.3f, 0.3f ); // 크기 30%
            
    }
    private void OffEffect()
    {
        if(effectOn){
            effectOn = false;
            Managers.Resource.Release(effect);
        }
    }
}
