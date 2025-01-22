using UnityEngine;

public class DevTools : MonoBehaviour
{
    MenuPanel panel;
    GuildManager gm;

    private void Start()
    {
        panel = GetComponentInChildren<MenuPanel>();
        panel.gameObject.SetActive(false);
        gm = GuildManager.instance;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            panel.gameObject.SetActive(!panel.gameObject.activeInHierarchy);
        }
    }

    public void AddCoin(int amount)
    {
        gm.AddCoin(amount);
    }

    public void GenerateCharactersForHire(int amount)
    {
        gm.GenerateCharactersForHire(amount);
    }
}
