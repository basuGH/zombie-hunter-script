using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentPickUp : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Player.Instance.DocumentPickUp();
                AudioManager.Instance.PlayPickUpSFX();
                ZombieSpawner.Instance.ZombieWave();
                Destroy(gameObject);
            }
        }
    }
}
