using UnityEngine;
using TemplateScripts;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ToyClickAnimationBS : MonoBehaviour, IPointerClickHandler {

	[FormerlySerializedAs("AnimationName")] [SerializeField] private string animationName = "";
	private Animator anim;

	public void OnPointerClick (PointerEventData eventData)
	{
		if(anim == null) anim = transform.GetComponent<Animator>();
		if(anim != null)    anim.CrossFade(animationName,.1f,-1,0);
		if(animationName == "Zvecka") 	if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.RattleToyY);
	}
}
