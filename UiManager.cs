using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    private static UiManager _instance;
    public static UiManager Instance { get { return _instance; } }
    [field : SerializeField]public GameObject AimImage {  get; set; }
    [field : SerializeField]public GameObject GunStatus {  get; set; }
    [SerializeField] Image _healthBar;

    [SerializeField] private TextMeshProUGUI _messageText, _taskText;
    [SerializeField] private TextMeshProUGUI _ammoInSlotsText, _ammoInMagazineText;
    [SerializeField] private GameObject _fullMap;
    [SerializeField] private GameObject _missionCompleteUI;
    [SerializeField] private GameObject _gameOverUI;
    private void Awake()
    {
        _instance = this;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }
    private void Start()
    {
        _missionCompleteUI.SetActive(false);
        _gameOverUI.SetActive(false);
        _messageText.gameObject.SetActive(false);
    }
    public void ShowMessage(Transform itemTransform)
    {
        _messageText.gameObject.SetActive(true);
        string name = itemTransform.name;
        _messageText.text = name.Replace("(Clone)", "").Trim() + " found, Press 'E' to pick up !";
    }
    public void HideMessage()
    {
        _messageText.gameObject.SetActive(true);
        _messageText.text = string.Empty;
    }
    public void UpdateAmmoInSlotText(AmmoType ammoType)
    {
        _ammoInSlotsText.text = Ammo.Instance.GetCurrentAmmo(ammoType).ToString() + " / ";
    }
 
    public void UpdateMagazineAmmoCountText(int ammoCount)
    {
        _ammoInMagazineText.text = ammoCount.ToString();
    }
    public void UpdateHealthUI(int health)
    {
        _healthBar.fillAmount = (float)health / 100f;
    }
    private void Update()
    {
        ShowFullMap();

    }

    private void ShowFullMap()
    {
        if (!Player.Instance.IsAlive)
        {
            return;
        }
        if (Input.GetKey(KeyCode.M))
        {
            _fullMap.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.M))
        {
            _fullMap.SetActive(false);
        }
    }
    public void TaskUI(int task)
    {
        _taskText.text = task.ToString() +"/3 Task Complete";
    }
    public void AllTaskComplete()
    {
        _taskText.text += "\n"+"Objective complete \nFind the exit and run";
    }
    public void GameOverUI()
    {
        _gameOverUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void MissionCompleteUI()
    {
        _missionCompleteUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void RestartBtnClick()
    {
        int i = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(i);
    }
    public void MainMenuBtnClick()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitBtnClick()
    {
        Application.Quit();
    }
    
}
