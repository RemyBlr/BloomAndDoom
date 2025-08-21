using UnityEngine;
using UnityEngine.UI;

public class ClassButtonUI : MonoBehaviour
{
    public Image portrait;
    public Image border;

    public void SetSelected(bool isSelected) {
        if(border != null)
            border.enabled = isSelected;
    }
}
