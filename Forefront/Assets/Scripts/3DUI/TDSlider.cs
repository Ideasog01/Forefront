using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;

public class TDSlider : MonoBehaviour
{
    [SerializeField]
    private Transform sliderFill;

    [SerializeField]
    private float sliderValue; //From 0 - 1

    [SerializeField]
    private UnityEvent onSliderValueChange;

    public float SliderValue
    {
        get { return sliderValue; }
    }

    private void Awake()
    {
        sliderFill.localScale = new Vector3(1, 1, sliderValue);
        onSliderValueChange.Invoke();
    }

    public void UpdateSlider(float localZ)
    {
        //1.6 == 1

        //1.6 == 100

        sliderValue = (1f / 1.6f) * localZ;
        sliderFill.localScale = new Vector3(1, 1, sliderValue);
        onSliderValueChange.Invoke();
    }
}
