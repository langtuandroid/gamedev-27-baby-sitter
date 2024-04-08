using UnityEngine;
using TemplateScripts;

public class AnimationEventsBs : MonoBehaviour {
	
	private Animator animM;

	public void AnimEventHideSceneAnimStartedD()
	{
		LevelTransitionBS.Instance.AnimEventHideSceneAnimStartedD();
	}
	
	public void AnimEventShowSceneAnimFinishedD()
	{
		LevelTransitionBS.Instance.AnimEventShowSceneAnimFinishedD();
	}
	
	public void ShampooBottle_Drop()
	{
		if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.CreamTubeE);
		transform.parent.SendMessage("ShampooBottleDrop");
	}
	
	public void ShampooBottle_MoveBack()
	{
		transform.parent.SendMessage("ShampooBottleMoveBack");
	}

	public void BathTubgPlug_MoveBack()
	{
		if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.BathtubPlugG);
		transform.parent.SendMessage("BathTubgPlugMoveBack");
	}
	
	public void CleaningAnimation_Finished()
	{
		transform.parent.SendMessage("CleaningAnimationFinished",SendMessageOptions.DontRequireReceiver);
	}
	
	public void StartParticlesS()
	{
		transform.GetComponentInChildren<ParticleSystem>().Play();
	}
	
	public void MosquitoAnim_End()
	{
		if(animM == null) animM = transform.GetComponent<Animator>();
		animM.SetInteger("moveAnimNo", Random.Range(1,7));
	}
	
	public void AnimSmack_End()
	{
		transform.parent.SendMessage("AnimSmackEnd");
	}

	public void AnimCereal_ChangeSprite1()
	{
		Camera.main.SendMessage("ChangeSpriteCereal", 1, SendMessageOptions.DontRequireReceiver);
	}
	public void AnimCereal_ChangeSprite2()
	{
		Camera.main.SendMessage("ChangeSpriteCereal", 2, SendMessageOptions.DontRequireReceiver);
	}
 
	public void AnimFruitsBowl_ChangeSprite()
	{
		Camera.main.SendMessage("ChangeSpriteFruitsBowl",   SendMessageOptions.DontRequireReceiver);
	}

 

	public void AnimLiquidIn()
	{
		if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.LiquidD);
	}
	public void AnimLiquidStop()
	{
		if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.StopSound( SoundManagerBS.Instance.LiquidD);
	}

	public void AnimCerealIn()
	{
		if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.CerealL);
	}

	public void AnimCerealStop()
	{
		if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.StopSound( SoundManagerBS.Instance.CerealL);
	}

	 

	public void AnimBlenderOn()
	{
		if( SoundManagerBS.Instance!=null)  
		{
			SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.BlenderR);
			SoundManagerBS.Instance.listStopSoundOnExit.Add( SoundManagerBS.Instance.BlenderR);
		}
	}

	public void AnimBlenderOff()
	{
		if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.StopSound( SoundManagerBS.Instance.BlenderR);
	}

	public void AnimTissueCleanEnd( )
	{
		if( !Application.loadedLevelName.StartsWith("Minigame 5") )
		 	GameObject.Find("RightMenu/ButtonsHolder/TissueBox").GetComponent<RightMenuItemBS>().HideTissueE();
	}

 
}
