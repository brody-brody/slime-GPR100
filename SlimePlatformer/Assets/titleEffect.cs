using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class titleEffect : MonoBehaviour
{
    [SerializeField] TMP_Text titleText;

    float t = 0;
    Color startColor = new Color32(255, 255, 255, 0);
    Color endColor = new Color32(255, 255, 255, 255);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        titleText.color = startColor;

        while (t < 1)
        {
            titleText.color = Color.Lerp(startColor, endColor, t);
            t += Time.deltaTime / 5f;
            Debug.Log(t);
        }
    }
}
