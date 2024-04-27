using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RelicDisplay : MonoBehaviour
{
    public TextMeshProUGUI relicNameText;
    public TextMeshProUGUI blessingCountText;

    public void DabRelic(DabriaStarterRelic dabRelic)
    {
        relicNameText.text = "Tiny Hope";
        blessingCountText.text = dabRelic.blessingCount.ToString();
    }
}
