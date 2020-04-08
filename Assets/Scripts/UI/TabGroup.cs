using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour {
    public List<TabButton> tabButtons;
    public Color idle, hover, active;
    public TabButton selectedButton;
    public List<GameObject> objectsToSwap;
    public bool shouldSwapObjects = false;
    public PanelGroup panelGroup;
    public void Subscribe(TabButton button) {
        if (tabButtons == null) {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)  {
        ResetTabs();
        if(selectedButton == null || button != selectedButton)
            button.background.color = hover;
    }

    public void OnTabExit(TabButton button) {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button) {
        if (selectedButton != null) selectedButton.Deselect();
        selectedButton = button;
        selectedButton.Select();
        ResetTabs();
        button.background.color = active;
        if (shouldSwapObjects) {
            SwapObjects(button);
        }
        else {
            if (panelGroup != null) {
                panelGroup.SetPageIndex(button.transform.GetSiblingIndex());
            }
        }
    }

    private void SwapObjects(TabButton button) {
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++) {
            if (i == index) objectsToSwap[i].SetActive(true);
            else objectsToSwap[i].SetActive(false);
        }
    }

    public void ResetTabs() {
        foreach (var button in tabButtons) {
            if (selectedButton != null && button == selectedButton) continue;
            button.background.color = idle;
        }
    }
}
