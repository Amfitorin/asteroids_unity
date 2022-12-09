namespace CoreMechanics.Managers
{
    public interface IManagerFabric<out T> where T : IManager
    {
        T GetManager();
    }
}