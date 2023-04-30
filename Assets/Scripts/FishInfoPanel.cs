using UnityEngine;
using TMPro;

public class FishInfoPanel : MonoBehaviour
{
    [SerializeField] private FishController parentFish;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private Vector3 offset;

    public float Energy
    {
        set
        {
            energyText.text = $"Energy: {value.ToString("F0")}";
        }
    }

    private void Start()
    {
        Energy = parentFish.Energy;
        parentFish.EnergyChanged.AddListener(() => Energy = parentFish.Energy);
    }

    private void Update()
    {
        transform.position = parentFish.transform.position + offset;
    }
}
