public interface IDamageDealer
{
    int DamageAmount { get; }
    void DealDamage(HealthSystem target);
}