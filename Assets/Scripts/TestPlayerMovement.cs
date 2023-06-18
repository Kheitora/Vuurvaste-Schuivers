using UnityEngine;

public class TestPlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * movementSpeed * Time.deltaTime;

        transform.Translate(movement);
    }
}
