using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;

public class SetBabyAtlasBS : MonoBehaviour {
	
	[FormerlySerializedAs("Baby1")] [SerializeField] private Sprite[] baby1;
	[FormerlySerializedAs("Baby2")] [SerializeField] private Sprite[] baby2;
	[FormerlySerializedAs("Baby3")] [SerializeField] private Sprite[] baby3;
	[FormerlySerializedAs("Baby4")] [SerializeField] private Sprite[] baby4;
    
	[FormerlySerializedAs("Body")] [SerializeField] private Image body;
	[FormerlySerializedAs("Head")] [SerializeField] private Image head;

	[FormerlySerializedAs("EyebrowLeft")] [SerializeField] private Image eyebrowLeft;
	[FormerlySerializedAs("EyeWhiteLeft")] [SerializeField] private Image eyeWhiteLeft;
	[FormerlySerializedAs("EyeBallLeft")] [SerializeField] private Image eyeBallLeft;

	[FormerlySerializedAs("EyeLidlLeft1")] [SerializeField] private Image eyeLidlLeft1;
	[FormerlySerializedAs("EyeLidlLeft2")] [SerializeField] private Image eyeLidlLeft2;
	[FormerlySerializedAs("EyeLidlLeft3")] [SerializeField] private Image eyeLidlLeft3;
	[FormerlySerializedAs("EyeLidlLeft4")] [SerializeField] private Image eyeLidlLeft4;
	
	[FormerlySerializedAs("EyebrowRight")] [SerializeField] private Image eyebrowRight;
	[FormerlySerializedAs("EyeWhiteRight")] [SerializeField] private Image eyeWhiteRight;
	[FormerlySerializedAs("EyeBallRight")] [SerializeField] private Image eyeBallRight;

	[FormerlySerializedAs("EyeLidlRight1")] [SerializeField] private Image eyeLidlRight1;
	[FormerlySerializedAs("EyeLidlRight2")] [SerializeField] private Image eyeLidlRight2;
	[FormerlySerializedAs("EyeLidlRight3")] [SerializeField] private Image eyeLidlRight3;
	[FormerlySerializedAs("EyeLidlRight4")] [SerializeField] private Image eyeLidlRight4;

	[FormerlySerializedAs("EatMouth4")] [SerializeField] private Image eatMouth4;
	[FormerlySerializedAs("EatMouth2")] [SerializeField] private Image eatMouth2;
	[FormerlySerializedAs("EatMouth1")] [SerializeField] private Image eatMouth1;
	[FormerlySerializedAs("EatMouth6")] [SerializeField] private Image eatMouth6;

	[FormerlySerializedAs("IdleMouth3")] [SerializeField] private Image idleMouth3;
	[FormerlySerializedAs("IdleMouth1")] [SerializeField] private Image idleMouth1;

	[FormerlySerializedAs("CryMouth0")] [SerializeField] private Image cryMouth0;
	[FormerlySerializedAs("CryMouth1")] [SerializeField] private Image cryMouth1;
	[FormerlySerializedAs("CryMouth2")] [SerializeField] private Image cryMouth2;

	[FormerlySerializedAs("MouthBT1")] [SerializeField] private Image mouthBt1;
	[FormerlySerializedAs("MouthBT2")] [SerializeField] private Image mouthBt2;

	[FormerlySerializedAs("BabyClothes")] [SerializeField] private Image babyClothes;
	[FormerlySerializedAs("BabyDiaper")] [SerializeField] private Image babyDiaper;

	[FormerlySerializedAs("Nose")] [SerializeField] private Image nose;

	[FormerlySerializedAs("Bib")] [SerializeField] private Image bib;
	 
	[FormerlySerializedAs("RedSpots1")] [SerializeField] private GameObject redSpots1;
	[FormerlySerializedAs("RedSpots2")] [SerializeField] private GameObject redSpots2;
	[FormerlySerializedAs("Dirt")] [SerializeField] private GameObject dirt;
	[FormerlySerializedAs("Poop")] [SerializeField] private GameObject poop;
	[FormerlySerializedAs("CleanNose")] [SerializeField] private GameObject cleanNose;
	
	private int indexBB = 0;
	private int mgIndexX = 0;

	public void SetBabyY(int babyIndex)
	{
		Sprite[] babySprites;
		if(babyIndex == 1) babySprites =  baby1;
		else if(babyIndex == 2) babySprites =  baby2;
		else if(babyIndex == 3) babySprites =  baby3;
		 else babySprites =  baby4;


		if(body!=null) body.sprite = babySprites[1];
		if(head!=null) head.sprite = babySprites[0];
		
		if(eyebrowLeft!=null) eyebrowLeft.sprite = babySprites[3];
		if(eyeWhiteLeft!=null) eyeWhiteLeft.sprite = babySprites[8];
		if(eyeBallLeft!=null) eyeBallLeft.sprite = babySprites[2];
		
		if(eyeLidlLeft1!=null) eyeLidlLeft1.sprite = babySprites[6];
		if(eyeLidlLeft2!=null) eyeLidlLeft2.sprite = babySprites[7];
		if(eyeLidlLeft3!=null) eyeLidlLeft3.sprite = babySprites[4];
		if(eyeLidlLeft4!=null) eyeLidlLeft4.sprite = babySprites[5];
		
		if(eyebrowRight!=null) eyebrowRight.sprite = babySprites[3];
		if(eyeWhiteRight!=null) eyeWhiteRight.sprite = babySprites[8];
		if(eyeBallRight!=null) eyeBallRight.sprite = babySprites[2];
		
		if(eyeLidlRight1!=null) eyeLidlRight1.sprite = babySprites[6];
		if(eyeLidlRight2!=null) eyeLidlRight2.sprite = babySprites[7];
		if(eyeLidlRight3!=null) eyeLidlRight3.sprite = babySprites[4];
		if(eyeLidlRight4!=null) eyeLidlRight4.sprite = babySprites[5];
		
		if(eatMouth4!=null) eatMouth4.sprite = babySprites[13];
		if(eatMouth2!=null) eatMouth2.sprite = babySprites[11];
		if(eatMouth1!=null) eatMouth1.sprite = babySprites[10];
		if(eatMouth6!=null) eatMouth6.sprite = babySprites[15];
		
		if(idleMouth3!=null) idleMouth3.sprite = babySprites[12];
		if(idleMouth1!=null) idleMouth1.sprite = babySprites[10];
		
		if(cryMouth0!=null) cryMouth0.sprite = babySprites[16];
		if(cryMouth1!=null) cryMouth1.sprite = babySprites[14];
		if(cryMouth2!=null) cryMouth2.sprite = babySprites[17];
		
		if(mouthBt1!=null) mouthBt1.sprite = babySprites[13];
		if(mouthBt2!=null) mouthBt2.sprite = babySprites[11];
		
		if(babyClothes!=null) babyClothes.sprite = babySprites[20];
		if(babyDiaper!=null) babyDiaper.sprite = babySprites[19];

		 
		nose.sprite = babySprites[18];
		if(bib!=null) bib.sprite = babySprites[21];
		//Bib;
 

	}

	public void SetMiniGameE(int mgIndex)
	{
		switch(mgIndex)
		{
		case 	0: //mg1 - presvlacenje
			if(babyClothes!=null)  babyClothes.gameObject.SetActive(true);
			if(bib!=null) bib.gameObject.SetActive(false);
			if(redSpots1!=null) redSpots1.gameObject.SetActive(false);
			if(redSpots2!=null) redSpots2.gameObject.SetActive(false);
			if(babyDiaper!=null) babyDiaper.gameObject.SetActive(true);
			if(dirt!=null) dirt.gameObject.SetActive(true);
			if(cleanNose!=null) cleanNose.gameObject.SetActive(false);
			if(poop!=null) poop.gameObject.SetActive(false);
			break;
		
		case 	1: //mg2 - kupanje
			if(babyClothes!=null)  babyClothes.gameObject.SetActive(false);
			if(bib!=null) bib.gameObject.SetActive(false);
			if(redSpots1!=null) redSpots1.gameObject.SetActive(false);
			if(redSpots2!=null) redSpots2.gameObject.SetActive(false);
			if(babyDiaper!=null) babyDiaper.gameObject.SetActive(true);
			if(dirt!=null) dirt.gameObject.SetActive(false);
			if(cleanNose!=null) cleanNose.gameObject.SetActive(false);
			if(poop!=null) poop.gameObject.SetActive(false);
			break;

		case 2: //mg3 - uspavljivanje
			if(babyClothes!=null)  babyClothes.gameObject.SetActive(true);
			if(bib!=null) bib.gameObject.SetActive(false);
			if(redSpots1!=null) redSpots1.gameObject.SetActive(false);
			if(redSpots2!=null) redSpots2.gameObject.SetActive(false);
			if(babyDiaper!=null) babyDiaper.gameObject.SetActive(false);
			if(dirt!=null) dirt.gameObject.SetActive(false);
			if(cleanNose!=null) cleanNose.gameObject.SetActive(false);
			if(poop!=null) poop.gameObject.SetActive(false);
			break;
		case 3: //mg4 - ciscenje
			if(babyClothes!=null)  babyClothes.gameObject.SetActive(false);
			if(bib!=null) bib.gameObject.SetActive(false);
			if(redSpots1!=null) redSpots1.gameObject.SetActive(true);
			if(redSpots2!=null) redSpots2.gameObject.SetActive(true);
			if(babyDiaper!=null) babyDiaper.gameObject.SetActive(true);
			if(dirt!=null) dirt.gameObject.SetActive(false);
			if(cleanNose!=null) cleanNose.gameObject.SetActive(true);
			if(poop!=null) poop.gameObject.SetActive(true);

			break;
		case 	4: //mg5A i mg5B - hranjenje
			if(babyClothes!=null)  babyClothes.gameObject.SetActive(false);
			if(bib!=null) bib.gameObject.SetActive(true);
			if(redSpots1!=null) redSpots1.gameObject.SetActive(false);
			if(redSpots2!=null) redSpots2.gameObject.SetActive(false);
			if(babyDiaper!=null) babyDiaper.gameObject.SetActive(true);
			if(dirt!=null) dirt.gameObject.SetActive(false);
			if(cleanNose!=null) cleanNose.gameObject.SetActive(false);
			if(poop!=null) poop.gameObject.SetActive(false);
			break;
		case 5: //mg6 - igracke
			if(babyClothes!=null)  babyClothes.gameObject.SetActive(true);
			if(bib!=null) bib.gameObject.SetActive(false);
			if(redSpots1!=null) redSpots1.gameObject.SetActive(false);
			if(redSpots2!=null) redSpots2.gameObject.SetActive(false);
			if(babyDiaper!=null) babyDiaper.gameObject.SetActive(true);
			if(dirt!=null) dirt.gameObject.SetActive(false);
			if(cleanNose!=null) cleanNose.gameObject.SetActive(false);
			if(poop!=null) poop.gameObject.SetActive(false);
			break;
		case 6: //mg7- komarci
			if(babyClothes!=null)  babyClothes.gameObject.SetActive(false);
			if(bib!=null) bib.gameObject.SetActive(false);
			if(redSpots1!=null) redSpots1.gameObject.SetActive(false);
			if(redSpots2!=null) redSpots2.gameObject.SetActive(false);
			if(babyDiaper!=null) babyDiaper.gameObject.SetActive(true);
			if(dirt!=null) dirt.gameObject.SetActive(false);
			if(cleanNose!=null) cleanNose.gameObject.SetActive(false);
			if(poop!=null) poop.gameObject.SetActive(false);
			break;
		case 7: //mg8- ploce
			if(babyClothes!=null)  babyClothes.gameObject.SetActive(true);
			if(bib!=null) bib.gameObject.SetActive(false);
			if(redSpots1!=null) redSpots1.gameObject.SetActive(false);
			if(redSpots2!=null) redSpots2.gameObject.SetActive(false);
			if(babyDiaper!=null) babyDiaper.gameObject.SetActive(false);
			if(dirt!=null) dirt.gameObject.SetActive(false);
			if(cleanNose!=null) cleanNose.gameObject.SetActive(false);
			if(poop!=null) poop.gameObject.SetActive(false);
			break;
		}
	}

	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			indexBB++;
			if(indexBB >3) indexBB = 0;
			SetBabyY(indexBB);
		}
		if(Input.GetKeyDown(KeyCode.A))
		{

			SetMiniGameE(indexBB);
			mgIndexX++;
			if(mgIndexX >8) indexBB = 0;
		}
	}
	 
}
