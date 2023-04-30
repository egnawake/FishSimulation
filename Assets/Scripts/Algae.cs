using UnityEngine;

public class Algae : MonoBehaviour
{
    [SerializeField] private float energyGranted;

    public float BeEaten()
    {
        Destroy(gameObject);
        return energyGranted;
    }
}
