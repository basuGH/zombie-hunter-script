using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    [SerializeField] int _hitPoint;
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            if (Player.Instance.IsMissionComplete)
            {
                return;
            }
            player.Damage(_hitPoint);
            Debug.Log("Zombie Attack !!");
        }
    }
}
