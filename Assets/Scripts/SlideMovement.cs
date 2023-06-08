using UnityEngine;

public class SlideMovement : MonoBehaviour
{
    private bool isSelected = false;
    private Vector3 offset;
    private float distanceToCamera;


    private void OnMouseUp()
    {
        isSelected = false;
    }

    private void OnMouseDown()
    {
        isSelected = true;
        distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
        offset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        if (!isSelected)
            return;

        float dragSpeed = 1f / transform.localScale.magnitude; // Calculate the drag speed based on object size
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 newPosition = Vector3.Lerp(transform.position, mousePosition, dragSpeed * Time.deltaTime);
        newPosition.y = transform.position.y; // Lock the object's position on the y-axis
        transform.position = newPosition;

    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = distanceToCamera;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
