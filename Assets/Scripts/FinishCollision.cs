using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishCollision : MonoBehaviour
{
    public int nextSceneBuildIndex; // The build index of the scene to load after collision

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneBuildIndex);
        }
    }
}
