using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Animations : MonoBehaviour
{
    [SerializeField] private List<Sprite> spritesStoped;
    [SerializeField] private List<Sprite> spritesTop;
    [SerializeField] private List<Sprite> spritesBotton;
    [SerializeField] private List<Sprite> spritesSideRight;
    [SerializeField] private float timeAnimation;

    [SerializeField] private List<Sprite> spritesBrokenTop;
    [SerializeField] private List<Sprite> spritesBrokenBotton;
    [SerializeField] private List<Sprite> spritesBrokenRight;

    private SpriteRenderer spriteRenderer;
    private bool changeMove = false;
    private bool isAnimation = false;
    private int direction = -1; // 0 = stoped | 1 = top | 2 = botton | 3 = right | 4 = left
    private bool waiting = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void LoadNewMove(Vector3 startPosition, Vector3 endPosition)
    {
        List<Sprite> spritesToUse;
        int type;

        // Calcule as diferenças absolutas entre as coordenadas X e Y.
        float deltaX = Mathf.Abs(endPosition.x - startPosition.x);
        float deltaY = Mathf.Abs(endPosition.y - startPosition.y);

        spriteRenderer.flipX = false;

        if (deltaX > deltaY)
        {
            if (endPosition.x > startPosition.x)
            {
                if (direction != 3)
                    direction = 3;
                else return;
                changeMove = true;
                type = 3;
                spritesToUse = spritesSideRight;
            }
            else
            {
                if (direction != 4)
                    direction = 4;
                else return;
                type = 4;
                changeMove = true;
                spritesToUse = spritesSideRight;
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            if (endPosition.y > startPosition.y)
            {
                if (direction != 1)
                    direction = 1;
                else return;

                type = 1;
                changeMove = true;
                spritesToUse = spritesTop;
            }
            else
            {
                if (direction != 2)
                    direction = 2;
                else return;

                type = 2;
                changeMove = true;
                spritesToUse = spritesBotton;
            }
        }

        StartCoroutine(WaitToChangeAnimation(spritesToUse, type));
    }

    public void LoadNewMove()
    {
        if (direction != 0)
            direction = 0;
        else return;

        changeMove = true;

        List<Sprite> spritesToUse = spritesStoped;
        if (!waiting)
            StartCoroutine(WaitToChangeAnimation(spritesToUse, 0));
    }

    public void LoadNewBroken()
    {
        spriteRenderer.flipX = false;
        List<Sprite> spritesToUse;

        if (direction == 1)
            spritesToUse = spritesBrokenTop;
        else if (direction == 2)
            spritesToUse = spritesBrokenBotton;
        else if (direction == 3)
            spritesToUse = spritesBrokenRight;
        else if (direction == 4)
        {
            spritesToUse = spritesBrokenRight;
            spriteRenderer.flipX = true;
        }
        else
            spritesToUse = spritesBrokenBotton;

        StartCoroutine(WaitToChangeAnimation(spritesToUse, 0));
    }

    IEnumerator WaitToChangeAnimation(List<Sprite> spritesToUse, int type)
    {
        //Debug.Log("Estou esperando pela animcao...");
        while(changeMove && isAnimation)
        {
            yield return null;
        }

        StartCoroutine(StartAnimationLoop(spritesToUse, type));
    }


    IEnumerator StartAnimationLoop(List<Sprite> sprites, int type)
    {
        if (changeMove)    
            changeMove = false;

        isAnimation = true;
        while (true)
        {
            foreach (Sprite sprite in sprites)
            {
                
                spriteRenderer.sprite = sprite;

                float currentTime = 0;

                while (currentTime < timeAnimation)
                {
                    if (changeMove || type != direction)
                    {
                        isAnimation = false;
                        changeMove = false;
                        yield break;
                    }
                    currentTime += 0.1f;

                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }
}
