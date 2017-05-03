using UnityEngine;

public static class ArrayExpander {

	public static void Expand( Transform parent, int itemCount) {

		// disable any extra items we dont need
		if(parent.childCount > itemCount)
		{
			for(int ii = itemCount; ii < parent.childCount; ++ii)
			{
				parent.GetChild(ii).gameObject.SetActive(false);
			}
		}

		// create new items if we need to
		if(parent.childCount < itemCount)
		{
			for(int ii = parent.childCount; ii < itemCount; ++ii)
			{
				Object.Instantiate(parent.GetChild(0), parent);
			}
		}

		// resize the Content window to match the size of all the elements inside
		/*float new_height = playerCount * playerEntries[0].GetComponent<LayoutElement>().minHeight;
		new_height += (playerCount - 1) * scrollContent.spacing;
		new_height += 16.0f;

		RectTransform rt = scrollContent.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(rt.sizeDelta.x, new_height);

		scrollBar.size = scrollWindow.sizeDelta.y / new_height;

		CoroutineHelper.CreateOneShotCoroutine(0.05f, this, WhyCantThisWorkOnTheFirstFrameUnity);
		*/
	}


	/*void WhyCantThisWorkOnTheFirstFrameUnity()
	{
		//scrollBar.value = 1.0f;
	}*/
}
