using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialBS : MonoBehaviour {

	public bool bTutorialFinished;

	[SerializeField] private Animator animTutorial;
	[SerializeField] private Animator animTutorialHolder;
 
	private int phaseE = -1;
	
	public Transform[] tutStartPos;
	public Transform[] tutEndPos;
	
	private bool bActiveE = false;
 
	private readonly float repeatTimeE = 2;
	private float leftTimeToRepeatT = 0;
	private string lastTutorialL = "";
	private Vector3 repStartPositionN;

	public static TutorialBS Instance;

	private void Awake()
	{
		Instance = this;
	}

	public void ShowPointerR()
	{
		animTutorialHolder.CrossFade("ShowTutorial",.05f);
		 
	}

	public void HidePointerR()
	{
		 if(animTutorialHolder.GetComponent<CanvasGroup>().alpha>0.05f)
			animTutorialHolder.CrossFade("HideTutorial",.05f); 
	}


	public void ShowPointerAndMoveToPositionN(int  tutorialPhase)
	{
		phaseE = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke(nameof(RepeatTutor));
		lastTutorialL = "";
		StartCoroutine($"CShowPointerAndMoveToPositionN", 0);
	}

	public void ShowPointerAndMoveToPositionN(int  tutorialPhase, float delay)
	{
		phaseE = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke(nameof(RepeatTutor));
		lastTutorialL = "";
		StartCoroutine(nameof(CShowPointerAndMoveToPositionN), delay);
	}
	
	private IEnumerator CShowPointerAndMoveToPositionN( float delay)
	{
		if(bActiveE )
		{
			HidePointerR();
			yield return new WaitForSeconds(0.1f);
		}

		yield return new WaitForSeconds(0.1f);
		animTutorial.Play("default");
		 

		yield return new WaitForSeconds(delay);
		animTutorial.transform.localScale =  tutStartPos[phaseE].localScale;
		animTutorial.transform.position =  tutStartPos[phaseE].position;
		bActiveE = true;
		ShowPointerR();
		yield return new WaitForSeconds(0.2f);

		animTutorial.CrossFade("pointerDown",.05f);
		yield return new WaitForSeconds(1f);
	 
		float speed = 1.3f;//Vector3.Magnitude(tutEndPos[phase].position - tutStartPos[phase].position) *.2f;
		float timeMove = 0;
		while(timeMove<1)
		{
			timeMove+=speed * Time.fixedDeltaTime;
			animTutorial.transform.position = Vector3.Lerp(         tutStartPos[phaseE].position,  tutEndPos[phaseE].position, timeMove );
			yield return new WaitForFixedUpdate();
		}
		animTutorial.transform.position =  tutEndPos[phaseE].position;
		 
 	yield return new WaitForSeconds(.3f);
//		animTutorial.CrossFade("pointerUp",.05f);
//		yield return new WaitForSeconds(1f);
		HidePointerR();
		yield return new WaitForSeconds(.2f);
		bActiveE = false;
		lastTutorialL = "MoveToPosition";
		InvokeRepeating(nameof(RepeatTutor),.5f,.5f);
	}


	public void ShowPointerAndTapOnPositionN(int  tutorialPhase)
	{
		phaseE = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke(nameof(RepeatTutor));
		lastTutorialL = "";
		StartCoroutine($"CShowPointerAndTapOnPosition", 0);
	}

	public void ShowPointerAndTapOnPositionN(int  tutorialPhase, float delay)
	{
		phaseE = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke(nameof(RepeatTutor));
		lastTutorialL = "";
		StartCoroutine(nameof(CShowPointerAndTapOnPosition), delay);
	}

	private IEnumerator CShowPointerAndTapOnPosition(float delay)
	{
		if(bActiveE )
		{
			HidePointerR();
			yield return new WaitForSeconds(0.1f);
		}

		yield return new WaitForSeconds(0.1f);
		animTutorial.Play("default");

		 
		yield return new WaitForSeconds(delay);
		animTutorial.transform.localScale =  tutStartPos[phaseE].localScale;
		animTutorial.transform.position =  tutStartPos[phaseE].position;
		bActiveE = true;
		ShowPointerR();
		yield return new WaitForSeconds(0.8f);
		
		animTutorial.CrossFade("pointerTap",.05f);
		yield return new WaitForSeconds(1f);

		HidePointerR();
		yield return new WaitForSeconds(.2f);
		bActiveE = false;
		lastTutorialL = "TapOnPosition";
		InvokeRepeating(nameof(RepeatTutor),.5f,.5f);
	}



	public void ShowPointerAndMoveRepeatingG(int  tutorialPhase, float dly)
	{
		phaseE = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke(nameof(RepeatTutor));
		lastTutorialL = "";
		StartCoroutine( CShowPointerAndMoveRepeatingG(tutStartPos[phaseE].position, dly));
	}

	public void ShowPointerAndMoveRepeatingG(int  tutorialPhase, Vector3 StartPosition, float dly)
	{
		phaseE = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke(nameof(RepeatTutor));
		lastTutorialL = "";
		StartCoroutine( CShowPointerAndMoveRepeatingG( StartPosition, dly));
	}
	
	private IEnumerator CShowPointerAndMoveRepeatingG(Vector3 StartPosition , float delay)
	{
		Vector3 _StartPosition = StartPosition;
		if(bActiveE )
		{
			HidePointerR();
			yield return new WaitForSeconds(0.1f);
		}

		yield return new WaitForSeconds(0.1f);
		animTutorial.Play("default");
	 
		yield return new WaitForSeconds(delay);
		animTutorial.transform.localScale =  tutStartPos[phaseE].localScale;
		animTutorial.transform.position =  StartPosition;
		bActiveE = true;
		ShowPointerR();
		yield return new WaitForSeconds(0.2f);
		
		animTutorial.CrossFade("pointerDown",.05f);
		yield return new WaitForSeconds(1f);
		
		float speed = 1.3f;//Vector3.Magnitude(tutEndPos[phase].position - tutStartPos[phase].position) *.2f;
		int  repeatCycles = 2;

		for(int i = 0; i < repeatCycles; i++)
		{
			float timeMove = 0;
			while(timeMove<1)
			{
				timeMove+=speed * Time.fixedDeltaTime;
				animTutorial.transform.position = Vector3.Lerp(    _StartPosition ,  tutEndPos[phaseE].position, timeMove );
				yield return new WaitForFixedUpdate();
			}
			animTutorial.transform.position =  tutEndPos[phaseE].position;
			_StartPosition =    tutStartPos[phaseE].position;
			while(timeMove > 0)
			{
				timeMove -=speed * Time.fixedDeltaTime;
				animTutorial.transform.position = Vector3.Lerp(         tutStartPos[phaseE].position,  tutEndPos[phaseE].position, timeMove );
				yield return new WaitForFixedUpdate();
			}

		}

 		yield return new WaitForSeconds(.3f);

		HidePointerR();
		yield return new WaitForSeconds(.2f);
		bActiveE = false;
		lastTutorialL = "MoveRepeating";
		repStartPositionN = StartPosition;
		InvokeRepeating(nameof(RepeatTutor),.5f,.5f);
	}

 
	public void RepeatTutor()
	{
		if(leftTimeToRepeatT< repeatTimeE)
		{
			leftTimeToRepeatT+=0.5f;
		}
		else
		{
			leftTimeToRepeatT = 0;
			if(lastTutorialL == "MoveToPosition") ShowPointerAndMoveToPositionN(phaseE);
			else if(lastTutorialL == "TapOnPosition") ShowPointerAndTapOnPositionN(phaseE);
			else if(lastTutorialL == "MoveRepeating") ShowPointerAndMoveRepeatingG(phaseE, repStartPositionN,0);


		}
	}

	public void SwitchStateE()
	{
		if(bActiveE) 
		{
			StopAllCoroutines();
			HidePointerR();
			
			bActiveE = false;
		}
		else
		{
			ShowTutorial(phaseE);

		}
	}

	public void ShowTutorial( int _phase)
	{
	 
		if(Application.loadedLevelName == "Minigame 1")
		{
			float dly = 2;
			
			if(_phase == 0)   ShowPointerAndMoveToPositionN(0,dly);
			else if(_phase == 1)    ShowPointerAndMoveToPositionN(1,dly);
			
			else if(_phase == 2)   ShowPointerAndMoveToPositionN(2,dly);
			
			else if(_phase == 3)   ShowPointerAndTapOnPositionN(3,dly);
			
			else if(_phase == 4)   ShowPointerAndTapOnPositionN(4,dly);
			
			else if(_phase == 5)   ShowPointerAndMoveToPositionN(5,dly);
			
			else if(_phase == 6)   ShowPointerAndTapOnPositionN(6,dly);
		
			else if(_phase == 7)   ShowPointerAndMoveToPositionN(7,dly);
			
			else if(_phase == 8)   ShowPointerAndTapOnPositionN(8,dly);
		
			else if(_phase == 9)   ShowPointerAndMoveToPositionN(9,dly);
		}

		else if(Application.loadedLevelName == "Minigame 2")
		{
			if(_phase == 0)   ShowPointerAndTapOnPositionN(0);
			
			else if(_phase == 1)   ShowPointerAndMoveToPositionN(1);
			
			else if(_phase == 2)   ShowPointerAndMoveRepeatingG(2 , GameObject.Find("TP2FS").transform.position,1);
			
			else if(_phase == 3)   ShowPointerAndMoveToPositionN( 3 );
			
			else if(_phase == 4)   ShowPointerAndMoveRepeatingG( 4 ,1);
		
			else if(_phase == 5)   ShowPointerAndTapOnPositionN( 5 );
			
			else if(_phase == 6)   ShowPointerAndTapOnPositionN( 6, 2 );
			
			else if(_phase == 7)   ShowPointerAndMoveToPositionN(7, 2);
			
			else if(_phase == 8)   ShowPointerAndMoveRepeatingG(8 , GameObject.Find("TP8FS").transform.position,1);
		 
		}

		else if(Application.loadedLevelName == "Minigame 3")
		{
			float dly = 2;
			
			if(_phase == 0)   ShowPointerAndMoveToPositionN(0,dly);
			
			else if(_phase == 1)    ShowPointerAndMoveToPositionN(1,dly);
			
			else if(_phase == 2)   ShowPointerAndMoveToPositionN(2,dly);
			
			else if(_phase == 3)   ShowPointerAndTapOnPositionN(3,dly);
		
			else if(_phase == 4)   ShowPointerAndMoveRepeatingG(4,dly );
		 
		}

		else if(Application.loadedLevelName == "Minigame 4")
		{
			float dly = 2;
			
			if(_phase == 0)   ShowPointerAndMoveToPositionN(0,dly);
			
			else if(_phase == 1)    ShowPointerAndMoveToPositionN(1,dly);
			
			else if(_phase == 2)   ShowPointerAndMoveToPositionN(2,dly );
			
			else if(_phase == 3)   ShowPointerAndTapOnPositionN(3,dly);
			
			else if(_phase == 4)   ShowPointerAndMoveRepeatingG(4,dly );
			
			else if(_phase == 5)   ShowPointerAndMoveRepeatingG(5, GameObject.Find("TP5FS").transform.position,dly );
			
		}

		else if(Application.loadedLevelName == "Minigame 5A")
		{
			float dly = 2;
			
			if(_phase == 0)   ShowPointerAndMoveToPositionN(0,dly+1);
			
			else if(_phase == 1)    ShowPointerAndTapOnPositionN(1,dly);
			
			else if(_phase == 2)   ShowPointerAndMoveToPositionN(2,dly );
			
			else if(_phase == 3)   ShowPointerAndMoveRepeatingG(3, GameObject.Find("TP3FS").transform.position,dly+4 );
			
			else if(_phase == 4)   ShowPointerAndMoveToPositionN(4,dly );
		}

		else if(Application.loadedLevelName == "Minigame 5B")
		{
			float dly = 2;
			
			if(_phase == 0)   ShowPointerAndMoveToPositionN(0,dly+1);
			
			else if(_phase == 1)    ShowPointerAndMoveToPositionN(1,dly);
		
			else if(_phase == 2)   ShowPointerAndMoveToPositionN(2,dly );
			
			else if(_phase == 3)   ShowPointerAndMoveRepeatingG(3, GameObject.Find("TP3FS").transform.position,dly );
			
			else if(_phase == 4)   ShowPointerAndMoveRepeatingG(4, GameObject.Find("TP3FS").transform.position,dly);
			
			else if(_phase == 5)   ShowPointerAndMoveToPositionN(5,dly );
		}

		else if(Application.loadedLevelName == "Minigame 6")
		{
			float dly = 2;
			
			if(_phase == 0)   ShowPointerAndMoveToPositionN(0,dly);
		}


		else if(Application.loadedLevelName == "Minigame 7")
		{
			float dly = 2;
		
			if(_phase == 0)   ShowPointerAndMoveToPositionN(0,dly);
			
			else if(_phase == 1)    ShowPointerAndTapOnPositionN(1,dly+2);
			
			else if(_phase == 2)   ShowPointerAndMoveRepeatingG(2, dly );
			
			else if(_phase == 3)   ShowPointerAndMoveToPositionN(3,dly);
			
			else if(_phase == 4)   ShowPointerAndMoveRepeatingG(4,dly );
			 
			
		}
		else if(Application.loadedLevelName == "Minigame 8")
		{
			float dly = 2;
			if(_phase == 0)   ShowPointerAndMoveToPositionN(0,dly);
		}


	}

	public void StopTutor( )
	{
		CancelInvoke(nameof(RepeatTutor));
		lastTutorialL = "";

		StopAllCoroutines();
		if(bActiveE)
		{
			HidePointerR();
			bActiveE = false;
		}
	}

	public void PauseTutorialL( string state )
	{
		if(Application.loadedLevelName == "Minigame 1")
		{
			string activeMenu = state.Replace("TopMenu ", "");
			if(    (activeMenu == "1" && phaseE == 2)  ||      (activeMenu == "2" && phaseE == 5)  || (activeMenu == "3" && phaseE == 7 ) ||  (activeMenu == "4" && phaseE == 9 ) )
				StopTutor( );
		}
		else if(Application.loadedLevelName == "Minigame 2")
		{
			if(phaseE == 1)
				StopTutor( );
		}
		else if(Application.loadedLevelName == "Minigame 3")
		{
				StopTutor( );
		}
		else if(Application.loadedLevelName == "Minigame 4")
		{
			StopTutor( );
		}

		else if(Application.loadedLevelName == "Minigame 5A")
		{
			StopTutor( );
		}

		else if(Application.loadedLevelName == "Minigame 5B")
		{
			StopTutor( );
		}
	}


 
}
