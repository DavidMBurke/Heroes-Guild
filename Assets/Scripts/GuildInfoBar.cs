using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuildInfoBar : MonoBehaviour
{
    public TextMeshProUGUI yearText = null!;
    public TextMeshProUGUI dayText = null!;
    public TextMeshProUGUI seasonText = null!;
    public TextMeshProUGUI timeText = null!;
    public TextMeshProUGUI coinText = null!;
    private GuildManager gm = null!;

    private void Start()
    {
        gm = GuildManager.instance;   
    }
    private void Update()
    {
        yearText.text = gm.year.ToString();
        dayText.text = gm.day.ToString();
        switch (gm.season)
            {
                case 0: seasonText.text = "Spring";
                break;
                case 1: seasonText.text = "Summer";
                break;
                case 2: seasonText.text = "Fall";
                break;
                case 3: seasonText.text = "Winter";
                break;
                default: Debug.LogWarning("Invalid season value");
                break;
            };
        timeText.text = $"{gm.hour:D2}:{gm.minute:D2}";
        coinText.text = gm.coin.ToString();
    }
}
