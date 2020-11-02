using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerButton : MonoBehaviour
{
    //handles the TowerButtons in the shop 

    [SerializeField] private GameObject towerPrefab;

    [SerializeField] private Sprite sprite;

    [SerializeField] private int towerPrice;

    [SerializeField] private TextMeshProUGUI priceText;
    
    public GameObject TowerPrefab{
        get{
            return towerPrefab;
        }
    }

    public Sprite Sprite { 
        get{
            return sprite;
        }
    }

    public int Price{
        get{
            return towerPrice;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        priceText.text = "$" + towerPrice;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
