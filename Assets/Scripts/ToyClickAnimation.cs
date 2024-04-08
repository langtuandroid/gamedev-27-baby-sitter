using UnityEngine;
using TemplateScripts;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ToyClickAnimation : MonoBehaviour, IPointerClickHandler {

	[FormerlySerializedAs("AnimationName")] [SerializeField] private string animationName = "";
	private Animator anim;

	public void OnPointerClick (PointerEventData eventData)
	{
		if(anim == null) anim = transform.GetComponent<Animator>();
		if(anim != null)    anim.CrossFade(animationName,.1f,-1,0);
		if(animationName == "Zvecka") 	if(  SoundManager.Instance!=null)  	SoundManager.Instance.Play_Sound( SoundManager.Instance.RattleToy);
	}
}
