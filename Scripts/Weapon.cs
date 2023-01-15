using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform shootingPoint;
    [SerializeField] private SO_Weapon weaponStats;
    public WeaponHolder holder;

    public int RemainingMagazineAmmo
    {
        get => remainingMagazineAmmo;
        private set => remainingMagazineAmmo = value;
    }
    public int RemainingMaxAmmo
    {
        get => remainingMaxAmmo;
        private set => remainingMaxAmmo = value;
    }

    [SerializeField] private int remainingMagazineAmmo;
    [SerializeField] private int remainingMaxAmmo;

    private float timeAtShot;
    private float tbns;
    private bool hasShot;

    public bool isShooting;

    [SerializeField] private Coroutine reloadRoutine;
    public bool IsReloading { get; private set; }

    private void Start()
    {
        RemainingMagazineAmmo = weaponStats.magazineSize;
        RemainingMaxAmmo = weaponStats.maxAmmo - weaponStats.magazineSize;

        tbns = 1f / (float)(weaponStats.fireRate / 60f);
    }

    private void Update()
    {
        if (holder != null)
            WeaponHolder.changeWeaponState.Invoke(remainingMagazineAmmo, remainingMaxAmmo);

        bool canShootAgain = Time.time > timeAtShot + tbns;
        bool magIsNotEmpty = RemainingMagazineAmmo > 0;
        IsReloading = reloadRoutine != null;

        if (isShooting && !IsReloading && canShootAgain && magIsNotEmpty)
        {
            if(weaponStats.isAutomatic)
                Shoot();
            else if(!hasShot)
                Shoot();
        }

        if (!isShooting) hasShot = false;
    }

    // Shoot
    private void Shoot()
    {
        hasShot = true;
        timeAtShot = Time.time;
        RemainingMagazineAmmo--;

        Hit();
    }

    private void Hit()
    {
        RaycastHit hit;
        Physics.Raycast(shootingPoint.position, shootingPoint.forward, out hit, weaponStats.range);
        if (hit.transform)
        {
            Debug.Log("Raakt");
            IDamagable script = hit.transform.gameObject.GetComponent<IDamagable>();
            if(script != null)
            {
                script.TakeDamage(weaponStats.damage);
            }
        } 
    }

    // Reload and Ammo related stuff
    public void Reload()
    {
        if (RemainingMagazineAmmo != weaponStats.magazineSize && RemainingMaxAmmo > 0)
        {
            if (reloadRoutine != null)
                return;

            reloadRoutine = StartCoroutine(ReloadWeapon());
        }
    }

    private IEnumerator ReloadWeapon()
    {
        Debug.Log("Reloading Weapon");
        yield return new WaitForSeconds(weaponStats.reloadTime);

        RemainingMaxAmmo -= weaponStats.magazineSize - RemainingMagazineAmmo;

        int amountUnderZero = 0;
        if (RemainingMaxAmmo < 0)
        {
            amountUnderZero = -RemainingMaxAmmo;
            RemainingMaxAmmo = 0;
        }

        RemainingMagazineAmmo = weaponStats.magazineSize - amountUnderZero;

        reloadRoutine = null;
        Debug.Log("Finished Reloading Weapon");
    }

    public void GetAmmo()
    {
        RemainingMaxAmmo = weaponStats.maxAmmo;
    }

    // For now, stop reloading when switching weapons
    private void OnDisable()
    {
        if (reloadRoutine != null)
        {
            StopCoroutine(reloadRoutine);
            reloadRoutine = null;
        }
    }
}