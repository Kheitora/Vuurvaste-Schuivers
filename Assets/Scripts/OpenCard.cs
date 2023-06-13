using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCard : MonoBehaviour
{
    public GameObject card;

    public void OnButtonClick()
    {
        card.gameObject.SetActive(!card.gameObject.activeSelf); // Toggle the visibility of the canvas
    }
}
