using CoreMechanics.ObjectLinks.UnityObjectLink;

namespace UIModel.Window
{
    public class WindowData
    {
        public GameObjectLink Prefab { get; private set; }

        public void SetupPrefab(GameObjectLink obj)
        {
            Prefab = obj;
        }
    }
}