using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTimeUI : MonoBehaviour
{
    public GameTime gameTime;
    public Sprite pause;
    public Sprite play;
    public Button pausePlayButton;
    // Start is called before the first frame update
    void Start()
    {
        pausePlayButton.image.sprite = pause;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameTime.stopTime)
        {
            pausePlayButton.image.sprite = play;
        } 
        else 
        {
            pausePlayButton.image.sprite = pause;
        }
    }
}
