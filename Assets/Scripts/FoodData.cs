using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Custom/FoodData", fileName="New Food Data")]
public class FoodData : ScriptableObject
{
    [SerializeField] private float energyGranted;
    [SerializeField] private FoodData[] edibleBy;

    public float EnergyGranted => energyGranted;
    public IEnumerable<FoodData> EdibleBy => edibleBy;
}