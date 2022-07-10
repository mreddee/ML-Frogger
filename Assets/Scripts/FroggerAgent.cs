using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class FroggerAgent: Agent
{

    private int _score = 0;
    private int _deaths = 0;
    private int _levels = 0;
    private int _homes_occupied = 0;
    private int _row = 0;
    private IEnumerator timerCoroutine;

    private int _time;

    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _deathsText;
    [SerializeField] private Text _levelsText;
    [SerializeField] private Text _homesOccupiedText;
    [SerializeField] private Text _timeText;

    [SerializeField] private Home [] homes;

    [SerializeField] private Transform [] carTransform;

    [SerializeField] private Transform [] alberoTransform;

    [SerializeField] private Transform [] tartarugaTransform;

    [SerializeField] private Frogger frogger;

    private Vector3 [] initCarTransform = new Vector3[15];
    private Vector3 [] initLogTransform = new Vector3[11];
    private Vector3 [] initTurtleTransform = new Vector3[8];
    
    private void Start()
    {
        SetScore(0);
        SetDeaths(0);
        SetLevels(0);

        for (int i=0; i<carTransform.Length; i++) initCarTransform[i] = carTransform[i].position;
        for (int i=0; i<alberoTransform.Length; i++) initLogTransform[i] = alberoTransform[i].position;
        for (int i=0; i<tartarugaTransform.Length; i++) initTurtleTransform[i] = tartarugaTransform[i].position;
    }

    private IEnumerator Timer(int duration)
    {
        _time = duration;

        _timeText.text = _time.ToString();

        while (_time > 0)
        {
            yield return new WaitForSeconds(1);

            _time--;
            _timeText.text = _time.ToString();
        }

        frogger.Death();
    }

    private void SetScore(int score)
    {
        this._score = score;
        _scoreText.text = score.ToString();
    }

    private void SetDeaths(int deaths)
    {
        this._deaths = deaths;
        _deathsText.text = deaths.ToString();
    }

    private void SetLevels(int levels)
    {
        this._levels = levels;
        _levelsText.text = levels.ToString();
    }

    private void SetHomeOccupied(int homes_occupied)
    {
        this._homes_occupied = homes_occupied;
        _homesOccupiedText.text = _homes_occupied.ToString();
    }

    public void AdvancedRow()
    {
        SetScore(_score + 10);
        if (_row>5) AddReward(0.5f);
        else AddReward(0.05f);
        _row +=1;
    }

    public override void OnEpisodeBegin(){

        SetScore(0);
        DisableHomes();
        Respawn();
    }


    public override void CollectObservations (VectorSensor sensor) {

        // Aggiunge la posizione di frogger alle osservazioni

        sensor.AddObservation(new Vector2(transform.position.x, transform.position.y));

        // Aggiunge la posizione e lo stato (occupata/non occupata) delle 5 case alle osservazioni

        foreach (var home in homes) sensor.AddObservation(new Vector3(home.transform.position.x, home.transform.position.y, Convert.ToSingle(home.enabled)));

        // Aggiunge la posizione delle macchine alle osservazioni (solo ascissa)

        foreach (var car in carTransform) sensor.AddObservation(new Vector2(car.position.x,car.position.y));

        // Aggiunge la posizione dei tronchi alle osservazioni (solo ascissa)

        foreach (var log in alberoTransform) sensor.AddObservation(new Vector2(log.position.x,log.position.y));

        // Aggiunge la posizione delle tartarughe alle osservazioni (solo ascissa)

        foreach (var turtle in tartarugaTransform) sensor.AddObservation(new Vector2(turtle.position.x,turtle.position.y));

    }

    public override void OnActionReceived (ActionBuffers actions){

        if (actions.DiscreteActions[0]==1){

            transform.rotation = Quaternion.Euler(0, 0, 0);
            frogger.Move(Vector3.up);
        }
        else if (actions.DiscreteActions[0]==2){

            transform.rotation = Quaternion.Euler(0, 0, 180f);
            frogger.Move(Vector3.down);
        }
        else if (actions.DiscreteActions[1]==1){

            transform.rotation = Quaternion.Euler(0, 0, 90f);
            frogger.Move(Vector3.left);
        }
        else if (actions.DiscreteActions[1]==2){

            transform.rotation = Quaternion.Euler(0, 0, -90f);
            frogger.Move(Vector3.right);
        }

    }

    public override void Heuristic(in ActionBuffers actionsOut){

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) discreteActions[0] = 1;

        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) discreteActions[0] = 2;
        
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) discreteActions[1] = 1;

        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) discreteActions[1] = 2;

    }

    public void HomeOccupied(Home home)
    {
        if (home.enabled) Died();

        else{

            int bonusPoints = _time * 20;
            SetScore(_score + bonusPoints + 50);
            SetHomeOccupied(_homes_occupied + 1);

            AddReward(2f);

            home.enabled = true;

            if (IsLevelCleared()){

                SetLevels(_levels + 1);
                EndEpisode();
            }
            else Respawn();
        }

    }

    private void Respawn(){

        StopAllCoroutines();
        StartCoroutine(Timer(30));

        _row = 0;

        for (int i=0; i<carTransform.Length; i++) carTransform[i].position = initCarTransform[i];
        for (int i=0; i<alberoTransform.Length; i++) alberoTransform[i].position = initLogTransform[i];
        for (int i=0; i<tartarugaTransform.Length; i++) tartarugaTransform[i].position = initTurtleTransform[i];

        frogger.Respawn();
    }

    public void Died()
    {
        SetDeaths(_deaths + 1);
        EndEpisode();
    }

    private bool IsLevelCleared()
    {
        for (int i = 0; i < homes.Length; i++) if (!homes[i].enabled) return false;

        return true;
    }

    private void DisableHomes()
    {
        for (int i = 0; i < homes.Length; i++) homes[i].enabled = false;
    }
    
}
