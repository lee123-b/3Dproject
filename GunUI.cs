using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    [Header("===== UI Reference =====")]
    [SerializeField] private Text ammoText; // Legacy UI Text

    [Header("===== Gun Reference =====")]
    [SerializeField] private GunController gunController;

    private void Start()
    {
        if (gunController == null)
            gunController = FindObjectOfType<GunController>();

        UpdateAmmoUI(); // 초기 UI 갱신
    }

    private void Update()
    {
        if (gunController != null && !gunController.IsReloading())
        {
            UpdateAmmoUI();
        }
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            int current = gunController.GetCurrentAmmo();
            int max = gunController.GetMaxAmmo();
            ammoText.text = $"{current} / {max}";
        }
    }
}
