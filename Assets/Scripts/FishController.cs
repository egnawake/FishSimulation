using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using LibGameAI.FSMs;
using AIUnityExamples.Movement.Dynamic;

public class FishController : MonoBehaviour
{
    [SerializeField] private float detectionRange = 1f;
    [SerializeField] private float eatingRange = 0.5f;
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float energyLossRate = 1f;
    [SerializeField] private float reproductionEnergyThreshold = 50f;
    [SerializeField] private SteeringBehaviour seekBehavior;
    [SerializeField] private SteeringBehaviour fleeBehavior;
    [SerializeField] private GameObject childPrefab;

    private DynamicAgent dagent;
    private StateMachine fsm;
    private Food food;

    private float energy;
    private Food foodTarget;
    private Food enemyTarget;

    private UnityEvent energyChanged;

    public float Energy
    {
        get => energy;
        set
        {
            energy = Mathf.Clamp(value, 0f, maxEnergy);

            energyChanged.Invoke();
        }
    }

    public UnityEvent EnergyChanged => energyChanged;

    private void Awake()
    {
        energyChanged = new UnityEvent();
    }

    private void Start()
    {
        dagent = GetComponent<DynamicAgent>();
        food = GetComponent<Food>();

        // States
        Action wanderActions = Reproduce;
        State wanderState = new State("Wander",
            null,
            wanderActions,
            null);

        Action huntingActions = Eat;
        huntingActions += Reproduce;
        State huntingState = new State("Hunting",
            null,
            huntingActions,
            null);

        State runState = new State("Run",
            null,
            null,
            null);

        // Transitions from "wander"
        Transition wander2Run = new Transition(
            () => enemyTarget != null,
            StartFleeing,
            runState);
        wanderState.AddTransition(wander2Run);

        Transition wander2Hunting = new Transition(
            () => foodTarget != null,
            StartHunting,
            huntingState);
        wanderState.AddTransition(wander2Hunting);

        // Transitions from "hunting"
        Transition hunting2Wander = new Transition(
            () => foodTarget == null,
            null,
            wanderState);
        huntingState.AddTransition(hunting2Wander);

        Transition hunting2Run = new Transition(
            () => enemyTarget != null,
            StartFleeing,
            runState);
        huntingState.AddTransition(hunting2Run);

        // Transitions from "run"
        Transition run2Wander = new Transition(
            () => enemyTarget == null,
            null,
            wanderState);
        runState.AddTransition(run2Wander);

        fsm = new StateMachine(wanderState);
    }

    void Update()
    {
        Action actionToDo = fsm.Update();
        actionToDo?.Invoke();

        LoseEnergy();
        UpdateEntitiesInRange();
    }

    private void UpdateEntitiesInRange()
    {
        // Reset targets
        foodTarget = null;
        enemyTarget = null;

        // Find colliders of entities in range
        Collider2D[] entitiesInRange = Physics2D.OverlapCircleAll(transform.position,
            detectionRange);

        // Sort entities by distance
        IEnumerable<GameObject> sortedEntities = entitiesInRange
            .Select(collider => collider.gameObject)
            .Where(gameObject => this.gameObject != gameObject)
            .OrderBy(gameObject => (transform.position - gameObject.transform.position).magnitude);

        // Get closest (first) food in entities
        Food potentialFood = null;
        foreach (GameObject entity in sortedEntities)
        {
            potentialFood = entity.GetComponent<Food>();
            if (potentialFood != null) break;
        }

        if (potentialFood != null)
        {
            // If food is edible by us, register it as food
            foreach (FoodData foodData in potentialFood.Data.EdibleBy)
            {
                if (foodData == food.Data)
                {
                    foodTarget = potentialFood;
                }
            }

            // If we are edible by target, register it as enemy
            foreach (FoodData foodData in food.Data.EdibleBy)
            {
                if (foodData == potentialFood.Data)
                {
                    enemyTarget = potentialFood;
                }
            }
        }
    }

    private void LoseEnergy()
    {
        Energy = Energy - energyLossRate * Time.deltaTime;
    }

    private void Eat()
    {
        if (foodTarget == null) return;

        float distanceToFood = Vector3.Distance(foodTarget.transform.position,
            transform.position);

        if (distanceToFood > eatingRange) return;

        Energy = Energy + foodTarget.Data.EnergyGranted;
        foodTarget.BeEaten();
    }

    private void Reproduce()
    {
        if (Energy < reproductionEnergyThreshold) return;

        GameObject child = Instantiate(childPrefab);
        PositionChild(child);
        child.GetComponent<FishController>().Energy = Energy / 2;

        Energy = Energy / 2;
    }

    private void StartHunting()
    {
        dagent.TargetObject = foodTarget.gameObject;
        seekBehavior.enabled = true;
        fleeBehavior.enabled = false;
    }

    private void StartFleeing()
    {
        dagent.TargetObject = enemyTarget.gameObject;
        seekBehavior.enabled = false;
        fleeBehavior.enabled = true;
    }

    private void PositionChild(GameObject child)
    {
        // Give children a random facing direction
        float direction = Random.Range(0f, 360f);
        child.transform.eulerAngles = new Vector3(0f, 0f, direction);
    }

    private void OnDrawGizmos()
    {
        if (foodTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, foodTarget.transform.position);
        }
    }
}
