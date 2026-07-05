using UnityEngine;
using TMPro;

public class PinParent : MonoBehaviour
{
    public TextMeshProUGUI countText;

    public int FallenPinsCount()
    {
        int fallenPins = 0;
        Pin[] pins = GetComponentsInChildren<Pin>();

        foreach (Pin pin in pins)
        {
            if(pin.IsFallen){
                fallenPins++;
            }
        }
        return fallenPins;
    }
}
