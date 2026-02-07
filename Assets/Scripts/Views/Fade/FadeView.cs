using UnityEngine;
using UnityEngine.UI;

public sealed class FadeView : MonoBehaviour
{
    [SerializeField] private Image fadeImage;

    public void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}
