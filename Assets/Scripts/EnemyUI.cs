using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUI : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI nameText;

    private EnemyHealth currentEnemy;

    void Update()
    {
        if (currentEnemy != null)
        {
            float current = currentEnemy.GetCurrentHealth();
            float max = currentEnemy.GetMaxHealth();

            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, current / max, Time.deltaTime * 10f);

            // enemy dies
            if (current <= 0)
            {
                ClearTarget();
            }
        }
    }

    public void SetTarget(EnemyHealth enemy)
    {
        currentEnemy = enemy;

        nameText.text = enemy.GetName();

        gameObject.SetActive(true);
    }

    public void ClearTarget()
    {
        currentEnemy = null;
        gameObject.SetActive(false);
    }
}