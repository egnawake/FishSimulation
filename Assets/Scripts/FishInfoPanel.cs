using UnityEngine;
using TMPro;

public class FishInfoPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text energyText;

    public float Energy {
        set
        {
            energyText.text = $"Energy: {value.ToString("F0")}";
        }
    }
}
