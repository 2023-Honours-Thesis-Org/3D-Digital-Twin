using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GraphGroup : MonoBehaviour
{
    public List<RawImage> images = new();
    public List<string> imageNames = new();
    public RawImage currentImage;
    public TextMeshProUGUI imageLabel;
    public int currentImageCount = 0;

    void Start()
    {
        currentImage.texture = images[currentImageCount % 4].texture;
        imageLabel.text = imageNames[currentImageCount % 4];
    }

    void Update()
    {
        currentImage.texture = images[currentImageCount % 4].texture;
        imageLabel.text = imageNames[currentImageCount % 4];
    }

    public void SelecteNext()
    {
        currentImageCount += 1;
        currentImage.texture = images[currentImageCount % 4].texture;
        imageLabel.text = imageNames[currentImageCount % 4];
    }

    public void SelectPrevious()
    {
        currentImageCount -= 1;
        currentImage.texture = images[currentImageCount % 4].texture;
        imageLabel.text = imageNames[currentImageCount % 4];
    }
}
