using System.Collections;
using UnityEngine;
using UnityEngine.UI;  // Legacy UI Text

public class GunController : MonoBehaviour
{
    [Header("===== References =====")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private GameObject tracerPrefab;
    [SerializeField] private GameObject impactEffectPrefab;

    [Header("===== Gun Settings =====")]
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private float reloadTime = 1.5f;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private float range = 100f;

    [Header("===== Damage per BodyPart =====")]
    [SerializeField] private float damageHead = 30f;
    [SerializeField] private float damageBody = 20f;
    [SerializeField] private float damageLimb = 10f;

    [Header("===== Layer Mask =====")]
    [SerializeField] private LayerMask hitMask;

    [Header("===== Tracer Settings =====")]
    [SerializeField] private float tracerDuration = 0.1f;

    [Header("===== Crosshair =====")]
    [SerializeField] private float crosshairSize = 10f;
    private Texture2D crosshairTexture = null;

    [Header("===== Ammo UI =====")]
    [SerializeField] private Text ammoText;  // Legacy UI Text

    private int currentAmmo;
    private bool isReloading = false;
    private float nextFireTime = 0f;
    private bool canShoot = true;

    private void Awake()
    {
        // 카메라 자동 할당
        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>() ?? Camera.main;

        // 탄 초기화 & UI
        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        // 크로스헤어 텍스처 생성
        crosshairTexture = new Texture2D(1, 1);
        crosshairTexture.SetPixel(0, 0, Color.red);
        crosshairTexture.Apply();
    }

    private void Update()
    {
        if (isReloading) return;

        // 재장전
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(ReloadRoutine());
            return;
        }

        // 사격
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && canShoot)
        {
            if (currentAmmo > 0)
                Shoot();
            else
                Debug.Log("총알이 없습니다. (R 키로 재장전)");
        }
    }

    private void Shoot()
    {
        nextFireTime = Time.time + fireRate;
        currentAmmo--;
        UpdateAmmoUI();

        Debug.Log($"사격! 남은 총알: {currentAmmo}/{maxAmmo}");

        // 머즐 플래시
        if (muzzleFlashPrefab != null && firePoint != null)
        {
            var flash = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
            Destroy(flash, 0.2f);
        }

        // 히트스캔 Raycast
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, range, hitMask))
        {
            // 트레이서
            if (tracerPrefab != null && firePoint != null)
            {
                var tracer = Instantiate(tracerPrefab);
                var lr = tracer.GetComponent<LineRenderer>();
                lr.SetPosition(0, firePoint.position);
                lr.SetPosition(1, hitInfo.point);
                Destroy(tracer, tracerDuration);
            }

            // 임팩트 이펙트
            if (impactEffectPrefab != null)
                Instantiate(impactEffectPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

            // 부위별 데미지 계산
            float appliedDamage = damageBody;
            var hb = hitInfo.collider.GetComponent<Hitbox>();
            if (hb != null)
            {
                switch (hb.partType)
                {
                    case Hitbox.BodyPart.Head:
                        appliedDamage = damageHead;
                        break;
                    case Hitbox.BodyPart.Body:
                        appliedDamage = damageBody;
                        break;
                    case Hitbox.BodyPart.Hand:
                    case Hitbox.BodyPart.Foot:
                        appliedDamage = damageLimb;
                        break;
                }
            }

            // Health 컴포넌트에 데미지 전달
            var targetHealth = hitInfo.collider.GetComponentInParent<Health>();
            if (targetHealth != null)
                targetHealth.TakeDamage(appliedDamage);
        }
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;
        Debug.Log("재장전 중...");
        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("재장전 완료");
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = $"{currentAmmo} / {maxAmmo}";
    }

    private void OnGUI()
    {
        if (crosshairTexture == null) return;
        float x = (Screen.width - crosshairSize) * 0.5f;
        float y = (Screen.height - crosshairSize) * 0.5f;
        GUI.DrawTexture(new Rect(x, y, crosshairSize, crosshairSize), crosshairTexture);
    }

    // 외부 제어용
    public void SetShootEnabled(bool e) => canShoot = e;
    public int GetCurrentAmmo() => currentAmmo;
    public int GetMaxAmmo() => maxAmmo;
    public bool IsReloading() => isReloading;
}
