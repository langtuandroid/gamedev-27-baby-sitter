using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TemplateScripts;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ItemActionBS : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public bool bEnabled = true;

	public bool bClickForAction = false;
	public bool bDragForAction = false;
	public string gamePhaseState = "";

	[FormerlySerializedAs("BubblesPref")] [SerializeField] private GameObject bubblesPref;
	private BubblesListHolder bubblesAnimHolderR;
	
	private float dragProgressS =0;
	private float targetProgressS = 0;
	
	[FormerlySerializedAs("ShampoDrop")] [SerializeField] private Image shampoDrop; //za sampon

	private bool bBlanketUpP = false;
	private Vector3 startPosS;
	
	[FormerlySerializedAs("EndPos")] [SerializeField] private Transform endPos;
	private Vector3 cradleLeftPosS;
	private Vector3 cradleRightPosS;
	private int moveCradleE = 0; //0 - ne pomera se,1-pomera se levo, 2-desno, 3 vraca se na start
	private float timePomM;
	
	private bool bBlanketMoveCradleE= false;
	private int cradleSwingCountT = 0;
	private int lastCompletedSideE = 0;
	
	public bool bCradleCountEnabled = false;
	private Animator animatorR;
	
	private bool bLampOnN = false;

	private void Start () {
		if(gamePhaseState == "ShampooHead") bubblesAnimHolderR = Camera.main.GetComponent<BubblesListHolder>();
		else if(gamePhaseState == "Blanket") startPosS = transform.localPosition;
		else if(gamePhaseState == "Cradle")
		{
			startPosS = transform.position;
			cradleLeftPosS = startPosS - new Vector3(.15f,0,0);
			cradleRightPosS = startPosS + new Vector3(.15f,0,0);
			bCradleCountEnabled = false;
		}
	}

	private void Update () {
	
		//kolevka
		if(moveCradleE> 0)
		{
			timePomM+=Time.deltaTime;//*.51f;
			 
			if(moveCradleE ==1)
			{
				transform.position = Vector3.Lerp(transform.position, cradleLeftPosS, timePomM);
				 
				if( /*bCradleCountEnabled && */ lastCompletedSideE!=1 &&  transform.position.x<cradleLeftPosS.x*.4f) //.7
				{
					lastCompletedSideE = 1;
					if(bCradleCountEnabled) 
					{
						cradleSwingCountT++;
						if(cradleSwingCountT == 1) Camera.main.SendMessage("BabySleep",1);
						else 	if(cradleSwingCountT == 3) Camera.main.SendMessage("BabySleep",2);

						TutorialBS.Instance.StopTutor();
					}
					if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.CradleSwingG);
				}

			}
			else if(moveCradleE ==2)
			{
				transform.position = Vector3.Lerp(transform.position, cradleRightPosS, timePomM);
 
				if(  /*bCradleCountEnabled && */ lastCompletedSideE!=2 &&  transform.position.x>cradleRightPosS.x*.4f) //.7
				{
					lastCompletedSideE = 2;
					if(bCradleCountEnabled) 
					{
						cradleSwingCountT++;
						if(cradleSwingCountT == 1) Camera.main.SendMessage("BabySleep",1);
						else 	if(cradleSwingCountT == 3) Camera.main.SendMessage("BabySleep",2);

						TutorialBS.Instance.StopTutor();
					}
					if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.CradleSwingG);
				}

			}
			if(moveCradleE ==3)
			{
				transform.position = Vector3.Lerp(transform.position, startPosS, timePomM);
				if(timePomM>.3f) 
				{
					timePomM = 0; 
					moveCradleE = 0;
					lastCompletedSideE = 0;
				}
			}

			if(bCradleCountEnabled && cradleSwingCountT == 5)
			{
				cradleSwingCountT= 6;
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

				dragProgressS+=Time.deltaTime*9;
				if(targetProgressS<dragProgressS) CreateBubbles();
			}
			else if(gamePhaseState == "Blanket")
			{

				if( ( bBlanketUpP  && Mathf.Abs(eventData.delta.x)>  Mathf.Abs(eventData.delta.y)))
				{
					if(  eventData.delta.x< 0)
					{
						transform.parent.SendMessage("MoveCradle",1);
					}
					else 
					{
						transform.parent.SendMessage("MoveCradle",2);
					}
					bBlanketMoveCradleE = true;
				}
			 
				else if( !bBlanketMoveCradleE && (( bBlanketUpP && eventData.delta.y<0)  || ( !bBlanketUpP && eventData.delta.y>0) ) )
				{
					if(GameDataBS.BCompletedMiniGameE) return;
					StopCoroutine(nameof(MoveBlanketT));
					StartCoroutine(nameof(MoveBlanketT));
				}
			}

			else if(gamePhaseState == "Cradle")
			{
				//Debug.Log(moveCredle+"  :  "+ eventData.position.x);
				if( moveCradleE !=1 && eventData.delta.x < 0 )  
				{
					timePomM = 0;
					moveCradleE = 1;
					//StartCoroutine( "WaitEnable",1.2f);
				}
				else if( moveCradleE !=2 && eventData.delta.x > 0 )  
				{
					timePomM = 0;
					moveCradleE = 2;
					//StartCoroutine( "WaitEnable",1.2f);
				}
			 
			}


			if(gamePhaseState == "RubCream" )
			{
				TutorialBS.Instance.StopTutor();
				//Tutorial.Instance.PauseTutorial("RubCream");
				dragProgressS+=Time.deltaTime ;
				if( dragProgressS >1) Camera.main.SendMessage("NextPhase",gamePhaseState);
			}

		}

		//------------------------------------
		if(bEnabled && gamePhaseState == "Curtains")
		{
			Camera.main.SendMessage("NextPhase",gamePhaseState);
			if(animatorR == null) animatorR = transform.GetComponent<Animator>();
			animatorR.SetTrigger("tClose");
			bEnabled = false;
			TutorialBS.Instance.StopTutor( );
		}
	}
	
	public void OnEndDrag (PointerEventData eventData)
	{
		if(moveCradleE == 1 || moveCradleE == 2)
		{
			timePomM = 0;
			moveCradleE = 3;
		}
		else if(bBlanketMoveCradleE)
		{
			transform.parent.SendMessage("ReleasseCradle",2);
			bBlanketMoveCradleE = false;
		}
	}

	private IEnumerator WaitEnableE(float timeE)
	{
		yield return new WaitForSeconds(timeE);
		bEnabled = true;

	}

	public void ReleaseCradle()
	{
		timePomM = 0;
		moveCradleE = 3;
	}

	public void MoveCradleE(int moveCradle)
	{
		if( moveCradleE !=1 && moveCradle == 1 )  
		{
			timePomM = 0;
			moveCradleE = 1;
			//StartCoroutine( "WaitEnable",1.2f);
		}
		else if( moveCradleE !=2 && moveCradle == 2 )  
		{
			timePomM = 0;
			moveCradleE = 2;
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
 
				if(GameDataBS.BCompletedMiniGameE) return;
				if(animatorR == null) animatorR = transform.parent.GetComponent<Animator>();
 
				if(!bLampOnN)
				{
					animatorR.Play("On");
					Camera.main.SendMessage("NextPhase",gamePhaseState);
					if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.LightSwitchH);
				}
				else 
				{
					Camera.main.SendMessage("UndoPhase",gamePhaseState);
					animatorR.Play("Off");
					if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.LightSwitchH);
				}

				bEnabled = false;
				bLampOnN = !bLampOnN;
				StartCoroutine( nameof(WaitEnableE),.4f);


			}

			else if(gamePhaseState == "CradleDecoration")
			{
				 
				if(animatorR == null) animatorR = transform.GetComponent<Animator>();
				animatorR.SetTrigger("tDecorations" );
				if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.ChimesS);
				bEnabled = false;
				StartCoroutine( nameof(WaitEnableE), .25f);

			}
			else if(gamePhaseState == "BlenderButton")
			{
				TutorialBS.Instance.StopTutor();
				Camera.main.SendMessage("NextPhase",gamePhaseState);

			}
			else if(gamePhaseState == "Curtains")
			{
				if(bEnabled)
				{
					 
					Camera.main.SendMessage("NextPhase",gamePhaseState);
					if(animatorR == null) animatorR = transform.GetComponent<Animator>();
					animatorR.SetTrigger("tClose");
					bEnabled = false;
	 
				}
				 
			}
			else if(gamePhaseState == "Krema")
			{
				 
				Camera.main.SendMessage("NextPhase",gamePhaseState);
				if(animatorR == null) animatorR = transform.GetComponent<Animator>();
				animatorR.Play ("insert");
				if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.CreamTubeE);
				bEnabled = false;
				 
			}

			else if(gamePhaseState == "Cream")
			{
				if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.CreamTubeE);
				Camera.main.SendMessage("NextPhase",gamePhaseState);
				bEnabled = false;
			}

			if(Application.loadedLevelName == "Minigame 7" && ( gamePhaseState == "Krema" || gamePhaseState == "Curtains" || gamePhaseState == "LampHandle" ))
			{
				TutorialBS.Instance.StopTutor( );
			}

		}
	}

	private void CreateBubbles()
	{
		if(targetProgressS == 0)
		{
			TutorialBS.Instance.StopTutor();
		}
		targetProgressS++;
		if(targetProgressS<11)
		{
			if(targetProgressS== 10) 
			{
				shampoDrop.enabled = false;
				shampoDrop.gameObject.SetActive( false);
			}
			else
				shampoDrop.color = new Color(1,1,1,1-targetProgressS*.1f);

		}
		
		GameObject bp = GameObject.Instantiate(bubblesPref);
		bp.transform.SetParent ( bubblesAnimHolderR.bubblesAnimM[0].transform.parent );
		bp.transform.position = transform.position + new Vector3(Random.Range(-.8f,.8f),Random.Range(-.2f,.2f),0);
		bubblesAnimHolderR.bubblesAnimM.Add(bp.GetComponent<Animator>() );

		if( SoundManagerBS.Instance!=null) 
		{
			SoundManagerBS.Instance.BubbleE.pitch = Random.Range(.9f, 1.7f);
			SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.BubbleE); 
		}

		if(targetProgressS >= 20 )
		{
			Camera.main.SendMessage("NextPhase",gamePhaseState);
 
			bDragForAction = false;
			gameObject.SetActive(false);
		}
		 
	}


	public void AnimCurtains_End()
	{
		Camera.main.SendMessage("NextPhase",gamePhaseState);
	}
	
	private IEnumerator MoveBlanketT()
	{
		bEnabled = false;
		float pom = 0;
		if(  SoundManagerBS.Instance!=null)  	SoundManagerBS.Instance.StopAndPlay_Sound( SoundManagerBS.Instance.BlanketT);
		if(bBlanketUpP)
		{
			while(pom<1)
			{
				pom += Time.fixedDeltaTime;
				transform.localPosition = Vector3.Lerp( endPos.localPosition, startPosS, pom);
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
				transform.localPosition = Vector3.Lerp(startPosS,  endPos.localPosition, pom);
				transform.localScale = Vector3.Lerp( Vector3.one, endPos.localScale,  pom);
				Camera.main.SendMessage("NextPhase",gamePhaseState);
				yield return new WaitForFixedUpdate();
			}
		}
		bBlanketUpP = !bBlanketUpP;
		yield return new WaitForFixedUpdate();
		bEnabled = true;
	}


}
