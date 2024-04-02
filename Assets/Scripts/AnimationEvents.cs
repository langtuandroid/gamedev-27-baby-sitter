using UnityEngine;
using TemplateScripts;

public class AnimationEvents : MonoBehaviour {
	
	private Animator anim;

	public void AnimEventHideSceneAnimStarted()
	{
		LevelTransition.Instance.AnimEventHideSceneAnimStarted();
	}
	
	public void AnimEventShowSceneAnimFinished()
	{
		LevelTransition.Instance.AnimEventShowSceneAnimFinished();
	}


	public void ShampooBottleDrop()
	{
		if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.CreamTube);
		transform.parent.SendMessage("ShampooBottleDrop");
	}
	
	public void ShampooBottleMoveBack()
	{
		transform.parent.SendMessage("ShampooBottleMoveBack");
	}

	public void BathTubgPlugMoveBack()
	{
		if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.BathtubPlug);
		transform.parent.SendMessage("BathTubgPlugMoveBack");
	}
	
	public void CleaningAnimationFinished()
	{
		transform.parent.SendMessage("CleaningAnimationFinished",SendMessageOptions.DontRequireReceiver);
	}
 

	public void StartParticles()
	{
		transform.GetComponentInChildren<ParticleSystem>().Play();
	}
		
 
	public void MosquitoAnimEnd()
	{
		if(anim == null) anim = transform.GetComponent<Animator>();
		anim.SetInteger("moveAnimNo", Random.Range(1,7));
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
		Camera.main.SendMessage("ChangeSpriteFruitsBowl",   SendMessageOptions.DontRequireReceiver);
	}

 

	public void AnimLiquidIn()
	{
		if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.Liquid);
	}
	public void AnimLiquidStop()
	{
		if( SoundManager.Instance!=null)  SoundManager.Instance.Stop_Sound( SoundManager.Instance.Liquid);
	}

	public void AnimCerealIn()
	{
		if( SoundManager.Instance!=null)  SoundManager.Instance.Play_Sound( SoundManager.Instance.Cereal);
	}

	public void AnimCerealStop()
	{
		if( SoundManager.Instance!=null)  SoundManager.Instance.Stop_Sound( SoundManager.Instance.Cereal);
	}

	 

	public void AnimBlenderOn()
	{
		if( SoundManager.Instance!=null)  
		{
			SoundManager.Instance.Play_Sound( SoundManager.Instance.Blender);
			SoundManager.Instance.listStopSoundOnExit.Add( SoundManager.Instance.Blender);
		}
	}

	public void AnimBlenderOff()
	{
		if( SoundManager.Instance!=null)  SoundManager.Instance.Stop_Sound( SoundManager.Instance.Blender);
	}

	public void AnimTissueCleanEnd( )
	{
		if( !Application.loadedLevelName.StartsWith("Minigame 5") )
		 	GameObject.Find("RightMenu/ButtonsHolder/TissueBox").GetComponent<RightMenuItem>().HideTissue();
	}

 
}
