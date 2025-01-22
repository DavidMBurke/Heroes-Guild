using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GuildMenu : MonoBehaviour
{
    List<MenuPanel> panels = new List<MenuPanel>();
    public MenuPanel startingPanel;

    private void Start()
    {
        panels = GetComponentsInChildren<MenuPanel>().ToList();
        GoToPage(startingPanel);
    }

    public void GoToPage(MenuPanel selectedPanel)
    {
        foreach (MenuPanel panel in panels)
        {
            if (panel != selectedPanel)
            {
                panel.gameObject.SetActive(false);
            }
        }
        selectedPanel.gameObject.SetActive(true);
    }
}
