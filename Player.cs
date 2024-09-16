using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.Animations.Rigging;
using System.Xml.Serialization;

public class Player : MonoBehaviour
{
    private static Player _instance;
    public static Player Instance { get { return _instance; } }
    [SerializeField] Transform _followCamera;
    [SerializeField] Transform _aimPoint;
    [SerializeField] float _range;
    [SerializeField] StarterAssetsInputs _inputs;
    [SerializeField] bool _holdGun;
    public bool HoldGun { get { return _holdGun; } }
    [SerializeField] bool _isFiring;
    [SerializeField] bool _isReloading;
    [SerializeField] int _weaponLimit;
    public bool IsFiring { get { return _isFiring; } }

    Animator _animator;
    [SerializeField] Transform _gunHolder;

    [SerializeField] GameObject _aimRig, _rightHandAimRig, _rightHand2BoneIKRig, _leftHand2BoneIKRig;
    [SerializeField] GameObject _slot1, _slot2;
    [SerializeField] LayerMask _layerMask;
    public Vector2 Recoil;
    [SerializeField] bool _resetRecoil;
    [SerializeField] int _playerHealth;
    [SerializeField] bool _isAlive;
    [SerializeField] AudioClip _hitAudio, _deathMusic, _introAudio;
    public bool IsTaskComplete { get; private set; }
    
    [SerializeField] int _documentCount;
    public bool IsAlive { get { return _isAlive; } }

    public bool IsMissionComplete { get; private set; }

    // Start is called before the first frame update
    private void Awake()
    {

        _instance = this;
    }
    void Start()
    {
        UiManager.Instance.UpdateHealthUI(_playerHealth);
        _animator = GetComponent<Animator>();
        AudioManager.Instance.PlaySFX(_introAudio, 1);
    }
    private void ProcessRaycast()
    {
        Vector2 screeCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        Ray ray = Camera.main.ScreenPointToRay(screeCenterPoint);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _range, _layerMask.value))
        {
            _aimPoint.position = hit.point;
            // Debug.Log(name + ", I hit : " + hit.collider.name + ", " + hit.point);

        }
        else
        {
            Debug.Log("N0 Ray Cast");
        }
    }
    private void PlayerRotation()
    {
        Vector3 worldLookTarget = _aimPoint.position;
        worldLookTarget.y = transform.position.y;
        Vector3 lookDirection = worldLookTarget - transform.position;

        transform.forward = Vector3.Lerp(transform.forward, lookDirection, Time.deltaTime * 200f);

    }

    void Firing()
    {
        if (_holdGun && Input.GetMouseButton(0) && !_isFiring && !_isReloading)
        {

            _isFiring = true;

            Gun gun = _gunHolder.GetComponentInChildren<Gun>();

            StartCoroutine(ResetFire(gun.FireRate));
            _animator.SetFloat("FireRate", gun.FireRate);
            gun.Shoot();
           // ProcessRecoil(gun.Recoil());
        }
        if (Input.GetMouseButtonUp(0))
        {
            _resetRecoil = true;
            UiManager.Instance.AimImage.SetActive(false);

        }


        _animator.SetBool("Firing", _isFiring);
    }
    public void PlayReloadAnimation()
    {
        _isReloading = true;
        _resetRecoil = true;
        UiManager.Instance.AimImage.SetActive(false);

        _animator.SetTrigger("Reloading");
    }
    public void ReloadAnimationEvent(AnimationEvent animationEvent)
    {
        _isReloading = false;

    }
    public void ManualReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && _holdGun && !_isFiring)
        {
            Gun gun = _gunHolder.GetComponentInChildren<Gun>();
            gun.ReloadMagazine();
        }
    }
    public void ProcessRecoil(Vector2 recoil)
    {
        _resetRecoil = false;
        Recoil.x -= Random.Range(0.2f, recoil.x) * Time.deltaTime;
        Recoil.y += Random.Range(-recoil.y, recoil.y) * Time.deltaTime;
    }
    IEnumerator ResetFire(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Debug.Log(delay);
        _isFiring = false;
    }

    void ResetRecoil()
    {
        if (_resetRecoil)
        {
            Recoil = Vector2.Lerp(Recoil, Vector2.zero, 10 * Time.deltaTime);
        }
    }
    public void FireAnimationEvent()
    {
        // Gun gun = _gunHolder.GetComponentInChildren<Gun>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!_isAlive) { return; }
        ProcessRaycast();
        PlayerRotation();

        if (IsMissionComplete)
        {
            return;
        }
        Firing();
        ManualReload();

        ResetRecoil();

    }
    private void FixedUpdate()
    {
        DropGun();
    }

    private void OnTriggerStay(Collider Other)
    {
        if (!_isAlive) { return; }
        PickUp pickUp = Other.GetComponent<PickUp>();
        if (pickUp != null && !pickUp.IsPickUp)
        {
            // Debug.Log(Other.name + " found!, Press F to Pickup");

            if (Input.GetKey(KeyCode.E))
            {
                if (_gunHolder.childCount > 0)
                {
                    DropGunProcess();
                }

                Debug.Log(Other.name + " item Pickup");

                pickUp.PickUpItem(_gunHolder);
                _holdGun = true;
                _animator.SetBool("HasGun", true);
            }
        }
    }
    private void DropGun()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DropGunProcess();
        }
    }

    private void DropGunProcess()
    {
        PickUp pickUp = _gunHolder.GetComponentInChildren<PickUp>();
        if (pickUp != null && pickUp.IsPickUp)
        {
            Debug.Log(" item drop");
            _isReloading = false;
            pickUp.DropItem();
            _animator.SetBool("HasGun", false);
            _holdGun = false;
            ResetRig();
        }
    }

    private void ResetRig()
    {
        _rightHandAimRig.GetComponent<MultiAimConstraint>().weight = 0;
        _rightHand2BoneIKRig.GetComponent<TwoBoneIKConstraint>().weight = 0;
        _leftHand2BoneIKRig.GetComponent<TwoBoneIKConstraint>().weight = 0;
    }
    private void ResetAllRig()
    {
        _aimRig.GetComponent<Rig>().weight = 0;
        ResetRig();
    }
    public void Damage(int hitPoint)
    {
        if (_isAlive)
        {
            _playerHealth -= hitPoint;
            AudioManager.Instance.PlaySFX(_hitAudio,1);
            if (_playerHealth <= 0)
            {
                _isAlive = false;
                AudioManager.Instance.PlayMusic(_deathMusic, true);
                _animator.SetTrigger("Death");
                ResetAllRig() ;
                DropGunProcess();
                UiManager.Instance.GameOverUI();
            }
            UiManager.Instance.UpdateHealthUI(_playerHealth);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CanPickUp")
        {
            UiManager.Instance.ShowMessage(other.transform);
        }
        else if (other.tag =="ExitPoint" && IsTaskComplete)
        {
            // mission Complete
            UiManager.Instance.MissionCompleteUI();
            IsMissionComplete = true;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CanPickUp")
        {
            UiManager.Instance.HideMessage();
        }
    }
    public void DocumentPickUp()
    {
        _documentCount++;
        UiManager.Instance.TaskUI(_documentCount);
        UiManager.Instance.HideMessage();
        if (_documentCount == 3)
        {
            IsTaskComplete = true;
            UiManager.Instance.AllTaskComplete();

        }
    }

    public AmmoType ActiveGunAmmo()
    {
        return _gunHolder.GetComponentInChildren<Gun>().AmmoType;
    }
}