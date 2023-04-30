using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private FoodData foodData;

    public FoodData Data => foodData;

    public void BeEaten()
    {
        Destroy(gameObject);
    }
}
