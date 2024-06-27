using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RelicDisplay : MonoBehaviour
{
    public TextMeshProUGUI relicNameText;
    public TextMeshProUGUI blessingCountText;

    public GameObject infoPanel;

    public void DabRelic(DabriaStarterRelic dabRelic)
    {
        relicNameText.text = "Sparkling Hope";
        blessingCountText.text = dabRelic.blessingCount.ToString();
    }

    public void HoverOn()
    {
        infoPanel.SetActive(true);
    }

    public void HoverOff()
    {
        infoPanel.SetActive(false);
    }
}
