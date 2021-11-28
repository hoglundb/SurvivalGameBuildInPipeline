using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipableSlotSelector : MonoBehaviour
{
    [SerializeField] private Color _highlightedColor;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Transform _imageTransform;
    private Image _image;

    private void Awake()
    {
        _image = _imageTransform.GetComponent<Image>();
    }


    public void Highlight()
    {
        _image.color = _highlightedColor;
    }


    public void Unhighlight()
    {
        _image.color = _defaultColor;
    }
}
