using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RemapImageColor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float inputMin;
    [SerializeField] float inputMax;
    [SerializeField] Color outputMin;

    private Image _image;
    private Color _defaultColor;

    private void Start()
    {
        _image = GetComponent<Image>();
        _defaultColor = _image.color;
    }

    internal void Change(float value)
    {
        float t = Mathf.InverseLerp(inputMin, inputMax, value);
        _image.color = Color.Lerp(outputMin, _defaultColor, t);
    }
}
