using UnityEngine;
using TemplateScripts;

public class AnimationEventsBs : MonoBehaviour {
	
	private Animator animM;

	public void AnimEventHideSceneAnimStarted()
	{
		LevelTransitionBS.Instance.AnimEventHideSceneAnimStarted();
	}
	
	public void AnimEventShowSceneAnimFinished()
	{
		LevelTransitionBS.Instance.AnimEventShowSceneAnimFinished();
	}
	
	public void ShampooBottleDrop()
	{
		if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.CreamTubeE);
		transform.parent.SendMessage("ShampooBottle_Drop");
	}
	
	public void ShampooBottleMoveBack()
	{
		transform.parent.SendMessage("ShampooBottle_MoveBack");
	}

	public void BathTubgPlugMoveBack()
	{
		if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.BathtubPlugG);
		transform.parent.SendMessage("BathTubgPlug_MoveBack");
	}
	
	public void CleaningAnimationFinished()
	{
		transform.parent.SendMessage("CleaningAnimation_Finished",SendMessageOptions.DontRequireReceiver);
	}
	
	public void StartParticlesS()
	{
		transform.GetComponentInChildren<ParticleSystem>().Play();
	}
	
	public void MosquitoAnimEnd()
	{
		if(animM == null) animM = transform.GetComponent<Animator>();
		animM.SetInteger("moveAnimNo", Random.Range(1,7));
	}
	
	public void AnimSmackEnd()
	{
		transform.parent.SendMessage("AnimSmackEnd");
	}

	public void AnimCerealChangeSprite1()
	{
		Camera.main.SendMessage("ChangeSpriteCereal", 1, SendMessageOptions.DontRequireReceiver);
	}
	public void AnimCerealChangeSprite2()
	{
		Camera.main.SendMessage("ChangeSpriteCereal", 2, SendMessageOptions.DontRequireReceiver);
	}
 
	public void AnimFruitsBowlChangeSprite()
	{
		Camera.main.SendMessage("ChangeSpriteFruitsBowlL",   SendMessageOptions.DontRequireReceiver);
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
