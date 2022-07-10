using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Frogger : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Animator _animator;
    [SerializeField] private FroggerAgent _agent;

    private Vector3 _movePoint;
    private Transform _parent;

    private float _farthestRow;

    private void Awake()
    {
        _movePoint = transform.localPosition;
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_parent != null)
        {
            _movePoint = _parent.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, _movePoint, _moveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (_movePoint != null)
        {
            Gizmos.DrawSphere(_movePoint, 0.5f);
        }
    }

    public void Move(Vector3 direction)
    {

        // Only round transform.position.y so Frogger is always on a rounded Y-axis.
        // transform.position.x is okay with float values since the Home-Zones aren't placed in a rounded X-axis.
        Vector3 destination = new Vector3(transform.position.x, Mathf.Round(transform.position.y), transform.position.z) + direction;
        Collider2D barrier = Physics2D.OverlapBox(destination, Vector2.zero, 0, LayerMask.GetMask("Barrier"));
        Collider2D platform = Physics2D.OverlapBox(destination, Vector2.zero, 0, LayerMask.GetMask("Platform"));
        Collider2D obstacle = Physics2D.OverlapBox(destination, Vector2.zero, 0, LayerMask.GetMask("Obstacle"));
        Collider2D water = Physics2D.OverlapBox(destination, Vector2.zero, 0, LayerMask.GetMask("Water"));

        if (destination.x > -7.5f && destination.x < 7.5f){

            if (barrier != null) // Prevents movement
            {
                return;
            }

            if (platform != null) // Parents movement 
            {
                _parent = platform.transform;
                transform.SetParent(_parent);
            }
            else
            {
                _parent = null;
                transform.SetParent(null);
            }

            if (obstacle != null || (water != null && platform == null)) // Allows movement, but kills
            {
                transform.position = destination;
                Death();
                
            }
            else
            {
                
                if (Vector3.Distance(transform.position, _movePoint) <= 0.05f)
                {
                    

                        _animator.SetTrigger("Leap");
                        _movePoint = destination;

                        if (destination.y > _farthestRow)
                        {
                            _farthestRow = destination.y;
                            _agent.AdvancedRow();
                        }
                    
                }
                
            }
        }
    }

    public void Death()
    {
        transform.rotation = Quaternion.identity;
        _animator.SetBool("Dead", true);
        _parent = null;
        enabled = false;

        _agent.Died();
    }

    public void Respawn()
    {
        _parent = null;
        transform.rotation = Quaternion.identity;
        transform.position = new Vector2(0f,-6f);
        _movePoint = transform.position;
        _farthestRow = transform.position.y;
        _animator.SetBool("Dead", false);
        enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enabled && other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Death();
        }
    }

}
