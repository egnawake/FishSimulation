using UnityEngine;
using System.Collections;

public class AlgaePatch : MonoBehaviour
{
    [SerializeField] private Algae algaePrefab;
    [SerializeField] private float growthChance = 0.5f;
    [SerializeField] private float growthRate = 1f;
    [SerializeField] private float maxAlgaePieces = 5f;

    private YieldInstruction growthWait;
    private Algae algae;
    private int algaePieces = 0;

    private void Start()
    {
        growthWait = new WaitForSeconds(growthRate);
        StartCoroutine(UpdatePatch());
    }

    private IEnumerator UpdatePatch()
    {
        while (true)
        {
            if (Random.Range(0f, 1f) < growthChance)
            {
                Grow();
            }

            if (algaePieces >= maxAlgaePieces)
            {
                Destroy(algae.gameObject);
            }

            yield return growthWait;
        }
    }

    private void Grow()
    {
        if (algae == null)
        {
            algae = Instantiate(algaePrefab, transform);
            algae.OnDeath.AddListener(ResetAlgae);
            algaePieces += 1;
        }
        else
        {
            algae.Grow();
            algaePieces += 1;
        }
    }

    private void ResetAlgae()
    {
        algae = null;
        algaePieces = 0;
    }
}
