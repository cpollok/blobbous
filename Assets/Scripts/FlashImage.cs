using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlashImage : MonoBehaviour
{
    Image blinkImage;
    Color startColor;

    void Start(){
        blinkImage = GetComponent<Image>();
        startColor = blinkImage.color;
    }
    
    public void BlinkBlack() {
        StartCoroutine(Blink(Color.black, 1, 5));
    }

    public IEnumerator ColorLerpTo(Color color, float duration) {
        float elapsedTime = 0.0f;
        while (elapsedTime < duration) {
            blinkImage.color = Color.Lerp(blinkImage.color, color, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    
    public IEnumerator Blink(Color _blinkIn, int _blinkCount, float _totalBlinkDuration) {
        float fractionalBlinkDuration = _totalBlinkDuration / _blinkCount;

        for (int blinked = 0; blinked < _blinkCount; blinked++) {
            float halfFracitonalBlinkDuration = fractionalBlinkDuration * 0.5f;
            yield return StartCoroutine(ColorLerpTo(_blinkIn, halfFracitonalBlinkDuration));
            yield return StartCoroutine(ColorLerpTo(Color.clear, halfFracitonalBlinkDuration));
        }
    }
}
