using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.InteropServices;

public class TweenHelper
{

    /**
     * To utilize this, the SpriteRenderer must be using the SpriteFlashable shader
     */
    public static Sequence FlashSprite(SpriteRenderer spriteRenderer, float duration)
    {
        Sequence sequence = DOTween.Sequence();
        Tween turnOnFlash = DOTween.To(() =>
        {
            return spriteRenderer.material.GetFloat("_FlashAmount");
        }, (newFlashAmount) =>
        {
            spriteRenderer.material.SetFloat("_FlashAmount", newFlashAmount);
        }, 1.0f, duration * 0.5f);

        Tween turnOffFlash = DOTween.To(() =>
        {
            return spriteRenderer.material.GetFloat("_FlashAmount");
        }, (newFlashAmount) =>
        {
            spriteRenderer.material.SetFloat("_FlashAmount", newFlashAmount);
        }, 0.0f, duration * 0.5f);

        sequence.Append(turnOnFlash);
        sequence.Append(turnOffFlash);

        return sequence;
    }

    public static Sequence ImageFadeInOut(Image image, float fadeInTime, float fadeOutTime, float fadeOutDelay)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(1.0f, fadeInTime).SetEase(Ease.InOutCirc));
        sequence.Append(image.DOFade(0.0f, fadeOutTime).SetDelay(fadeOutDelay));
        return sequence;
    }

    public static Sequence ImageFlyInFade(Image image, float flyInTime, float fadeOutTime, float fadeOutDelay, Ease easing)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(image.rectTransform.DOAnchorPos(new Vector2(0, 0), flyInTime).SetEase(easing));
        sequence.Append(image.DOFade(0.0f, fadeOutTime).SetDelay(fadeOutDelay));
        return sequence;
    }

    /**
     * Fades all children of the given RectTransform
     */

    public static Sequence FadeCanvasChildren(RectTransform parentTransform, float alpha, float duration)
    {
        Sequence sequence = DOTween.Sequence();
        Image[] images = parentTransform.GetComponentsInChildren<Image>();
        int i = 0;
        foreach (Image image in images)
        {
            if (i > 0)
            {
                sequence.Insert(0, image.DOFade(alpha, duration));
            }
            else
            {
                sequence.Append(image.DOFade(alpha, duration));
            }
            i++;
        }

        return sequence;
    }

    // With help from http://answers.unity3d.com/questions/583316/fluent-animation-from-orthographic-to-perspective.html
    public static Sequence ToggleCameraProjection(Camera camera, float duration)
    {
        bool currentMode = camera.orthographic;
        Matrix4x4 orthoMatrix, persMatrix;
        if (currentMode)
        {
            orthoMatrix = camera.projectionMatrix;
            camera.orthographic = false;
            camera.ResetProjectionMatrix();
            persMatrix = camera.projectionMatrix;
        }
        else
        {
            persMatrix = camera.projectionMatrix;
            camera.orthographic = true;
            camera.ResetProjectionMatrix();
            orthoMatrix = camera.projectionMatrix;
        }

        camera.orthographic = currentMode;
        Sequence sequence = DOTween.Sequence();
        if (currentMode)
        {
            // To perspective

            sequence.Append(DOTween.To(() => orthoMatrix.GetRow(0), (destVect4) =>
            {
                Matrix4x4 mat = camera.projectionMatrix;
                camera.projectionMatrix.SetRow(0, destVect4);
                mat.SetRow(0, destVect4);
                camera.projectionMatrix = mat;
            }, persMatrix.GetRow(0), duration)
                );

            sequence.Insert(0, DOTween.To(() => orthoMatrix.GetRow(1), (destVect4) =>
            {
                Matrix4x4 mat = camera.projectionMatrix;
                camera.projectionMatrix.SetRow(1, destVect4);
                mat.SetRow(1, destVect4);
                camera.projectionMatrix = mat;
            }, persMatrix.GetRow(1), duration)
                );

            sequence.Insert(0, DOTween.To(() => orthoMatrix.GetRow(2), (destVect4) =>
            {
                Matrix4x4 mat = camera.projectionMatrix;
                camera.projectionMatrix.SetRow(2, destVect4);
                mat.SetRow(2, destVect4);
                camera.projectionMatrix = mat;
            }, persMatrix.GetRow(2), duration)
                );

            sequence.Insert(0, DOTween.To(() => orthoMatrix.GetRow(3), (destVect4) =>
            {
                Matrix4x4 mat = camera.projectionMatrix;
                camera.projectionMatrix.SetRow(3, destVect4);
                mat.SetRow(3, destVect4);
                camera.projectionMatrix = mat;
            }, persMatrix.GetRow(3), duration)
                );
        }
        else
        {
            sequence.Append(DOTween.To(() => persMatrix.GetRow(0), (destVect4) =>
            {
                Matrix4x4 mat = camera.projectionMatrix;
                camera.projectionMatrix.SetRow(0, destVect4);
                mat.SetRow(0, destVect4);
                camera.projectionMatrix = mat;
            }, orthoMatrix.GetRow(0), duration)
                );

            sequence.Insert(0, DOTween.To(() => persMatrix.GetRow(1), (destVect4) =>
            {
                Matrix4x4 mat = camera.projectionMatrix;
                camera.projectionMatrix.SetRow(1, destVect4);
                mat.SetRow(1, destVect4);
                camera.projectionMatrix = mat;
            }, orthoMatrix.GetRow(1), duration)
                );

            sequence.Insert(0, DOTween.To(() => persMatrix.GetRow(2), (destVect4) =>
            {
                Matrix4x4 mat = camera.projectionMatrix;
                camera.projectionMatrix.SetRow(2, destVect4);
                mat.SetRow(2, destVect4);
                camera.projectionMatrix = mat;
            }, orthoMatrix.GetRow(2), duration)
                );

            sequence.Insert(0, DOTween.To(() => persMatrix.GetRow(3), (destVect4) =>
            {
                Matrix4x4 mat = camera.projectionMatrix;
                camera.projectionMatrix.SetRow(3, destVect4);
                mat.SetRow(3, destVect4);
                camera.projectionMatrix = mat;
            }, orthoMatrix.GetRow(3), duration)
                );
        }

        sequence.OnComplete(() =>
        {
            camera.orthographic = !currentMode;
            camera.ResetProjectionMatrix();
        });
        return sequence;
    }

    public static Sequence RunAfterDelay(TweenCallback callback, float delayDuration)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(delayDuration).AppendCallback(callback);
        return sequence;
    }

}
