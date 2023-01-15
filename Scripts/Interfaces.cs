internal interface IDamagable
{
    int Health { get; }

    void TakeDamage(int value);
}

internal interface ICollectible
{
    void Collect(Player player);
}