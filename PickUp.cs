using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private static PickUp _instance;
    public static PickUp Instance {  get { return _instance; } }
    public bool IsPickUp;

    Rigidbody _rb;
    Collider _collider;
    Player _player;

    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _player = FindObjectOfType<Player>();

    }

    public void PickUpItem( Transform parent)
    {
        IsPickUp = true;
        Debug.Log("Gun Picked up");
        transform.parent = parent;
 
        _rb.isKinematic = true;
        _rb.useGravity = false;
        _collider.enabled = false;
        UiManager.Instance.HideMessage();
        UiManager.Instance.GunStatus.SetActive(true);
        GetComponent<Gun>().DisplayUI();
        AudioManager.Instance.PlayPickUpSFX();
        // Reset the gun's local position and rotation to align it with the gun holder
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    public void DropItem()
    {
        UiManager.Instance.GunStatus.SetActive(false);
       // IsPickUp = false;
        StartCoroutine(ResetIsPickUP());
        transform.parent = null;

        _collider.enabled = true;
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.AddForce(_player.transform.forward * 4, ForceMode.Impulse);
        _rb.AddTorque(_player.transform.right * 1, ForceMode.Impulse);

    }

    IEnumerator ResetIsPickUP()
    {
        yield return new WaitForSeconds(1f);
        IsPickUp = false;
    }

}
