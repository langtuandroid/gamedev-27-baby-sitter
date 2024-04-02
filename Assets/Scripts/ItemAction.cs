using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;
//using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ItemAction : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public bool bEnabled = true;

	public bool bClickForAction = false;
	public bool bDragForAction = false;
	public string gamePhaseState = "";

	[FormerlySerializedAs("BubblesPref")] [SerializeField] private GameObject bubblesPref;
	private BubblesListHolder bubblesAnimHolder;
	
	private float dragProgress =0;
	private float targetProgress = 0;
	
	[FormerlySerializedAs("ShampoDrop")] [SerializeField] private Image shampoDrop; //za sampon

	private bool bBlanketUp = false;
	private Vector3 startPos;
	
	[FormerlySerializedAs("EndPos")] [SerializeField] private Transform endPos;
	private Vector3 cradleLeftPos;
	private Vector3 cradleRightPos;
	private int moveCradle = 0; //0 - ne pomera se,1-pomera se levo, 2-desno, 3 vraca se na start
	private float timePom;
	
	private bool bBlanketMoveCradle= false;
	private int cradleSwingCount = 0;
	private int lastCompletedSide = 0;
	
	public bool bCradleCountEnabled = false;
	private Animator animator;
	
	private bool bLampOn = false;

	private void Start () {
		if(gamePhaseState == "ShampooHead") bubblesAnimHolder = Camera.main.GetComponent<BubblesListHolder>();
		else if(gamePhaseState == "Blanket") startPos = transform.localPosition;
		else if(gamePhaseState == "Cradle")
		{
			startPos = transform.position;
			cradleLeftPos = startPos - new Vector3(.15f,0,0);
			cradleRightPos = startPos + new Vector3(.15f,0,0);
			bCradleCountEnabled = false;
		}
	}

	private void Update () {
	
		//kolevka
		if(moveCradle> 0)
		{
			timePom+=Time.deltaTime;//*.51f;
			 
			if(moveCradle ==1)
			{
				transform.position = Vector3.Lerp(transform.position, cradleLeftPos, timePom);
				 
				if( /*bCradleCountEnabled && */ lastCompletedSide!=1 &&  transform.position.x<cradleLeftPos.x*.4f) //.7
				{
					lastCompletedSide = 1;
					if(bCradleCountEnabled) 
					{
						cradleSwingCount++;
						if(cradleSwingCount == 1) Camera.main.SendMessage("BabySleep",1);
						else 	if(cradleSwingCount == 3) Camera.main.SendMessage("BabySleep",2);

						Tutorial.Instance.StopTutorial();
					}
					if( SoundManager.Instance!=null)  SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.CradleSwing);
				}

			}
			else if(moveCradle ==2)
			{
				transform.position = Vector3.Lerp(transform.position, cradleRightPos, timePom);
 
				if(  /*bCradleCountEnabled && */ lastCompletedSide!=2 &&  transform.position.x>cradleRightPos.x*.4f) //.7
				{
					lastCompletedSide = 2;
					if(bCradleCountEnabled) 
					{
						cradleSwingCount++;
						if(cradleSwingCount == 1) Camera.main.SendMessage("BabySleep",1);
						else 	if(cradleSwingCount == 3) Camera.main.SendMessage("BabySleep",2);

						Tutorial.Instance.StopTutorial();
					}
					if( SoundManager.Instance!=null)  SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.CradleSwing);
				}

			}
			if(moveCradle ==3)
			{
				transform.position = Vector3.Lerp(transform.position, startPos, timePom);
				if(timePom>.3f) 
				{
					timePom = 0; 
					moveCradle = 0;
					lastCompletedSide = 0;
				}
			}

			if(bCradleCountEnabled && cradleSwingCount == 5)
			{
				cradleSwingCount= 6;
				Camera.main.SendMessage("NextPhase",gamePhaseState);
			}
		}

	}

	public void OnBeginDrag (PointerEventData eventData)
	{
	 
	}
	
	
	public void OnDrag (PointerEventData eventData)
	{
		if(bDragForAction && bEnabled ) 
		{
			if(gamePhaseState == "ShampooHead")
			{

				dragProgress+=Time.deltaTime*9;
				if(targetProgress<dragProgress) CreateBubbles();
			}
			else if(gamePhaseState == "Blanket")
			{

				if( ( bBlanketUp  && Mathf.Abs(eventData.delta.x)>  Mathf.Abs(eventData.delta.y)))
				{
					if(  eventData.delta.x< 0)
					{
						transform.parent.SendMessage("MoveCradle",1);
					}
					else 
					{
						transform.parent.SendMessage("MoveCradle",2);
					}
					bBlanketMoveCradle = true;
				}
			 
				else if( !bBlanketMoveCradle && (( bBlanketUp && eventData.delta.y<0)  || ( !bBlanketUp && eventData.delta.y>0) ) )
				{
					if(GameData.BCompletedMiniGame) return;
					StopCoroutine(nameof(MoveBlanket));
					StartCoroutine(nameof(MoveBlanket));
				}
			}

			else if(gamePhaseState == "Cradle")
			{
				//Debug.Log(moveCredle+"  :  "+ eventData.position.x);
				if( moveCradle !=1 && eventData.delta.x < 0 )  
				{
					timePom = 0;
					moveCradle = 1;
					//StartCoroutine( "WaitEnable",1.2f);
				}
				else if( moveCradle !=2 && eventData.delta.x > 0 )  
				{
					timePom = 0;
					moveCradle = 2;
					//StartCoroutine( "WaitEnable",1.2f);
				}
			 
			}


			if(gamePhaseState == "RubCream" )
			{
				Tutorial.Instance.StopTutorial();
				//Tutorial.Instance.PauseTutorial("RubCream");
				dragProgress+=Time.deltaTime ;
				if( dragProgress >1) Camera.main.SendMessage("NextPhase",gamePhaseState);
			}

		}

		//------------------------------------
		if(bEnabled && gamePhaseState == "Curtains")
		{
			Camera.main.SendMessage("NextPhase",gamePhaseState);
			if(animator == null) animator = transform.GetComponent<Animator>();
			animator.SetTrigger("tClose");
			bEnabled = false;
			Tutorial.Instance.StopTutorial( );
		}
	}
	
	public void OnEndDrag (PointerEventData eventData)
	{
		if(moveCradle == 1 || moveCradle == 2)
		{
			timePom = 0;
			moveCradle = 3;
		}
		else if(bBlanketMoveCradle)
		{
			transform.parent.SendMessage("ReleasseCradle",2);
			bBlanketMoveCradle = false;
		}
	}

	IEnumerator WaitEnable(float timeE)
	{
		yield return new WaitForSeconds(timeE);
		bEnabled = true;

	}

	public void ReleasseCradle()
	{
		timePom = 0;
		moveCradle = 3;
	}

	public void MoveCradle(int _moveCredle)
	{
	 
		if( moveCradle !=1 && _moveCredle == 1 )  
		{
			timePom = 0;
			moveCradle = 1;
			//StartCoroutine( "WaitEnable",1.2f);
		}
		else if( moveCradle !=2 && _moveCredle == 2 )  
		{
			timePom = 0;
			moveCradle = 2;
			//StartCoroutine( "WaitEnable",1.2f);
		}
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if(bClickForAction && bEnabled ) 
		{
			if(gamePhaseState == "ShowerTap") 
				Camera.main.SendMessage("NextPhase",gamePhaseState);
			 
			else if(gamePhaseState == "LampHandle") 
			{
 
				if(GameData.BCompletedMiniGame) return;
				if(animator == null) animator = transform.parent.GetComponent<Animator>();
 
				if(!bLampOn)
				{
					animator.Play("On");
					Camera.main.SendMessage("NextPhase",gamePhaseState);
					if(  SoundManager.Instance!=null)  	SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.LightSwitch);
				}
				else 
				{
					Camera.main.SendMessage("UndoPhase",gamePhaseState);
					animator.Play("Off");
					if(  SoundManager.Instance!=null)  	SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.LightSwitch);
				}

				bEnabled = false;
				bLampOn = !bLampOn;
				StartCoroutine( "WaitEnable",.4f);


			}

			else if(gamePhaseState == "CradleDecoration")
			{
				 
				if(animator == null) animator = transform.GetComponent<Animator>();
				animator.SetTrigger("tDecorations" );
				if(  SoundManager.Instance!=null)  	SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.Chimes);
				bEnabled = false;
				StartCoroutine( nameof(WaitEnable), .25f);

			}
			else if(gamePhaseState == "BlenderButton")
			{
				Tutorial.Instance.StopTutorial();
				Camera.main.SendMessage("NextPhase",gamePhaseState);

			}
			else if(gamePhaseState == "Curtains")
			{
				if(bEnabled)
				{
					 
					Camera.main.SendMessage("NextPhase",gamePhaseState);
					if(animator == null) animator = transform.GetComponent<Animator>();
					animator.SetTrigger("tClose");
					bEnabled = false;
	 
				}
				 
			}
			else if(gamePhaseState == "Krema")
			{
				 
				Camera.main.SendMessage("NextPhase",gamePhaseState);
				if(animator == null) animator = transform.GetComponent<Animator>();
				animator.Play ("insert");
				if(  SoundManager.Instance!=null)  	SoundManager.Instance.Play_Sound( SoundManager.Instance.CreamTube);
				bEnabled = false;
				 
			}

			else if(gamePhaseState == "Cream")
			{
				if(  SoundManager.Instance!=null)  	SoundManager.Instance.Play_Sound( SoundManager.Instance.CreamTube);
				Camera.main.SendMessage("NextPhase",gamePhaseState);
				bEnabled = false;
			}

			if(Application.loadedLevelName == "Minigame 7" && ( gamePhaseState == "Krema" || gamePhaseState == "Curtains" || gamePhaseState == "LampHandle" ))
			{
				Tutorial.Instance.StopTutorial( );
			}

		}
	}

	private void CreateBubbles()
	{
		if(targetProgress == 0)
		{
			//ShampoDrop.enabled = false;
			//ShampoDrop.gameObject.SetActive( false);
			Tutorial.Instance.StopTutorial();
		}
		targetProgress++;
		if(targetProgress<11)
		{
			if(targetProgress== 10) 
			{
				shampoDrop.enabled = false;
				shampoDrop.gameObject.SetActive( false);
			}
			else
				shampoDrop.color = new Color(1,1,1,1-targetProgress*.1f);

		}
		


		GameObject bp = GameObject.Instantiate(bubblesPref);
		bp.transform.SetParent ( bubblesAnimHolder.bubblesAnim[0].transform.parent );
		bp.transform.position = transform.position + new Vector3(Random.Range(-.8f,.8f),Random.Range(-.2f,.2f),0);
		bubblesAnimHolder.bubblesAnim.Add(bp.GetComponent<Animator>() );

		if( SoundManager.Instance!=null) 
		{
			SoundManager.Instance.Bubble.pitch = Random.Range(.9f, 1.7f);
			SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.Bubble); 
		}

		if(targetProgress >= 20 )
		{
			Camera.main.SendMessage("NextPhase",gamePhaseState);
 
			bDragForAction = false;
			gameObject.SetActive(false);
		}
		 
	}


	public void AnimCurtainsEnd()
	{

		Camera.main.SendMessage("NextPhase",gamePhaseState);
	}
	
	private IEnumerator MoveBlanket()
	{
		bEnabled = false;
		float pom = 0;
		if(  SoundManager.Instance!=null)  	SoundManager.Instance.StopAndPlay_Sound( SoundManager.Instance.Blanket);
		if(bBlanketUp)
		{
			while(pom<1)
			{
				pom += Time.fixedDeltaTime;
				transform.localPosition = Vector3.Lerp( endPos.localPosition, startPos, pom);
			 	transform.localScale = Vector3.Lerp( endPos.localScale, Vector3.one, pom);

				Camera.main.SendMessage("UndoPhase",gamePhaseState);
				yield return new WaitForFixedUpdate();
			}
		}
		else
		{
			while(pom<1)
			{
				pom += Time.fixedDeltaTime;
				transform.localPosition = Vector3.Lerp(startPos,  endPos.localPosition, pom);
				transform.localScale = Vector3.Lerp( Vector3.one, endPos.localScale,  pom);
				Camera.main.SendMessage("NextPhase",gamePhaseState);
				yield return new WaitForFixedUpdate();
			}
		}
		bBlanketUp = !bBlanketUp;
		yield return new WaitForFixedUpdate();
		bEnabled = true;
	}


}
