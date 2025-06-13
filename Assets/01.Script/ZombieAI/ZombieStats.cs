public class ZombieStats
{
    public int maxHealth;
    public int damage;
    public float moveSpeed;
    public float attackDelay;
    public float attackRange;

    public ZombieStats(int health, int dmg, float speed, float delay, float range)
    {
        maxHealth = health;
        damage = dmg;
        moveSpeed = speed;
        attackDelay = delay;
        attackRange = range;
    }
}
