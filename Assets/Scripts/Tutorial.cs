using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour {

	public bool bTutorialFinished;

	[SerializeField] private Animator animTutorial;
	[SerializeField] private Animator animTutorialHolder;
 
	private int phase = -1;
	
	public Transform[] tutStartPos;
	public Transform[] tutEndPos;
	
	private bool bActive = false;
 
	private readonly float repeatTime = 2;
	private float leftTimeToRepeat = 0;
	private string lastTutorial = "";
	private Vector3 repStartPosition;

	public static Tutorial Instance;

	private void Awake()
	{
		Instance = this;
	}

	public void ShowPointer()
	{
		animTutorialHolder.CrossFade("ShowTutorial",.05f);
		 
	}

	public void HidePointer()
	{
		 if(animTutorialHolder.GetComponent<CanvasGroup>().alpha>0.05f)
			animTutorialHolder.CrossFade("HideTutorial",.05f); 
	}


	public void ShowPointerAndMoveToPosition(int  tutorialPhase)
	{
		phase = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke(nameof(RepeatTutorial));
		lastTutorial = "";
		StartCoroutine($"CShowPointerAndMoveToPosition", 0);
	}

	public void ShowPointerAndMoveToPosition(int  tutorialPhase, float delay)
	{
		phase = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke(nameof(RepeatTutorial));
		lastTutorial = "";
		StartCoroutine(nameof(CShowPointerAndMoveToPosition), delay);
	}
	
	private IEnumerator CShowPointerAndMoveToPosition( float delay)
	{
		if(bActive )
		{
			HidePointer();
			yield return new WaitForSeconds(0.1f);
		}

		yield return new WaitForSeconds(0.1f);
		animTutorial.Play("default");
		 

		yield return new WaitForSeconds(delay);
		animTutorial.transform.localScale =  tutStartPos[phase].localScale;
		animTutorial.transform.position =  tutStartPos[phase].position;
		bActive = true;
		ShowPointer();
		yield return new WaitForSeconds(0.2f);

		animTutorial.CrossFade("pointerDown",.05f);
		yield return new WaitForSeconds(1f);
	 
		float speed = 1.3f;//Vector3.Magnitude(tutEndPos[phase].position - tutStartPos[phase].position) *.2f;
		float timeMove = 0;
		while(timeMove<1)
		{
			timeMove+=speed * Time.fixedDeltaTime;
			animTutorial.transform.position = Vector3.Lerp(         tutStartPos[phase].position,  tutEndPos[phase].position, timeMove );
			yield return new WaitForFixedUpdate();
		}
		animTutorial.transform.position =  tutEndPos[phase].position;
		 
 	yield return new WaitForSeconds(.3f);
//		animTutorial.CrossFade("pointerUp",.05f);
//		yield return new WaitForSeconds(1f);
		HidePointer();
		yield return new WaitForSeconds(.2f);
		bActive = false;
		lastTutorial = "MoveToPosition";
		InvokeRepeating("RepeatTutorial",.5f,.5f);
	}


	public void ShowPointerAndTapOnPosition(int  tutorialPhase)
	{
		phase = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke(nameof(RepeatTutorial));
		lastTutorial = "";
		StartCoroutine($"CShowPointerAndTapOnPosition", 0);
	}

	public void ShowPointerAndTapOnPosition(int  tutorialPhase, float delay)
	{
		phase = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke(nameof(RepeatTutorial));
		lastTutorial = "";
		StartCoroutine(nameof(CShowPointerAndTapOnPosition), delay);
	}

	private IEnumerator CShowPointerAndTapOnPosition(float delay)
	{
		if(bActive )
		{
			HidePointer();
			yield return new WaitForSeconds(0.1f);
		}

		yield return new WaitForSeconds(0.1f);
		animTutorial.Play("default");

		 
		yield return new WaitForSeconds(delay);
		animTutorial.transform.localScale =  tutStartPos[phase].localScale;
		animTutorial.transform.position =  tutStartPos[phase].position;
		bActive = true;
		ShowPointer();
		yield return new WaitForSeconds(0.8f);
		
		animTutorial.CrossFade("pointerTap",.05f);
		yield return new WaitForSeconds(1f);

		HidePointer();
		yield return new WaitForSeconds(.2f);
		bActive = false;
		lastTutorial = "TapOnPosition";
		InvokeRepeating(nameof(RepeatTutorial),.5f,.5f);
	}



	public void ShowPointerAndMoveRepeating(int  tutorialPhase, float dly)
	{
		phase = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke(nameof(RepeatTutorial));
		lastTutorial = "";
		StartCoroutine( CShowPointerAndMoveRepeating(tutStartPos[phase].position, dly));
	}

	public void ShowPointerAndMoveRepeating(int  tutorialPhase, Vector3 StartPosition, float dly)
	{
		phase = tutorialPhase;
		StopAllCoroutines();
		CancelInvoke(nameof(RepeatTutorial));
		lastTutorial = "";
		StartCoroutine( CShowPointerAndMoveRepeating( StartPosition, dly));
	}
	
	private IEnumerator CShowPointerAndMoveRepeating(Vector3 StartPosition , float delay)
	{
		Vector3 _StartPosition = StartPosition;
		if(bActive )
		{
			HidePointer();
			yield return new WaitForSeconds(0.1f);
		}

		yield return new WaitForSeconds(0.1f);
		animTutorial.Play("default");
	 
		yield return new WaitForSeconds(delay);
		animTutorial.transform.localScale =  tutStartPos[phase].localScale;
		animTutorial.transform.position =  StartPosition;
		bActive = true;
		ShowPointer();
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
				animTutorial.transform.position = Vector3.Lerp(    _StartPosition ,  tutEndPos[phase].position, timeMove );
				yield return new WaitForFixedUpdate();
			}
			animTutorial.transform.position =  tutEndPos[phase].position;
			_StartPosition =    tutStartPos[phase].position;
			while(timeMove > 0)
			{
				timeMove -=speed * Time.fixedDeltaTime;
				animTutorial.transform.position = Vector3.Lerp(         tutStartPos[phase].position,  tutEndPos[phase].position, timeMove );
				yield return new WaitForFixedUpdate();
			}

		}

 		yield return new WaitForSeconds(.3f);

		HidePointer();
		yield return new WaitForSeconds(.2f);
		bActive = false;
		lastTutorial = "MoveRepeating";
		repStartPosition = StartPosition;
		InvokeRepeating(nameof(RepeatTutorial),.5f,.5f);
	}

 
	public void RepeatTutorial()
	{
		if(leftTimeToRepeat< repeatTime)
		{
			leftTimeToRepeat+=0.5f;
		}
		else
		{
			leftTimeToRepeat = 0;
			if(lastTutorial == "MoveToPosition") ShowPointerAndMoveToPosition(phase);
			else if(lastTutorial == "TapOnPosition") ShowPointerAndTapOnPosition(phase);
			else if(lastTutorial == "MoveRepeating") ShowPointerAndMoveRepeating(phase, repStartPosition,0);


		}
	}

	public void SwitchState()
	{
		if(bActive) 
		{
			StopAllCoroutines();
			HidePointer();
			
			bActive = false;
		}
		else
		{
			ShowTutorial(phase);

		}
	}

	public void ShowTutorial( int _phase)
	{
	 
		if(Application.loadedLevelName == "Minigame 1")
		{
			float dly = 2;
			
			if(_phase == 0)   ShowPointerAndMoveToPosition(0,dly);
			else if(_phase == 1)    ShowPointerAndMoveToPosition(1,dly);
			
			else if(_phase == 2)   ShowPointerAndMoveToPosition(2,dly);
			
			else if(_phase == 3)   ShowPointerAndTapOnPosition(3,dly);
			
			else if(_phase == 4)   ShowPointerAndTapOnPosition(4,dly);
			
			else if(_phase == 5)   ShowPointerAndMoveToPosition(5,dly);
			
			else if(_phase == 6)   ShowPointerAndTapOnPosition(6,dly);
		
			else if(_phase == 7)   ShowPointerAndMoveToPosition(7,dly);
			
			else if(_phase == 8)   ShowPointerAndTapOnPosition(8,dly);
		
			else if(_phase == 9)   ShowPointerAndMoveToPosition(9,dly);
		}

		else if(Application.loadedLevelName == "Minigame 2")
		{
			if(_phase == 0)   ShowPointerAndTapOnPosition(0);
			
			else if(_phase == 1)   ShowPointerAndMoveToPosition(1);
			
			else if(_phase == 2)   ShowPointerAndMoveRepeating(2 , GameObject.Find("TP2FS").transform.position,1);
			
			else if(_phase == 3)   ShowPointerAndMoveToPosition( 3 );
			
			else if(_phase == 4)   ShowPointerAndMoveRepeating( 4 ,1);
		
			else if(_phase == 5)   ShowPointerAndTapOnPosition( 5 );
			
			else if(_phase == 6)   ShowPointerAndTapOnPosition( 6, 2 );
			
			else if(_phase == 7)   ShowPointerAndMoveToPosition(7, 2);
			
			else if(_phase == 8)   ShowPointerAndMoveRepeating(8 , GameObject.Find("TP8FS").transform.position,1);
		 
		}

		else if(Application.loadedLevelName == "Minigame 3")
		{
			float dly = 2;
			
			if(_phase == 0)   ShowPointerAndMoveToPosition(0,dly);
			
			else if(_phase == 1)    ShowPointerAndMoveToPosition(1,dly);
			
			else if(_phase == 2)   ShowPointerAndMoveToPosition(2,dly);
			
			else if(_phase == 3)   ShowPointerAndTapOnPosition(3,dly);
		
			else if(_phase == 4)   ShowPointerAndMoveRepeating(4,dly );
		 
		}

		else if(Application.loadedLevelName == "Minigame 4")
		{
			float dly = 2;
			
			if(_phase == 0)   ShowPointerAndMoveToPosition(0,dly);
			
			else if(_phase == 1)    ShowPointerAndMoveToPosition(1,dly);
			
			else if(_phase == 2)   ShowPointerAndMoveToPosition(2,dly );
			
			else if(_phase == 3)   ShowPointerAndTapOnPosition(3,dly);
			
			else if(_phase == 4)   ShowPointerAndMoveRepeating(4,dly );
			
			else if(_phase == 5)   ShowPointerAndMoveRepeating(5, GameObject.Find("TP5FS").transform.position,dly );
			
		}

		else if(Application.loadedLevelName == "Minigame 5A")
		{
			float dly = 2;
			
			if(_phase == 0)   ShowPointerAndMoveToPosition(0,dly+1);
			
			else if(_phase == 1)    ShowPointerAndTapOnPosition(1,dly);
			
			else if(_phase == 2)   ShowPointerAndMoveToPosition(2,dly );
			
			else if(_phase == 3)   ShowPointerAndMoveRepeating(3, GameObject.Find("TP3FS").transform.position,dly+4 );
			
			else if(_phase == 4)   ShowPointerAndMoveToPosition(4,dly );
		}

		else if(Application.loadedLevelName == "Minigame 5B")
		{
			float dly = 2;
			
			if(_phase == 0)   ShowPointerAndMoveToPosition(0,dly+1);
			
			else if(_phase == 1)    ShowPointerAndMoveToPosition(1,dly);
		
			else if(_phase == 2)   ShowPointerAndMoveToPosition(2,dly );
			
			else if(_phase == 3)   ShowPointerAndMoveRepeating(3, GameObject.Find("TP3FS").transform.position,dly );
			
			else if(_phase == 4)   ShowPointerAndMoveRepeating(4, GameObject.Find("TP3FS").transform.position,dly);
			
			else if(_phase == 5)   ShowPointerAndMoveToPosition(5,dly );
		}

		else if(Application.loadedLevelName == "Minigame 6")
		{
			float dly = 2;
			
			if(_phase == 0)   ShowPointerAndMoveToPosition(0,dly);
		}


		else if(Application.loadedLevelName == "Minigame 7")
		{
			float dly = 2;
		
			if(_phase == 0)   ShowPointerAndMoveToPosition(0,dly);
			
			else if(_phase == 1)    ShowPointerAndTapOnPosition(1,dly+2);
			
			else if(_phase == 2)   ShowPointerAndMoveRepeating(2, dly );
			
			else if(_phase == 3)   ShowPointerAndMoveToPosition(3,dly);
			
			else if(_phase == 4)   ShowPointerAndMoveRepeating(4,dly );
			 
			
		}
		else if(Application.loadedLevelName == "Minigame 8")
		{
			float dly = 2;
			if(_phase == 0)   ShowPointerAndMoveToPosition(0,dly);
		}


	}

	public void StopTutorial( )
	{
		CancelInvoke(nameof(RepeatTutorial));
		lastTutorial = "";

		StopAllCoroutines();
		if(bActive)
		{
			HidePointer();
			bActive = false;
		}
	}

	public void PauseTutorial( string state )
	{
		if(Application.loadedLevelName == "Minigame 1")
		{
			string activeMenu = state.Replace("TopMenu ", "");
			if(    (activeMenu == "1" && phase == 2)  ||      (activeMenu == "2" && phase == 5)  || (activeMenu == "3" && phase == 7 ) ||  (activeMenu == "4" && phase == 9 ) )
				StopTutorial( );
		}
		else if(Application.loadedLevelName == "Minigame 2")
		{
			if(phase == 1)
				StopTutorial( );
		}
		else if(Application.loadedLevelName == "Minigame 3")
		{
				StopTutorial( );
		}
		else if(Application.loadedLevelName == "Minigame 4")
		{
			StopTutorial( );
		}

		else if(Application.loadedLevelName == "Minigame 5A")
		{
			StopTutorial( );
		}

		else if(Application.loadedLevelName == "Minigame 5B")
		{
			StopTutorial( );
		}
	}


 
}
