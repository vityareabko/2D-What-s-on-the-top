using UnityEngine;

public class TriggerPlayerTransionToOtherLevel : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D colider)
    {
        if (colider.CompareTag(ConstTags.Player))
            GetComponent<BoxCollider2D>().isTrigger = false;
    }
}
