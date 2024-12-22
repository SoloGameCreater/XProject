
using Localizetion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderZero : MonoBehaviour
{
    public Slider Slider;
    public TextMeshProUGUI ProgressText;

    private void OnDisable()
    {
        Slider.value = 0f;
        ProgressText.SetText(LocalizationManager.Instance.GetLocalizedStringWithFormats("&key.UI_loading_progress_text", 0.ToString("#0.0")));
    }
}