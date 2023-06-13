using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlip : MonoBehaviour
{
    public Image original;
    public Sprite NewSprite;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("NewImage", 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewImage() {
        original.sprite = NewSprite;
        button.gameObject.SetActive(true);
    }
}
