using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LibGameAI.FSMs;
using AIUnityExamples.Movement.Dynamic;

public class SMFishes : MonoBehaviour
{

    [SerializeField] private float detectionRange = 1f;
    [SerializeField] private float eatingRange = 0.5f;
    [SerializeField] private SteeringBehaviour seekBehavior;
    [SerializeField] private SteeringBehaviour fleeBehavior;

    private DynamicAgent dagent;
    private StateMachine fsm;
    private FishInfoPanel infoPanel;
    private Food food;

    private float energy;
    private float Energy
    {
        get => energy;
        set
        {
            energy = value;
            infoPanel.Energy = value;
        }
    }

    private Food foodTarget;
    private Food enemyTarget;

    private void Start()
    {
        dagent = GetComponent<DynamicAgent>();
        food = GetComponent<Food>();
        infoPanel = GetComponentInChildren<FishInfoPanel>();

        // States
        State wanderState = new State("Wander",
            null,
            Wander,
            null);

        State huntingState = new State("Hunting",
            null,
            Eat,
            null);

        State runState = new State("Run",
            null,
            Run,
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

        UpdateEntitiesInRange();
    }

    private void Run()
    {

    }

    private void Wander()
    {

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

    private void Eat()
    {
        if (foodTarget == null) return;

        float distanceToFood = Vector3.Distance(foodTarget.transform.position,
            transform.position);

        if (distanceToFood > eatingRange) return;

        Energy = Energy + foodTarget.Data.EnergyGranted;
        foodTarget.BeEaten();
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

    private void OnDrawGizmos()
    {
        if (foodTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, foodTarget.transform.position);
        }
    }
}
