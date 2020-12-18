using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
    public State state = State.Game;
    
    public float length = 10f;
    public float speed = 5f;

    public float lifetime = 10;
    
    private LineRenderer _lr;
    private Transform _tf;
    private PlayerStats _ps;
    private Vector3 _origin, _end;
    
    private void Start()
    {
        switch (state)
        {
            case State.Game:
                _lr = GetComponent<LineRenderer>();
                _ps = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        
                _tf = transform;
                break;
            case State.Menu:
                _lr = GetComponent<LineRenderer>();

                _tf = transform;
                break;
        }
    }


    private void Update()
    {
        switch (state)
        {
            case State.Game:
                Destroy(gameObject, lifetime);
                Move();
                UpdateSprite();
                Shoot();
                break;
            case State.Menu:
                UpdateSprite();
                break;
        }
    }

    private void Shoot()
    {
        if (!Physics.Raycast(_origin, _tf.right, out var hit, length)) return;
        if (hit.transform.CompareTag("Player"))
        {
            _ps.Damage(3);
        }
        if (hit.transform.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        _tf.position += _tf.forward * (speed * Time.deltaTime);
    }

    private void UpdateSprite()
    {
        var pos = _tf.position;
        var rot = _tf.right;

        _origin = pos - rot * (length * 0.5f);
        _end = pos + rot  * (length * 0.5f);
        
        _lr.SetPosition(0, _origin);
        _lr.SetPosition(1, _end);
    }
    
    public enum State
    {
        Game,
        Menu
    }
}
