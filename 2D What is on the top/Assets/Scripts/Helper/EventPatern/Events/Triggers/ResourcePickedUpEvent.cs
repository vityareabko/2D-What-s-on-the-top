
using ResourcesCollector;

public class ResourcePickedUpEvent
{
    public IPickUp Resource { get; }

    public ResourcePickedUpEvent(IPickUp resource)
    {
        Resource = resource;
    }
}
