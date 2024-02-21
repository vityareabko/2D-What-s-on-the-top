
namespace Obstacles
{
    public class FallResource : FallObjectBase
    {
        public ResourceTypes Type;
        public ResourceCategory ResourceCategoryType { get; private set; }

        public void Initialize(float speed, ResourceTypes type, ResourceCategory typeCategory)
        {
            base.Initialize(speed);
            Type = type;
            ResourceCategoryType = typeCategory;
        }
        
    }
}