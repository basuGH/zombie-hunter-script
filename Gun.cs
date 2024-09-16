using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] ParticleSystem _muzzleFlash;
    [SerializeField] AudioClip _gunshotAudio, _noAmmoAudio, _magazineReloadAudio;
    [SerializeField] float _fireRate;
    [SerializeField] float _range;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] AmmoType _ammoType;
    public AmmoType AmmoType { get { return _ammoType; } }

    [SerializeField] int _magazineCapacity;
    [SerializeField] int _hitPoint;
    [field: SerializeField] public int MagazineAmmoCount { get; set; }

    public float FireRate { get { return 1/_fireRate; } }

    [SerializeField] float _maxRecoilX, _maxRecoilY;
    //[SerializeField] int _ammoCapacity;
    private void Start()
    {
        MagazineAmmoCount = _magazineCapacity;
    }
    public void Shoot()
    {
        if (MagazineAmmoCount > 0)
        {
            _muzzleFlash.Play();
            UiManager.Instance.AimImage.SetActive(true);
            Player.Instance.ProcessRecoil(Recoil());
            AudioManager.Instance.PlaySFX(_gunshotAudio, .4f);
            ProcessRaycast();
            MagazineAmmoCount--;
            DisplayUI();
            //Debug.Log("Fire");
        }
        else
        {
            ReloadMagazine();

        }
    }
    public void DisplayUI()
    {
        UiManager.Instance.UpdateAmmoInSlotText(_ammoType);
        UiManager.Instance.UpdateMagazineAmmoCountText(MagazineAmmoCount);
    }
    public void ReloadMagazine()
    {
        if (Ammo.Instance.GetCurrentAmmo(_ammoType) > 0)
        {
            if (MagazineAmmoCount == _magazineCapacity)
            {
                return;
            }
            AudioManager.Instance.PlaySFX(_magazineReloadAudio, 1);
            Player.Instance.PlayReloadAnimation();

            int ammoToFullCapacity = _magazineCapacity - MagazineAmmoCount;

            if (Ammo.Instance.GetCurrentAmmo(_ammoType) >= ammoToFullCapacity )
            {
                MagazineAmmoCount += ammoToFullCapacity;
                Ammo.Instance.ReduceAmmo(_ammoType, ammoToFullCapacity);
            }
            else
            {
                ammoToFullCapacity = Ammo.Instance.GetCurrentAmmo(_ammoType);
                MagazineAmmoCount += ammoToFullCapacity;
                Ammo.Instance.ReduceAmmo(_ammoType, ammoToFullCapacity);
            }
            DisplayUI();
        }
        else
        {
            if (Player.Instance.IsFiring)
            {
                AudioManager.Instance.PlaySFX(_noAmmoAudio, 1);
            }
        }

    }


    public Vector2 Recoil()
    {
        return new Vector2(_maxRecoilX, _maxRecoilY);
    }
    private void ProcessRaycast()
    {
        Vector3 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, _range))
        {
            if (hitInfo.transform.TryGetComponent<EnemyAI>(out EnemyAI enemyAI))
            {
                enemyAI.Damage(_hitPoint);
                Debug.Log("Gun Fire " + hitInfo.transform.name);

            }
            if (hitInfo.transform.TryGetComponent<BulletEffect>(out BulletEffect bullet))
            {
                bullet.PlayEffect(hitInfo.point);
                Debug.Log("VFX " + hitInfo.transform.name);
            }
        }

    }

}
