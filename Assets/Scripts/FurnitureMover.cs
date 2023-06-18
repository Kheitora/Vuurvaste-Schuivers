using UnityEngine;

public class FurnitureMover : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private int gridSize = 1;
    private LayerMask furnitureLayer;
    private LayerMask floorLayer;
    private Collider furnitureCollider;
    private float originalY;

    private void Start()
    {
        mainCamera = Camera.main;
        furnitureLayer = LayerMask.GetMask("Furniture");
        floorLayer = LayerMask.GetMask("Floor");
        furnitureCollider = GetComponent<Collider>();
        originalY = 1;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (IsTouchingFurniture(touch.position))
                    {
                        isDragging = true;
                        offset = gameObject.transform.position - GetTouchWorldPosition(touch.position);
                    }
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        Vector3 newPosition = GetTouchWorldPosition(touch.position);
                        newPosition.y = originalY; // Maintain the original Y position
                        newPosition = SnapToGrid(newPosition);

                        if (CanMoveToPosition(newPosition))
                        {
                            transform.position = newPosition + offset;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    isDragging = false;
                    break;
            }
        }
    }

    private bool IsTouchingFurniture(Vector2 touchPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, furnitureLayer))
        {
            return hit.collider.gameObject == gameObject;
        }
        return false;
    }

    private Vector3 GetTouchWorldPosition(Vector2 touchPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorLayer | furnitureLayer))
        {
            return hit.point;
        }
        return transform.position;
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        int xCount = Mathf.RoundToInt(position.x / gridSize);
        int zCount = Mathf.RoundToInt(position.z / gridSize);

        Vector3 result = new Vector3(xCount * gridSize, originalY, zCount * gridSize);
        return result;
    }

    private bool CanMoveToPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapBox(position, furnitureCollider.bounds.extents, Quaternion.identity, furnitureLayer);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
