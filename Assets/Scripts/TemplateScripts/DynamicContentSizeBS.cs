using System.Collections;
using UnityEngine;

namespace TemplateScripts
{
	public class DynamicContentSizeBS : MonoBehaviour {
		
		[SerializeField] private bool isVertical = true;

		[SerializeField] private float itemSpacing=10f;

		[SerializeField] private float itemSize;

		public void SetSizeAndChild()
		{
			StartCoroutine (WaitAndWork ());
		}

		private IEnumerator WaitAndWork ()
		{
			yield return new WaitForEndOfFrame ();
			
			if (transform.childCount > 0) {
				if (isVertical) {
					gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (gameObject.GetComponent<RectTransform> ().sizeDelta.x, 1f);
					int childCount = gameObject.transform.childCount;
					gameObject.transform.GetChild (0).GetComponent<RectTransform> ().localScale = Vector3.one;
					itemSize = gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta.y;
					Vector2 newSize = new Vector2 ();
					newSize.x = gameObject.GetComponent<RectTransform> ().sizeDelta.x;
					newSize.y = (childCount + 1) * itemSpacing + childCount * itemSize;
					gameObject.GetComponent<RectTransform> ().sizeDelta = newSize;
					float startPositionY = newSize.y / 2 - itemSpacing - itemSize / 2;
				
					for (int i=0; i<gameObject.transform.childCount; i++) {//for every child
						gameObject.transform.GetChild (i).GetComponent<RectTransform> ().localScale = Vector3.one;
						gameObject.transform.GetChild (i).GetComponent<RectTransform> ().sizeDelta = new Vector2 (newSize.x, itemSize);
						gameObject.transform.GetChild (i).transform.localPosition = new Vector3 (0, startPositionY, 0);
						startPositionY -= itemSpacing + itemSize;
					
					}

				}
				else {
					gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (1f, gameObject.GetComponent<RectTransform> ().sizeDelta.y);
					int childCount = gameObject.transform.childCount;
					gameObject.transform.GetChild (0).GetComponent<RectTransform> ().localScale = Vector3.one;
					itemSize = gameObject.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta.x;
					Vector2 newSize = new Vector2 ();
					newSize.y = gameObject.GetComponent<RectTransform> ().sizeDelta.y;
					newSize.x = (childCount + 1) * itemSpacing + childCount * itemSize;
					gameObject.GetComponent<RectTransform> ().sizeDelta = newSize;
					float startPositionX = newSize.x / 2 - itemSpacing - itemSize / 2;
				
					for (int i=0; i<gameObject.transform.childCount; i++) {//for every child
						gameObject.transform.GetChild (i).GetComponent<RectTransform> ().localScale = Vector3.one;
						gameObject.transform.GetChild (i).GetComponent<RectTransform> ().sizeDelta = new Vector2 (itemSize, newSize.y);
						gameObject.transform.GetChild (i).transform.localPosition = new Vector3 (startPositionX, 0, 0);
						startPositionX -= itemSpacing + itemSize;
					
					}
				
				}
			}
		}
	}
}
