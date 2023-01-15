using UnityEngine;
using UnityEngine.Events;

public class Player : Person
{
    public static UnityEvent<bool> IsShooting;
    public static UnityEvent IsReloading;
    public static UnityEvent<int> SwitchWeapon;
    public static UnityEvent<bool> NextWeapon;
    public static UnityEvent GetAmmo;

    [Header("References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private WeaponHolder weapon;
    [SerializeField] private UserInterface ui;

    [Header("Check Ground")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance;
    [SerializeField] private bool isGrounded;

    [Header("Keycodes")]
    [SerializeField] private KeyCode shoot;
    [SerializeField] private KeyCode reload;
    [SerializeField] private KeyCode weapon1;
    [SerializeField] private KeyCode weapon2;
    [SerializeField] private KeyCode weapon3;
    [SerializeField] private KeyCode nextWeapon;
    [SerializeField] private KeyCode previousWeapon;

    public GameObject pauseMenu;
    

    private float _x, _z;

    private float gravity = -9.81f;
    private float verticalVelocity;

    private void Awake()
    {
        IsShooting = new();
        IsReloading = new();
        SwitchWeapon = new();
        NextWeapon = new();
        GetAmmo = new();
    }

    private void Update()
    {
        _x = Input.GetAxisRaw("Horizontal");
        _z = Input.GetAxisRaw("Vertical");

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, ~groundMask);
        Fall();

        Vector3 inputDirection = transform.forward * _z + transform.right * _x;
        Move(inputDirection.normalized);

        CheckInput();
    }


    // Movement
    protected override void Move(Vector3 _direction) 
    {
        controller.Move(_direction * moveSpeed * Time.deltaTime);
    }

    private void Fall()
    {
        if(isGrounded)
            verticalVelocity = -0.02f;
        else
            verticalVelocity += gravity * Time.deltaTime * Time.deltaTime;

        controller.Move( new Vector3(0, verticalVelocity, 0) );
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(shoot)) IsShooting.Invoke(true);
        if (Input.GetKeyUp(shoot)) IsShooting.Invoke(false);

        if (Input.GetKeyDown(reload)) IsReloading.Invoke();

        if (Input.GetKeyDown(weapon1)) SwitchWeapon.Invoke(0);
        if (Input.GetKeyDown(weapon2)) SwitchWeapon.Invoke(1);
        if (Input.GetKeyDown(weapon3)) SwitchWeapon.Invoke(2);

        if (Input.GetKeyDown(nextWeapon)) NextWeapon.Invoke(true);
        if (Input.GetKeyDown(previousWeapon)) NextWeapon.Invoke(false);
        if (Input.mouseScrollDelta.y > 0) NextWeapon.Invoke(true);
        if (Input.mouseScrollDelta.y < 0) NextWeapon.Invoke(false);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!ui.IsPauseMenuOpened)
                ui.IsPauseMenuOpened = true;
            else
                ui.IsPauseMenuOpened = false;
        }
        
    }

    // Damagable
    protected override void Die()
    {
        ui.OpenDieMenu();
    }
}