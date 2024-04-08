using UnityEngine;
using System.Collections;
using TemplateScripts;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Minigame6BS : MonoBehaviour {

	[FormerlySerializedAs("progressBar")] [SerializeField] private ProgressBar progressBarR;

	private static int _completedActionNoN = 0;
	
	[FormerlySerializedAs("ButtonNext")] [SerializeField] private GameObject buttonNext;
	[FormerlySerializedAs("ButtonHome")] [SerializeField] private GameObject buttonHome;
 
	[SerializeField] private Image[] toysShelf;
	[SerializeField] private Image[] toys;
	[SerializeField] private Sprite[] toysSprites;
	
	[FormerlySerializedAs("StartPositions")] [SerializeField] private Transform[] startPositions;
	[FormerlySerializedAs("Shelf1Positions")] [SerializeField] private Transform[] shelf1Positions;
	[FormerlySerializedAs("Shelf2Positions")] [SerializeField] private Transform[] shelf2Positions;
	[FormerlySerializedAs("Shelf3Positions")] [SerializeField] private Transform[] shelf3Positions;

	private int toyImgAA, toyImgBB, toyImgCC;
	[SerializeField] private ParticleSystem psLevelCompleted;

	private void Awake()
	{
		buttonNext.SetActive(false);
	}
	
	private IEnumerator Start () {
		
		Transform pom;
		int a,b;
		for(int i = 0; i<100;i++)
		{
		 	a = Random.Range(0,startPositions.Length);
			pom = startPositions[a];
			b = Random.Range(0,startPositions.Length);
			startPositions[a] = startPositions[b];
			startPositions[b] = pom;
		}

		toyImgAA = Random.Range(0,toysSprites.Length);

		toyImgBB = Random.Range(0,toysSprites.Length);
		while(toyImgAA == toyImgBB)
		{
			toyImgBB = Random.Range(0,toysSprites.Length);
		}

		toyImgCC = Random.Range(0,toysSprites.Length);
		while(toyImgCC == toyImgBB || toyImgCC == toyImgAA)
		{
			toyImgCC = Random.Range(0,toysSprites.Length);
		}
		
		for(int i = 0; i<toys.Length;i++)
		{
			toys[i].transform.parent.SetParent(startPositions[i]);
			toys[i].transform.parent.localPosition = Vector3.zero;
			MoveToyBS mt = toys[i].transform.parent.GetComponent<MoveToyBS>();
			if(i<3)
			{
				toys[i].sprite = toysSprites[toyImgAA];
				mt.ShelfIndex = 1;
				mt.TargetPoint = shelf1Positions;
			}
			else if(i<6)
			{
				toys[i].sprite = toysSprites[toyImgBB];
				mt.ShelfIndex = 2;
				mt.TargetPoint = shelf2Positions;
			}
			else
			{
				toys[i].sprite = toysSprites[toyImgCC];
				mt.ShelfIndex = 3;
				mt.TargetPoint = shelf3Positions;
			}
		}

		toysShelf[0].sprite =  toysSprites[toyImgAA];
		toysShelf[1].sprite =  toysSprites[toyImgBB];
		toysShelf[2].sprite =  toysSprites[toyImgCC];

		toysShelf[0].transform.parent.SetParent(shelf1Positions[Random.Range(0,shelf1Positions.Length)]);
		toysShelf[1].transform.parent.SetParent(shelf2Positions[Random.Range(0,shelf2Positions.Length)]);
		toysShelf[2].transform.parent.SetParent(shelf3Positions[Random.Range(0,shelf3Positions.Length)]);

		toysShelf[0].transform.parent.localPosition = Vector3.zero;
		toysShelf[1].transform.parent.localPosition = Vector3.zero;
		toysShelf[2].transform.parent.localPosition = Vector3.zero;
		
		_completedActionNoN = 0;
		
		
		BlockClicksBs.Instance.SetBlockAllL(true);
		BlockClicksBs.Instance.SetBlockAllDelayY(.2f,false);
		
		yield return new WaitForSeconds(1);
		LevelTransitionBS.Instance.ShowSceneE();

		TutorialBS.Instance.tutStartPos[0].position = toys[5].transform.position + new Vector3(0,0.2f,0);

		int deadloop = 0;
		int tutPosIndex=   Random.Range(0,shelf2Positions.Length);
		while(shelf2Positions[tutPosIndex]  .childCount>0 && deadloop<1000)
		{
			tutPosIndex=   Random.Range(0,shelf2Positions.Length) ;
		}
		                            
		TutorialBS.Instance.tutEndPos[0].position =shelf2Positions[tutPosIndex] .position+ new Vector3(0,0.2f,0);
		TutorialBS.Instance.ShowTutorial(0);
	}

	public void CompletedActionN()
	{
 
		_completedActionNoN++;
		progressBarR.SetProgressBar(_completedActionNoN/9f , true );
		
	 
		if(_completedActionNoN == 9)
		{
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.MinigameCompleted);
			StartCoroutine(nameof(LevelCompletedD));
		}
		else 
		{
			if( SoundManagerBS.Instance!=null)  SoundManagerBS.Instance.PlaySound( SoundManagerBS.Instance.ActionCompleted);
		}
		 
	}
	
	public void NextPhaseE(   )
	{
		CompletedActionN();
	}
	
	private IEnumerator LevelCompletedD()
	{
		//Debug.Log("Completed");
		GameDataBS.BCompletedMiniGameE = true;
		psLevelCompleted.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		psLevelCompleted.Stop();
		psLevelCompleted.Play();
		
		yield return new WaitForSeconds(1);
		buttonNext.SetActive(true);
		buttonHome.SetActive(false);
	}
	
	public void ButtonHomeClickedD( )
	{
		_completedActionNoN = 0;
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		StopAllCoroutines();
		GameDataBS.BCompletedMiniGameE = false;
		LevelTransitionBS.Instance.HideSceneAndLoadNextT("SelectMinigame"); 
		
		BlockClicksBs.Instance.SetBlockAllL(true);
		//Implementation.Instance.ShowInterstitial();
	}

	public void ButtonNextClickedD()
	{
		_completedActionNoN = 0;
		BlockClicksBs.Instance.SetBlockAllL(true);
		if(SoundManagerBS.Instance!=null) SoundManagerBS.Instance.Play_ButtonClickK();
		LevelTransitionBS.Instance.HideSceneAndLoadNextT("SelectMinigame");
		StopAllCoroutines();
		//Implementation.Instance.ShowInterstitial();
	}
}
