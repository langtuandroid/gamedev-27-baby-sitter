﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;

/// <summary>
/// Scene: CharacterSelect
/// Object: Characters
/// Description: Used for character scrolling with snap at every character
/// </summary>

public class ScrollRectSnapControllerBS : MonoBehaviour {

	// Snapping positions list
	public List<float> snapPositions;

	// List element size
	public float cellSizeX;
	public float cellSizeY;
	public float spacing;

	// Used for checking characters snap point
	private float currentCharCheckTemp;

	public Vector3 newLerpPosition;

	// Lerping variables
	private bool lerpingG;
	public float lerpingSpeed;

	// true if we are interacting with scroll rect
	private bool holdingRect;

	public float focusedElementScale;
	public float unfocusedElementsScale;

	public List<GameObject> listOfCharacters;

	public bool horizontalList;

	public GameObject backwardButton;
	public GameObject forwardButton;

	private bool buttonPressedD;

	[FormerlySerializedAs("currentCharacter")] public int currentCharacterR;

	private void Awake()
	{
		// If we are coming from another scene and we want focus to be on some
		// other character than first one
		//		if (GlobalVariables.selectedCharacterIndex != -1)
		//			currentCharacter = GlobalVariables.selectedCharacterIndex;
		//		else
		currentCharacterR = 0;

		lerpingG = false;
		buttonPressedD = false;

		// Set size of the cell
		if (GetComponent<GridLayoutGroup>().cellSize == Vector2.zero)
		{
			Vector2 cellSize = new Vector2(cellSizeX, cellSizeY);
			GetComponent<GridLayoutGroup>().cellSize = cellSize;
		}
		else
		{
			cellSizeX = GetComponent<GridLayoutGroup>().cellSize.x;
			cellSizeY = GetComponent<GridLayoutGroup>().cellSize.y;
		}

		// Set size delta of parent scroll rect so elements wouldn't be jumpy
		transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSizeX, cellSizeY);

		if (horizontalList)
		{
			transform.parent.GetComponent<ScrollRect>().horizontal = true;
			transform.parent.GetComponent<ScrollRect>().vertical = false;

			// Check if layout spacing differes from zero vector
			if (GetComponent<GridLayoutGroup>().spacing == Vector2.zero)
			{
				Vector2 spacingVector = new Vector2(spacing, 0);
				GetComponent<GridLayoutGroup>().spacing = spacingVector;
			}
			else
			{
				if (GetComponent<GridLayoutGroup>().spacing.x != 0)
					spacing = GetComponent<GridLayoutGroup>().spacing.x;

				Vector2 spacingVector = new Vector2(spacing, 0);
			}

			GetComponent<GridLayoutGroup>().startAxis = GridLayoutGroup.Axis.Vertical;
			GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedRowCount;
			GetComponent<GridLayoutGroup>().constraintCount = 1;
			currentCharCheckTemp = (cellSizeX + spacing) / 2;
		}
		else
		{
			transform.parent.GetComponent<ScrollRect>().horizontal = false;
			transform.parent.GetComponent<ScrollRect>().vertical = true;

			if (GetComponent<GridLayoutGroup>().spacing == Vector2.zero)
			{
				Vector2 spacingVector = new Vector2(0, spacing);
				GetComponent<GridLayoutGroup>().spacing = spacingVector;
			}
			else
			{
				if (GetComponent<GridLayoutGroup>().spacing.y != 0)
					spacing = GetComponent<GridLayoutGroup>().spacing.y;
				
				Vector2 spacingVector = new Vector2(0, spacing);
			}

			GetComponent<GridLayoutGroup>().startAxis = GridLayoutGroup.Axis.Horizontal;
			GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			GetComponent<GridLayoutGroup>().constraintCount = 1;
			currentCharCheckTemp = (cellSizeY + spacing) / 2;
		}

		snapPositions.Clear();
		snapPositions = new List<float>();

//		if (currentCharacter == 0)
//			leftArrow.SetActive(false);
//		else if (currentCharacter == 5)
//			rightArrow.SetActive(false);

		// Get all characters and put then into list
		foreach(Transform t in transform)
			listOfCharacters.Add(t.gameObject);

		// Set transform rect position and size depending of number of characters and spacing
		if (horizontalList)
		{
			GetComponent<RectTransform>().sizeDelta = new Vector2(listOfCharacters.Count * cellSizeX + (listOfCharacters.Count - 1) * spacing, cellSizeY);
			GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().sizeDelta.x - 2 * spacing, GetComponent<RectTransform>().anchoredPosition.y);

			float startSnapPosition = GetComponent<RectTransform>().sizeDelta.x / 2 - cellSizeX / 2;
			snapPositions.Add(startSnapPosition);

			// Set fist character to be of focused scale
			listOfCharacters[0].transform.localScale = new Vector3(focusedElementScale, focusedElementScale, 1);
			
			for (int i = 1; i < listOfCharacters.Count; i++)
			{
				startSnapPosition -= cellSizeX + spacing;
				snapPositions.Add(startSnapPosition);
				
				// Set scale for not focused elements to be scale
				listOfCharacters[i].transform.localScale = new Vector3(unfocusedElementsScale, unfocusedElementsScale, 1);
			}
		}
		else
		{
			GetComponent<RectTransform>().sizeDelta = new Vector2(cellSizeX, listOfCharacters.Count * cellSizeY + (listOfCharacters.Count - 1) * spacing);
			GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, -(GetComponent<RectTransform>().sizeDelta.y - 2 * spacing));

			float startSnapPosition = GetComponent<RectTransform>().sizeDelta.y / 2 - cellSizeY / 2;
			snapPositions.Add(startSnapPosition);

			// Set fist character to be of focused scale
			listOfCharacters[0].transform.localScale = new Vector3(focusedElementScale, focusedElementScale, 1);
			
			for (int i = 1; i < listOfCharacters.Count; i++)
			{
				startSnapPosition -= cellSizeY + spacing;
				snapPositions.Add(startSnapPosition);
				
				// Set scale for not focused elements to be scale
				listOfCharacters[i].transform.localScale = new Vector3(unfocusedElementsScale, unfocusedElementsScale, 1);
			}
		}
	}

	// Determining closesst snap point -349 is half distance - 1 and 350 is half distance
	private void SetLerpPositionToClosestSnapPoint()
	{
		for (int i = 0; i < snapPositions.Count; i++)
		{
			if (horizontalList)
			{
				if (transform.localPosition.x > snapPositions[i] - currentCharCheckTemp - 1 && transform.localPosition.x <= snapPositions[i] + currentCharCheckTemp)
				{
					newLerpPosition = new Vector3(snapPositions[i], 0, 0);
					lerpingG = true;
					currentCharacterR = i;
					break;
				}
			}
			else
			{
				if (transform.localPosition.y > snapPositions[i] - currentCharCheckTemp - 1 && transform.localPosition.y <= snapPositions[i] + currentCharCheckTemp)
				{
					newLerpPosition = new Vector3(0, snapPositions[i], 0);
					lerpingG = true;
					currentCharacterR = listOfCharacters.Count - i - 1;
					break;
				}
			}
		}
	}

	private void SetCurrentCharacter()
	{
		for (int i = 0; i < snapPositions.Count; i++)
		{
			if (horizontalList)
			{
				if (transform.localPosition.x > snapPositions[i] - currentCharCheckTemp - 1 && transform.localPosition.x <= snapPositions[i] + currentCharCheckTemp)
				{
					currentCharacterR = i;
					break;
				}
			}
			else
			{
				if (transform.localPosition.y > snapPositions[i] - currentCharCheckTemp - 1 && transform.localPosition.y <= snapPositions[i] + currentCharCheckTemp)
				{
					currentCharacterR = listOfCharacters.Count - i - 1;
					break;
				}
			}
		}

		//numberOfDressedCharacter.text= (currentCharacter + 1).ToString() + "/6";
	}

	// This function purpouse is to wait a little before pressing button again
	private IEnumerator ButtonPressed()
	{
		yield return new WaitForSeconds(0.4f);
		buttonPressedD = false;
	}

	public void BackwardButtonPressedD()
	{
		if (horizontalList)
		{
			if (currentCharacterR > 0 && !buttonPressedD)
			{
				// Button pressed
				buttonPressedD = true;

				currentCharacterR -= 1;
				newLerpPosition = new Vector3(snapPositions[currentCharacterR], transform.localPosition.y, 0);
				lerpingG = true;

				StartCoroutine(ButtonPressed());
			}
		}
		else
		{
			if (currentCharacterR > 0 && !buttonPressedD)
			{
				// Button pressed
				buttonPressedD = true;
				
				currentCharacterR -= 1;
				newLerpPosition = new Vector3(transform.localPosition.x, snapPositions[listOfCharacters.Count - currentCharacterR - 1], 0);
				lerpingG = true;
				
				StartCoroutine(ButtonPressed());
			}
		}
	}

	public void ForwardButtonPressedD()
	{
		if (horizontalList)
		{
			if (currentCharacterR < snapPositions.Count - 1 && !buttonPressedD)
			{
				// Button pressed
				buttonPressedD = true;

				currentCharacterR += 1;
				newLerpPosition = new Vector3(snapPositions[currentCharacterR], transform.localPosition.y, 0);
				lerpingG = true;

				StartCoroutine(ButtonPressed());
			}
		}
		else
		{
			if (currentCharacterR < listOfCharacters.Count - 1 && !buttonPressedD)
			{
				// Button pressed
				buttonPressedD = true;
				
				currentCharacterR += 2;
				newLerpPosition = new Vector3(transform.localPosition.x, snapPositions[listOfCharacters.Count - currentCharacterR], 0);
				lerpingG = true;
				
				StartCoroutine(ButtonPressed());
			}
		}
	}

	public void SetButtonActiveE(GameObject button)
	{
		Color c = button.GetComponent<Image>().color;
		c = new Color(1, 1, 1, 1);
		button.GetComponent<Image>().color = c;

		button.GetComponent<Button>().interactable = true;

	}

	public void SetButtonInactive(GameObject button)
	{
		Color c = button.GetComponent<Image>().color;
		c = new Color(1, 1, 1, 0.3f);
		button.GetComponent<Image>().color = c;
		
		button.GetComponent<Button>().interactable = false;
	}

	private void Update()
	{
		// If we are holding button than do not lerp
		if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && !buttonPressedD)
		{
			holdingRect = true;
			SetCurrentCharacter();
			newLerpPosition = transform.localPosition;
		}

		if (Input.GetMouseButtonUp(0))
			holdingRect = false;

		// If not lerping and velocityis small enough find closest snap point and lerp to it
		if (horizontalList)
		{
			if (!lerpingG && !holdingRect && Mathf.Abs(transform.parent.GetComponent<ScrollRect>().velocity.x) >= 0f && Mathf.Abs(transform.parent.GetComponent<ScrollRect>().velocity.x) < 100f)
			{
				SetLerpPositionToClosestSnapPoint();
			}
			else
			{
				SetCurrentCharacter();
			}
		}
		else
		{
			if (!lerpingG && !holdingRect && Mathf.Abs(transform.parent.GetComponent<ScrollRect>().velocity.y) >= 0f && Mathf.Abs(transform.parent.GetComponent<ScrollRect>().velocity.y) < 100f)
			{
				SetLerpPositionToClosestSnapPoint();
			}
			else
			{
				SetCurrentCharacter();
			}
		}

		// Set appropriate for elements in list according to distance from current snap point
		if (horizontalList)
		{
			if(currentCharacterR == 0)
			{
				float sb = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacterR] - transform.localPosition.x - currentCharCheckTemp * 2) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);
				float s = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacterR] - transform.localPosition.x) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);

				if (s <= unfocusedElementsScale)
					s = unfocusedElementsScale;

				if (sb <= unfocusedElementsScale)
					sb = unfocusedElementsScale;

				listOfCharacters[currentCharacterR].transform.localScale = new Vector3(s, s, 1);

				listOfCharacters[currentCharacterR + 1].transform.localScale = new Vector3(sb, sb, 1);
			}
			else if(currentCharacterR == listOfCharacters.Count - 1)
			{
				float s = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacterR] - transform.localPosition.x) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);
				float sf = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacterR] - transform.localPosition.x + currentCharCheckTemp * 2) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);

				if (s <= unfocusedElementsScale)
					s = unfocusedElementsScale;

				if (sf <= unfocusedElementsScale)
					sf = unfocusedElementsScale;

				listOfCharacters[currentCharacterR - 1].transform.localScale = new Vector3(sf, sf, 1);
				listOfCharacters[currentCharacterR].transform.localScale = new Vector3(s, s, 1);
			}
			else
			{
				float sb = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacterR] - transform.localPosition.x - currentCharCheckTemp * 2) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);
             	float s = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacterR] - transform.localPosition.x) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);
                float sf = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacterR] - transform.localPosition.x + currentCharCheckTemp * 2) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);

				if (s <= unfocusedElementsScale)
					s = unfocusedElementsScale;
				
				if (sb <= unfocusedElementsScale)
					sb = unfocusedElementsScale;

				if (sf <= unfocusedElementsScale)
					sf = unfocusedElementsScale;

				listOfCharacters[currentCharacterR - 1].transform.localScale = new Vector3(sf, sf, 1);
				listOfCharacters[currentCharacterR].transform.localScale = new Vector3(s, s, 1);
				listOfCharacters[currentCharacterR + 1].transform.localScale = new Vector3(sb, sb, 1);
			}
		}
		else
		{
			if(currentCharacterR == 0)
			{
				float sb = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacterR] + transform.localPosition.y - currentCharCheckTemp * 2) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);
             	float s = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacterR] + transform.localPosition.y) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);

				if (s <= unfocusedElementsScale)
					s = unfocusedElementsScale;
				
				if (sb <= unfocusedElementsScale)
					sb = unfocusedElementsScale;

				listOfCharacters[currentCharacterR].transform.localScale = new Vector3(s, s, 1);
				listOfCharacters[currentCharacterR + 1].transform.localScale = new Vector3(sb, sb, 1);
			}
			else if(currentCharacterR == listOfCharacters.Count - 1)
			{
				float s = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacterR] + transform.localPosition.y) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);
                float sf = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacterR] + transform.localPosition.y + currentCharCheckTemp * 2) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);

				if (s <= unfocusedElementsScale)
					s = unfocusedElementsScale;
				
				if (sf <= unfocusedElementsScale)
					sf = unfocusedElementsScale;

				listOfCharacters[currentCharacterR - 1].transform.localScale = new Vector3(sf, sf, 1);
				listOfCharacters[currentCharacterR].transform.localScale = new Vector3(s, s, 1);
			}
			else
			{
				float sb = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacterR] + transform.localPosition.y - currentCharCheckTemp * 2) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);
         		float s = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacterR] + transform.localPosition.y) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);
           		float sf = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacterR] + transform.localPosition.y + currentCharCheckTemp * 2) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);

				if (s <= unfocusedElementsScale)
					s = unfocusedElementsScale;
				
				if (sb <= unfocusedElementsScale)
					sb = unfocusedElementsScale;
				
				if (sf <= unfocusedElementsScale)
					sf = unfocusedElementsScale;

				listOfCharacters[currentCharacterR - 1].transform.localScale = new Vector3(sf, sf, 1);
				listOfCharacters[currentCharacterR].transform.localScale = new Vector3(s, s, 1);
				listOfCharacters[currentCharacterR + 1].transform.localScale = new Vector3(sb, sb, 1);
			}
		}

		// If we let the mouse button and velocity small enough
		if (lerpingG)
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, newLerpPosition, lerpingSpeed);

			if (Vector3.Distance(transform.localPosition, newLerpPosition) < 1f)
			{
				transform.localPosition = newLerpPosition;
				transform.parent.GetComponent<ScrollRect>().velocity = new Vector3(0, 0, 0);
				lerpingG = false;

				for (int i = 0; i < listOfCharacters.Count; i++)
				{
					if (i != currentCharacterR)
						listOfCharacters[i].transform.localScale = new Vector3(unfocusedElementsScale, unfocusedElementsScale, 1);
				}

			}
		}

		if (horizontalList)
		{
			// Updating arrow buttons
			if (transform.localPosition.x > snapPositions[snapPositions.Count - 1] + spacing / 2)
			{
				SetButtonActiveE(forwardButton);
			}
			else
			{
				SetButtonInactive(forwardButton);
			}

			if (transform.localPosition.x < snapPositions[0] - spacing / 2)
			{
				SetButtonActiveE(backwardButton);
			}
			else
			{
				SetButtonInactive(backwardButton);
			}
		}
		else
		{
			// Updating arrow buttons
			if (transform.localPosition.y > snapPositions[snapPositions.Count - 1] + spacing / 2)
			{
				SetButtonActiveE(backwardButton);
			}
			else
			{
				SetButtonInactive(backwardButton);
			}
			
			if (transform.localPosition.y < snapPositions[0] - spacing / 2)
			{
				SetButtonActiveE(forwardButton);
			}
			else
			{
				SetButtonInactive(forwardButton);
			}
		}
	}

}
