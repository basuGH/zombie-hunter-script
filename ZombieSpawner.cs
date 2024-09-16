using System.Collections;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    private static ZombieSpawner _instance;
    public static ZombieSpawner Instance {  get { return _instance; } }
    [SerializeField] GameObject[] _zombiePrefab;
    [SerializeField] Transform _target;
    [SerializeField] float _maxRange, _minRange;
    //[SerializeField] float _interval, _delay = 1f;
    [SerializeField] int _maxZombieCount;
    [SerializeField] Vector2Int _intervalRange;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        _target = FindAnyObjectByType<Player>().transform;
        StartCoroutine(PoolZombie());
    }

    public void ZombieWave()
    {
        _maxZombieCount = _maxZombieCount + 2;
        _maxRange = _maxRange + 50;
    }
    
    public void ZombieSpawn(Transform transform)
    {
       // Debug.Log(transform.childCount);

        if (transform.childCount >= _maxZombieCount)
        {
            return;
        }
        // Debug.Log("1");
        float distance = Vector3.Distance(transform.position, _target.position);
        //Debug.Log(" name " + transform.name + ", Distance : " + distance);
        if (distance < _maxRange && distance > _minRange)
        {
                int i = Random.Range(0, _zombiePrefab.Length);
                GameObject zombie = Instantiate(_zombiePrefab[i], transform.position, Quaternion.identity, transform);
        }
    }
    IEnumerator PoolZombie()
    {
        while (Player.Instance.IsAlive)
        {
           // Debug.Log(transform.childCount);
            for (int i = 0; i < transform.childCount; i++)
            {
                //Debug.Log(i);
                ZombieSpawn(transform.GetChild(i));
            }
            int delay = Random.Range(_intervalRange.x, _intervalRange.y);

            yield return new WaitForSeconds(delay);
        }
    }
    //void OnDrawGizmosSelected()
    //{
    //    for (int i = 0;i < transform.childCount;i++)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(transform.GetChild(i).position, _maxRange);
    //        Gizmos.DrawWireSphere(transform.GetChild(i).position, _minRange);
    //    }

    //}
    void Update()
    {

    }
}
