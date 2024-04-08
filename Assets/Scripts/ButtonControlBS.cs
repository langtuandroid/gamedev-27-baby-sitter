using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonControlBS : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler    
{
	public bool changeInteractable = true;
	private bool bPointerInN = false;
	private bool bPointerUpP= true;
	private Animator animM;
	private Button btnN;
	
	private void Start () {
		animM = transform.GetComponent<Animator>();
		btnN = transform.GetComponent<Button>();
	}
	
	public void OnPointerDown( PointerEventData eventData)
	{
		if(!changeInteractable && !btnN.interactable ) return;
		if(bPointerUpP )
		{
			btnN.interactable = true;
			bPointerInN = true;
			bPointerUpP = false;
			animM.SetBool("bPointerIn",bPointerInN );
		}
	}

	public void OnPointerUp( PointerEventData eventData)
	{
		bPointerUpP = true;
		bPointerInN = false;
		animM.SetBool("bPointerIn",bPointerInN );
	}

	public void OnPointerExit( PointerEventData eventData)
	{
		bPointerInN = false;
		animM.SetBool("bPointerIn",bPointerInN );
		if(changeInteractable)
		{
			btnN.interactable = false;
			animM.SetTrigger("Highlighted" ); 
		}
	}

}
