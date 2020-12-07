using System.Collections;
using UnityEngine;

public class LaserGenerator : MonoBehaviour
{
    public float resetTimer = 10f;
    
    public Material on, off;

    private Renderer _r;
    private EnemySpawningManager _esm;

    [HideInInspector] public bool isOn;

    private void Start()
    {
        var gameManager = GameObject.Find("GameManager");
        
        _esm = gameManager.GetComponent<EnemySpawningManager>();
        _r = GetComponent<Renderer>();
        isOn = true;

        _r.material = on;
    }

    private void Update()
    {
        if (_esm.currGeneratorsOn > 0) return;
        StopCoroutine(Reset());
        Destroy(gameObject);
    }

    public IEnumerator Reset()
    {
        isOn = false;
        _r.material = off;

        yield return new WaitForSeconds(resetTimer);

        isOn = true;
        _r.material = on;
    }
}
