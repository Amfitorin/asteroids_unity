using JetBrains.Annotations;

namespace CoreMechanics.ObjectLinks
{
    public interface IObjectLink
    {
        [CanBeNull]
        object Resource { get; }
#if UNITY_EDITOR
        string Path { get; }
#endif
        void Initialize(string path);
    }

    public interface IObjectLink<T> : IObjectLink
    {
    }
}