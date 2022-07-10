using UnityEngine;

public class Home : MonoBehaviour
{
    public GameObject OccupiedFrog;

    public bool _enabled = false;

    private void OnEnable(){

        OccupiedFrog.SetActive(true);
    }

    private void OnDisable(){

        OccupiedFrog.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other){

        FroggerAgent _agent = other.GetComponent<FroggerAgent>();

        if (other.tag == "Player") _agent.HomeOccupied(this);
    }

}
