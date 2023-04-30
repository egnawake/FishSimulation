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

    private DynamicAgent dagent;
    private StateMachine fsm;

    private float energy;
    private GameObject enemy;
    private Algae food;

    private void Start()
    {
        dagent = GetComponent<DynamicAgent>();

        State wanderState = new State("Wander", () => Debug.Log("Enter wander state"), Wander, () => Debug.Log("Exit on wander state"));

        State huntingState = new State("Hunting", () => Debug.Log("Enter hunting state"), LookForFood, () => Debug.Log("Exit on hunting state"));

        State runState = new State("Run", () => Debug.Log("Enter run state"), Run, () => Debug.Log("Exit on run state"));

        Transition Wander2Hunting = new Transition(
            () => (transform.position - food.transform.position).magnitude < detectionRange,
            () => Debug.Log("See food"),
            huntingState
            );

        Transition Run2Hunting = new Transition(
            () => (transform.position - food.transform.position).magnitude < detectionRange
            && (transform.position - enemy.transform.position).magnitude < detectionRange,
            () => Debug.Log("See food"),
            huntingState
            );

        wanderState.AddTransition(Wander2Hunting);
        runState.AddTransition(Run2Hunting);

        Transition Hunting2Wander = new Transition(
            () => (transform.position - food.transform.position).magnitude > detectionRange,
            () => Debug.Log("Nothing To eat"),
            wanderState
            );

        Transition Run2Wander = new Transition(
            () => (transform.position - food.transform.position).magnitude > detectionRange
            && (transform.position - enemy.transform.position).magnitude > detectionRange,
            () => Debug.Log("Am Safe"),
            wanderState
            );

        runState.AddTransition(Run2Wander);
        huntingState.AddTransition(Hunting2Wander);

        Transition Hunting2Run = new Transition(
            () => (transform.position - enemy.transform.position).magnitude < detectionRange,
            () => Debug.Log("Can't eat gotta run"),
            runState
            );

        Transition Wander2Run = new Transition(
            () => (transform.position - food.transform.position).magnitude < detectionRange,
            () => Debug.Log("Oh no i'm afraid"),
            runState
            );

        wanderState.AddTransition(Wander2Run);
        huntingState.AddTransition(Hunting2Run);

        fsm = new StateMachine(wanderState);
    }

    void Update()
    {
        //Action actionToDo = fsm.Update();
        //actionToDo?.Invoke();

        UpdateEntitiesInRange();
        Eat();
    }

    private void Run()
    {

    }

    private void Wander()
    {

    }

    private void LookForFood()
    {

    }

    private void UpdateEntitiesInRange()
    {
        // Find colliders of entities in range
        Collider2D[] entitiesInRange = Physics2D.OverlapCircleAll(transform.position,
            detectionRange);

        // Sort entities by distance
        IEnumerable<GameObject> sortedEntities = entitiesInRange
            .Select(collider => collider.gameObject)
            .OrderBy(gameObject => (transform.position - gameObject.transform.position).magnitude);

        // Get closest algae object
        GameObject algaeObject = sortedEntities.FirstOrDefault(
            gameObject => gameObject.GetComponent<Algae>() != null);

        dagent.TargetObject = algaeObject;
        food = algaeObject?.GetComponent<Algae>();
    }

    private void Eat()
    {
        if (food == null) return;

        float distanceToFood = Vector3.Distance(food.transform.position,
            transform.position);

        if (distanceToFood > eatingRange) return;

        energy = energy + food.BeEaten();
    }
}
