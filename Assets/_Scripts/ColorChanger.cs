using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Action<Color> onColorChanged;
    [SerializeField] private Color textColor;
    private TextMeshProUGUI text;
    void Awake() {
        text = GetComponent<TextMeshProUGUI>();
        text.faceColor = textColor;
        text.color = textColor;
    }

    void OnValidate() {
        text = GetComponent<TextMeshProUGUI>();
        text.faceColor = textColor;
        text.color = textColor;
    }
}
