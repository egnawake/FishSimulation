using UnityEngine;
using UnityEngine.Events;

public class Algae : MonoBehaviour
{
    [SerializeField] private AlgaePiece algaePiecePrefab;

    private Transform nextStackTransform;
    private UnityEvent onDeath;

    public UnityEvent OnDeath => onDeath;

    private void Awake()
    {
        onDeath = new UnityEvent();
    }

    private void Start()
    {
        nextStackTransform = transform;
        Grow();
    }

    private void OnDestroy()
    {
        onDeath.Invoke();
    }

    public void Grow()
    {
        AlgaePiece piece = Instantiate(algaePiecePrefab,
            nextStackTransform.position, transform.rotation, transform);

        nextStackTransform = piece.StackTransform;
    }
}
