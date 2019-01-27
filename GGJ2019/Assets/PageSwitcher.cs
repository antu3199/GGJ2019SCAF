using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageSwitcher : MonoBehaviour {

    public List<Image> pages;
    public int currentPage = 0;

	public void AdvancePage()
    {
        if (currentPage + 1 < pages.Count)
        {
            pages[currentPage].gameObject.SetActive(false);
            currentPage++;
            pages[currentPage].gameObject.SetActive(true);
        }

    }
}
