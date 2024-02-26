namespace Systems.ResourcesLoaderSystem
{
    public interface IResourceLoaderSystem
    {
        public T Load<T>(ResourceID resourceID) where T : UnityEngine.Object;
    }
}