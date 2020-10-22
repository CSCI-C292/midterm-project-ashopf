using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour
{
    TextMeshProUGUI health;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        health.text = "Health: " + Mathf.FloorToInt(Base.health).ToString();
    }
}
