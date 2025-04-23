using UnityEngine;
using UnityEngine.UI;

public class OffScreenIndicator : MonoBehaviour
{
    public Transform target;         // MathChest
    public Camera mainCam;
    public RectTransform arrowUI;   // UI Image RectTransform
    public float borderSize = 50f;  // Keeps arrow from edge of screen

    private void Update()
    {
        Vector3 screenPos = mainCam.WorldToScreenPoint(target.position);

        // Check if off-screen
        if (screenPos.z < 0 || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
        {
            arrowUI.gameObject.SetActive(true);

            // Clamp arrow position within screen
            screenPos.x = Mathf.Clamp(screenPos.x, borderSize, Screen.width - borderSize);
            screenPos.y = Mathf.Clamp(screenPos.y, borderSize, Screen.height - borderSize);
            screenPos.z = 0;

            // Position the arrow
            arrowUI.position = screenPos;

            // Rotate to face the target
            Vector3 dir = (target.position - mainCam.transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arrowUI.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
        else
        {
            arrowUI.gameObject.SetActive(false);
        }
    }
}
