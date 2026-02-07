using R3;

public sealed class TextModel
{
    private readonly ReactiveProperty<string> currentText = new("");
    public ReadOnlyReactiveProperty<string> CurrentText => currentText;

    private readonly ReactiveProperty<bool> isActive = new(false);
    public ReadOnlyReactiveProperty<bool> IsActive => isActive;

    public void SetText(string text)
    {
        currentText.Value = text;
        isActive.Value = !string.IsNullOrEmpty(text);
    }

    public void ClearText()
    {
        currentText.Value = "";
        isActive.Value = false;
    }
}
