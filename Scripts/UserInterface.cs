using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class UserInterface : MonoBehaviour
{
    public static UnityEvent<int> healthChanged;
    public static UnityEvent<int, int, int> ammoChanged;
    public static UnityEvent<int> enemyCounterChanged;

    [SerializeField] private GameObject
        hud,
        pauseMenu,
        dieMenu
    ;

    private bool isPauseMenuOpened;
    public bool IsPauseMenuOpened
    {
        get => isPauseMenuOpened;
        set
        {
            isPauseMenuOpened = value;
            OpenClosePauseMenu(isPauseMenuOpened);
        }
    }

    [SerializeField] private Player player;

    [SerializeField] private Text
        pistolAmmo,
        pistolMaxAmmo,
        laserAmmo,
        laserMaxAmmo,
        rifleAmmo,
        rifleMaxAmmo,
        enemyCounter
    ;

    public Slider healthSlider;

    private void Awake()
    {
        healthChanged = new();
        healthChanged.AddListener(ChangeHealthValue);

        ammoChanged = new();
        ammoChanged.AddListener(ChangeAmmoValue);

        enemyCounterChanged = new();
        enemyCounterChanged.AddListener(ChangeEnemyCounterValue);

        healthSlider.maxValue = player.MaxHealth;
        healthSlider.value = player.Health;
        
        Enemy.AmountOfEnemies = 0;
    }

    // Health and Ammo values
    private void ChangeHealthValue(int _value) => healthSlider.value = _value;

    private void ChangeAmmoValue(int whatWeapon, int _ammoAmount, int _maxAmmoAmount)
    {
        switch(whatWeapon)
        {
            case 0:
                pistolAmmo.text = _ammoAmount.ToString();
                pistolMaxAmmo.text = _maxAmmoAmount.ToString();

                break;
            case 1:
                laserAmmo.text = _ammoAmount.ToString();
                laserMaxAmmo.text = _maxAmmoAmount.ToString();

                break;
            case 2:
                rifleAmmo.text = _ammoAmount.ToString();
                rifleMaxAmmo.text = _maxAmmoAmount.ToString();

                break;
            default:
                Debug.LogError("Choose a number between 0 and 2!");
                break;
        }
    }

    private void ChangeEnemyCounterValue(int _value) => enemyCounter.text = $"{_value}";

    // Menu Panels
    public void OpenClosePauseMenu(bool _isPaused)
    {
        hud.SetActive(!_isPaused);
        pauseMenu.SetActive(_isPaused);

        Cursor.lockState = _isPaused ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = _isPaused;
    }

    public void OpenDieMenu()
    {
        hud.SetActive(false);
        pauseMenu.SetActive(false);
        dieMenu.SetActive(true);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}