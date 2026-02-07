using TMPro;
using UnityEngine;

public sealed class TextView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetText(string text)
    {
        textMeshPro.text = text;
    }
}
