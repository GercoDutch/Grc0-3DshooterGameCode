using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WeaponHolder : MonoBehaviour
{
    public static UnityEvent<int, int> changeWeaponState;

    [SerializeField] private List<GameObject> weapons;

    public int ChosenWeapon
    {
        get => chosenWeapon;
        private set
        {
            chosenWeapon = value;
            Switch();
        }
    }
    [SerializeField] private int chosenWeapon;

    private void Awake()
    {
        changeWeaponState = new();
        changeWeaponState.AddListener(UpdateWeaponUI);
    }

    private void Start()
    {
        Player.IsShooting.AddListener(Shoot);
        Player.IsReloading.AddListener(Reload);
        Player.SwitchWeapon.AddListener(SwitchWeapon);
        Player.NextWeapon.AddListener(SwitchWeapon);
        Player.GetAmmo.AddListener(GiveAmmo);

        foreach(Transform child in transform)
        {
            weapons.Add(child.gameObject);
            child.gameObject.GetComponent<Weapon>().holder = this;
        }
        
        ChosenWeapon = 0;
    }


    // Switch Weapons
    private void SwitchWeapon(bool nextWeapon)
    {
        ChosenWeapon += nextWeapon ? 1 : -1;

        if(ChosenWeapon >= weapons.Count)
            ChosenWeapon = 0;

        if(ChosenWeapon < 0)
            ChosenWeapon = weapons.Count - 1;
    }

    private void SwitchWeapon(int _weapon)
    {
        if(_weapon < 0)
            _weapon = 0;

        if(_weapon >= weapons.Count)
            _weapon = weapons.Count - 1;

        ChosenWeapon = _weapon;
    }

    private void Switch()
    {
        for (int i = 0; i < weapons.Count; i++)
            weapons[i].SetActive(i == ChosenWeapon);
    }


    // Shoot active weapon
    private void Shoot(bool _value) => weapons[ChosenWeapon].GetComponent<Weapon>().isShooting = _value;


    // Reload active Weapon
    private void Reload() => weapons[ChosenWeapon].GetComponent<Weapon>().Reload();

    private void GiveAmmo() { weapons[ChosenWeapon].GetComponent<Weapon>().GetAmmo(); }


    private void UpdateWeaponUI(int _ammo, int _maxAmmo)
    {
        UserInterface.ammoChanged.Invoke(ChosenWeapon, _ammo, _maxAmmo);
    }
}