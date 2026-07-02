using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEntrance : MonoBehaviour
{
    [Header("Scene")]
    public string sceneName;

    [Header("Interaction")]
    public float enterRange = 1.5f;

    public bool TryUse(GameObject player)
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);

        if (distance > enterRange)
            return false;

        SceneManager.LoadScene(sceneName);

        return true;
    }
}