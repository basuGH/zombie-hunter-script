using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] AmmoType _ammoType;
    [SerializeField] int _ammoAmount;
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.Impulse);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log(other.name + " Pick Up " + _ammoType + " amount " + _ammoAmount);


                other.GetComponent<Ammo>().IncreaseAmmo(_ammoType, _ammoAmount);
                UiManager.Instance.HideMessage();
                if (Player.Instance.HoldGun && Player.Instance.ActiveGunAmmo() == _ammoType)
                {
                    UiManager.Instance.UpdateAmmoInSlotText(_ammoType);
                }
                AudioManager.Instance.PlayPickUpSFX();

                Destroy(gameObject);
            }
        }
    }
}
