using R3;
using UnityEngine;
using UnityEngine.UI;

public sealed class EndingView : MonoBehaviour
{
    [SerializeField] private Button surveyButton;

    public Observable<Unit> OnSurveyButtonClicked => surveyButton.OnClickAsObservable();
}
