using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHover : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    //FollowMouse() allows us to have an image of our tower follow the mousePosition so we can accurately tell where we are placing our tower
    private void FollowMouse(){
        if(spriteRenderer.enabled){
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    //ShowHover() sets the sprite and enables the spriteRenderer for our Hover object whenever we are about to place a tower.
    public void showHover(Sprite sprite){
        this.spriteRenderer.sprite = sprite;
        spriteRenderer.enabled = true;
    }

    //Whenever we have SetTower down onto its tile we must get rid of the Hover object and we call hideHover() to disable our spriteRenderer.
    public void hideHover(){
        spriteRenderer.enabled = false;
    }
}
