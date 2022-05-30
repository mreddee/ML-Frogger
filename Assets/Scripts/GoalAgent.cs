using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class GoalAgent: Agent
{
    [SerializeField] private Transform [] goalTransform;

    [SerializeField] private Transform [] carTransform;

    [SerializeField] private Transform [] alberoTransform;

    [SerializeField] private Transform [] tartarugaTransform;

    [SerializeField] private Frogger frogger;


    public override void CollectObservations (VectorSensor sensor) {

        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.y);

        foreach (var home in goalTransform){

            sensor.AddObservation(home.localPosition.x);
            sensor.AddObservation(home.localPosition.y);
        }

        foreach (var car in carTransform) sensor.AddObservation(car.localPosition.x);

        foreach (var albero in alberoTransform) sensor.AddObservation(albero.localPosition.x);

        foreach (var tartaruga in tartarugaTransform) sensor.AddObservation(tartaruga.localPosition.x);
        
        //foreach (var prova in GetObservations()) Debug.Log(prova);

    }

    public override void OnActionReceived (ActionBuffers actions){

        if (actions.DiscreteActions[0]==1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            frogger.Move(Vector3.up);
        }
        else if (actions.DiscreteActions[1]==1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180f);
            frogger.Move(Vector3.down);
        }
        else if (actions.DiscreteActions[2]==1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90f);
            frogger.Move(Vector3.left);
        }
        else if (actions.DiscreteActions[3]==1)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90f);
            frogger.Move(Vector3.right);
        }

        /*float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        transform.localPosition += new Vector3(moveX, moveY: Of, movez) * Time.deltaTime * 6;*/
    }

    public override void Heuristic(in ActionBuffers actionsOut){

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        //continuousActions[0] = inputController.Agent.Move.ReadValue<Vector2>().x;
        //continuousActions[1] = inputController.Agent.Move.ReadValue<Vector2>().y;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) discreteActions[0] = 1;

        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) discreteActions[1] = 1;
        
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) discreteActions[2] = 1;

        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) discreteActions[3] = 1;

    }
    
}
