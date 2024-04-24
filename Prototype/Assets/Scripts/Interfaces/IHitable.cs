namespace Behaviors
{
    public interface IHitable
    {
        void OnDamageReceived(int damage = 1);
    }
}