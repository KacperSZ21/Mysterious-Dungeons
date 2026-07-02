using UnityEngine;

public class CaveEntrance : MonoBehaviour
{
    [Header("Teleport Target")]
    public Transform destination;

    [Header("Interaction")]
    public float enterRange = 1.5f;

    public void EnterCave(GameObject player)
    {
        if (destination == null)
            return;

        float distance = Vector2.Distance(player.transform.position, transform.position);

        if (distance > enterRange)
            return;

        player.transform.position = destination.position;
        Camera.main.transform.position = new Vector3(destination.position.x, destination.position.y, Camera.main.transform.position.z);
    }
}