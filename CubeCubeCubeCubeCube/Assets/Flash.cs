using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Flash : MonoBehaviour {

    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();
        image.color = new Color(1, 1, 1);
    }
    void Update () {
        Color newColor = image.color;
        newColor.a -= Time.deltaTime * 2f;
        if(newColor.a <= 0)
        {
            Destroy(transform.parent.gameObject);
            
        }
        image.color = newColor;
	}
}
