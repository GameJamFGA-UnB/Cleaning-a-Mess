using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Action : MonoBehaviour
{
    public List<Sprite> sprites;

    public float timeAnimation;

    public bool IsLoop;

    private Image image;
    private bool isOpen;
    private bool isPlaying;


    private void Start()
    {
        image = gameObject.GetComponent<Image>();

        if (IsLoop)
            StartCoroutine(StartAnimationLoop());
    }

    public void StartAnimation()
    {
        if (isPlaying) return;
        isPlaying = true;

        if (isOpen)
        {
            StartCoroutine(StartAnimationClose());
            isOpen = false;
        }
        else
        {
            StartCoroutine(StartAnimationOpen());
            isOpen = true;
        }
    }

    IEnumerator StartAnimationLoop()
    { 
        while (true)
        {
            foreach (Sprite sprite in sprites)
            {
                image.sprite = sprite;
                yield return new WaitForSeconds(timeAnimation);
            }
        }
    }

    IEnumerator StartAnimationClose()
    {
        for (int i = sprites.Count - 1; i >= 0; i--)
        {
            image.sprite = sprites[i];
            yield return new WaitForSeconds(timeAnimation);
        }

        isPlaying = false;
    }

    IEnumerator StartAnimationOpen()
    {
        foreach (Sprite sprite in sprites)
        {
            image.sprite = sprite;
            yield return new WaitForSeconds(timeAnimation);
        }

        isPlaying = false;
    }
}
