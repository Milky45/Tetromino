using UnityEngine;
using UnityEngine.UI;

public class CRTFilter : MonoBehaviour
{
    public CRTFilterEffect crtFilter;
    public Toggle toggleUI;

    private void Start()
    {
        if (crtFilter.enabled == true)
        {
            toggleUI.isOn = false;
            crtFilter.enabled = true;
        }
        else if(crtFilter.enabled == false)
        {
            toggleUI.isOn = true;
            crtFilter.enabled = false;
        }
    }

    public void OnClickToggle()
    {
        // Toggle the CRT filter on or off
        if (crtFilter != null)
        {
            crtFilter.enabled = !crtFilter.enabled;
            Debug.Log("CRT Filter toggled: " + crtFilter.enabled);
        }
        else
        {
            Debug.LogWarning("CRT Filter not assigned.");
        }
    }
}
