using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // <-- Add this for TextMeshPro support

public class TetroSkill : MonoBehaviour
{
    [Header("Blinding Overlay")]
    public GameObject blindOverlay;

    [Header("Cooldown Settings")]
    public float cooldownTime = 60f;
    private float cooldownTimer = 0f;
    public bool isOnCooldown = true;

    [Header("Cooldown UI")]
    public TextMeshProUGUI cooldownText; // <-- Assign in inspector
    private string cooldownTempText;
    private string originalText;
    private Color originalTextColor;

    private Coroutine pulseRoutine;

    private void Start()
    {
        originalText = cooldownText.text;
        cooldownTimer = cooldownTime;
    }

    void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownTimer = Mathf.Max(cooldownTimer, 0f);
            cooldownTempText = Mathf.CeilToInt(cooldownTimer).ToString();

            if (cooldownText != null)
            {
                cooldownText.text = cooldownTempText;
                cooldownText.color = Color.red; // Set to red while counting down
            }

            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;

                if (cooldownText != null)
                {
                    cooldownText.text = "OK"; // Should be "0"
                    cooldownText.color = Color.green; // Restore original color
                }
            }
        }
    }

    public void ActivateSkill()
    {
        if (isOnCooldown || blindOverlay == null)
        {
            Debug.Log("Skill is on cooldown or overlay is missing.");
            AudioManager.Instance.sfxSource.PlayOneShot(AudioManager.Instance.invalid);
            return;
        }

        // Activate pulse
        if (pulseRoutine != null)
            StopCoroutine(pulseRoutine);
        pulseRoutine = StartCoroutine(BlindPulse());

        // Start cooldown
        isOnCooldown = true;
        cooldownTimer = cooldownTime;
    }

    private IEnumerator BlindPulse()
    {
        blindOverlay.SetActive(true);

        Image overlayImage = blindOverlay.GetComponent<Image>();
        Color baseColor = overlayImage.color;

        // Step 1: Set to full opacity
        overlayImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f);

        // Hold full opacity for 3 seconds
        yield return new WaitForSeconds(2.5f);

        // Step 2: Fade out over 3 seconds
        float fadeDuration = 1.5f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            overlayImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        overlayImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
        blindOverlay.SetActive(false);
    }


    // Optional method to retrieve the current cooldown string
    public string GetCooldownDisplay()
    {
        return cooldownTempText;
    }
}
