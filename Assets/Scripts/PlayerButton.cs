using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using MEC;
public class PlayerButton : MonoBehaviour, IPointerDownHandler
{
    public InteractionManager.PlayerInteractions Interaction;
    public UnityEngine.UI.RawImage BGImage;
    public Texture2D ImageTexture;
    public Color Colorchange;
    private float CurrentTime = 0;
    bool _resetWave = false;
    //what if you got all the pixels of x color and they reacted in a certain way :o
    //the juice!
    public void OnPointerDown(PointerEventData eventData)
    {
        Timing.RunCoroutine(ClickColorChange(Colorchange,true));
        _resetWave = true;
        InteractionManager.Instance.ButtonPress(Interaction);
    }


    public void SetTextureColor(Color color)
    {
        for (int x = 0; x < ImageTexture.width; x++)
        {
            for (int i = 0; i < ImageTexture.height; i++)
            {
                ImageTexture.SetPixel(x, i, color);
            }
        }
        ImageTexture.Apply();
    }

    private void Update()
    {
        if (_resetWave)
        {
            CurrentTime += Time.deltaTime;
            if (CurrentTime > InteractionManager.Instance.TimeBetweenWaves)
            {
                _resetWave = false;
                CurrentTime = 0;
                Timing.RunCoroutine(ClickColorChange(InteractionManager.Instance.GetColorForInteration(Interaction)));
            }
        }
    }

    private IEnumerator<float> ClickColorChange(Color color, bool lerp=false)
    {
        int DownX = ImageTexture.width / 2, UpX = ImageTexture.width / 2;
        int DownY = ImageTexture.height / 2, UpY = ImageTexture.height / 2;
        while (DownX >= 0 && DownY >= 0 && UpX <= ImageTexture.width && UpY <= ImageTexture.height)
        {
            //do all the rows of pixels required
            for (int i = 0; i <= InteractionManager.Instance.PixelsPerRow; i++)
            {
                //bot row
                for (int x = DownX; x < UpX; x++)
                {
                    if (lerp)
                    //alt the color alpha by distance from middle
                    color.a = Mathf.InverseLerp(0, ImageTexture.width / 2, DownY + i);

                    ImageTexture.SetPixel(x, DownY + i, color);                   
                }
            }

            //do all the rows of pixels required
            for (int i = 0; i < InteractionManager.Instance.PixelsPerRow; i++)
            {
                //left column
                for (int y = DownY; y <= UpY; y++)
                {
                    if (lerp)
                        //alt the color alpha by distance from middle
                        color.a = Mathf.InverseLerp(0, ImageTexture.width / 2, DownX + i);
                    ImageTexture.SetPixel(DownX + i, y, color);
                }
            }
            //do all the rows of pixels required
            for (int i = 0; i < InteractionManager.Instance.PixelsPerRow; i++)
            {
                //top row
                for (int x = DownX; x <= UpX; x++)
                {
                    if (lerp)
                        //alt the color alpha by distance from middle
                        color.a = Mathf.InverseLerp(ImageTexture.width, ImageTexture.width / 2, UpY - i);
                    ImageTexture.SetPixel(x, UpY-i, color);
                }
            }
            //do all the rows of pixels required
            for (int i = 0; i < InteractionManager.Instance.PixelsPerRow; i++)
            {
                //right column
                for (int y = DownY; y <= UpY; y++)
                {
                    if (lerp)
                        //alt the color alpha by distance from middle
                        color.a = Mathf.InverseLerp(ImageTexture.width, ImageTexture.width / 2, UpX - i);
                    ImageTexture.SetPixel(UpX-i, y, color);
                }
            }
            ////bot left
            //ImageTexture.SetPixel(DownX, DownY, color);
            ////top right
            //ImageTexture.SetPixel(UpX, UpY, color);
            ////top left
            //ImageTexture.SetPixel(DownX, UpY, color);
            ////bot right
            //ImageTexture.SetPixel(UpX, DownY, color);
            ImageTexture.Apply();
            yield return Timing.WaitForSeconds(InteractionManager.Instance.TimeBetweenPixels);
            DownX -= InteractionManager.Instance.PixelsPerRow;
            DownY -= InteractionManager.Instance.PixelsPerRow;
            UpX += InteractionManager.Instance.PixelsPerRow;
            UpY += InteractionManager.Instance.PixelsPerRow;
        }

    }


}
