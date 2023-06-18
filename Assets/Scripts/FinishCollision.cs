using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishCollision : MonoBehaviour
{
    public int nextSceneBuildIndex; 

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneBuildIndex);
        }
    }
}
