using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon", menuName = "Weapon", order = 0)]
public class SO_Weapon : ScriptableObject
{
    public int damage;          // Per instance
    public int fireRate;        // Per minute
    public int magazineSize;
    public int maxAmmo;

    public float reloadTime;    // In seconds
    public float range;         // In meters

    public bool isAutomatic;
}
