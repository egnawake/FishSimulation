using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using LibGameAI.FSMs;

public class BigFishes : MonoBehaviour
{

    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private GameObject food;

    private StateMachine fsm;

    private void Start()
    {

        State wanderState = new State("Wander", () => Debug.Log("Enter wander state"), Wander, () => Debug.Log("Exit on wander state"));

        State huntingState = new State("Hunting", () => Debug.Log("Enter hunting state"), LookForFood, () => Debug.Log("Exit on hunting state"));

        Transition Wander2Hunting = new Transition(
            () => (transform.position - food.transform.position).magnitude < detectionRange,
            () => Debug.Log("See food"),
            huntingState
            );

        Transition Hunting2Wander = new Transition(
            () => (transform.position - food.transform.position).magnitude > detectionRange,
            () => Debug.Log("Nothing To eat"),
            wanderState
            );

        huntingState.AddTransition(Hunting2Wander);
        wanderState.AddTransition(Wander2Hunting);

    }

    private void Update()
    {
        Action actionToDo = fsm.Update();
        actionToDo?.Invoke();
    }

    private void Wander()
    {

    }

    private void LookForFood()
    {

    }
}
