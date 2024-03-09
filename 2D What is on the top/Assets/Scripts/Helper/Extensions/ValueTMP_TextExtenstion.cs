using System;
using TMPro;

namespace Extensions
{
    public static class ValueTMP_TextExtenstion
    {
        public static void Show<T>(this TMP_Text tmpText, T value) where T : IConvertible
        {
            tmpText.gameObject.SetActive(true);
            tmpText.text = value.ToString();
        }
        
        public static void Hide(this TMP_Text tmpText) =>
            tmpText.gameObject.SetActive(false);
    }
}
