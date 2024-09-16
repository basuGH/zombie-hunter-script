using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    public void PlayEffect(Vector3 pos)
    {
        ParticleSystem effect = Instantiate(_particleSystem, pos, Quaternion.identity);
        Destroy(effect,10f);
    }

}
