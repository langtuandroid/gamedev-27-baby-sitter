using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler    
{
	public bool changeInteractable = true;
	private bool bPointerIn = false;
	private bool bPointerUp= true;
	private Animator anim;
	private Button btn;
	
	private void Start () {
		anim = transform.GetComponent<Animator>();
		btn = transform.GetComponent<Button>();
	}
	
	public void OnPointerDown( PointerEventData eventData)
	{
		if(!changeInteractable && !btn.interactable ) return;
		if(bPointerUp )
		{
			btn.interactable = true;
			bPointerIn = true;
			bPointerUp = false;
			anim.SetBool("bPointerIn",bPointerIn );
		}
	}

	public void OnPointerUp( PointerEventData eventData)
	{
		bPointerUp = true;
		bPointerIn = false;
		anim.SetBool("bPointerIn",bPointerIn );
	}

	public void OnPointerExit( PointerEventData eventData)
	{
		bPointerIn = false;
		anim.SetBool("bPointerIn",bPointerIn );
		if(changeInteractable)
		{
			btn.interactable = false;
			anim.SetTrigger("Highlighted" ); 
		}
	}

}
