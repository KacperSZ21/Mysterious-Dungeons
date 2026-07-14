using System.Collections;
using UnityEngine;

public class BerserkBuff : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] eyeSprites;
    private ClickToMove2D player;

    private Coroutine activeBuff;
    private float previousAttackSpeed;
    private Color[] defaultColors;

    private void Awake()
    {
        player = GetComponent<ClickToMove2D>();

        defaultColors = new Color[eyeSprites.Length];

        for (int i = 0; i < eyeSprites.Length; i++)
        {
            defaultColors[i] = eyeSprites[i].color;
        }
    }

    public void Activate(float newAttackSpeed, float duration)
    {
        if (activeBuff != null)
        {
            RestoreState();
            StopCoroutine(activeBuff);
        }

        activeBuff = StartCoroutine(BerserkRoutine(newAttackSpeed, duration));
    }

    IEnumerator BerserkRoutine(float attackSpeed, float duration)
    {
        previousAttackSpeed = player.attackCooldown;

        player.attackCooldown = attackSpeed;

        foreach (SpriteRenderer eye in eyeSprites)
        {
            eye.color = Color.red;
        }

        yield return new WaitForSeconds(duration);

        for (int i = 0; i < eyeSprites.Length; i++)
        {
            eyeSprites[i].color = defaultColors[i];
        }

        player.attackCooldown = previousAttackSpeed;

        activeBuff = null;
    }

    private void RestoreState()
    {
        player.attackCooldown = previousAttackSpeed;

        for (int i = 0; i < eyeSprites.Length; i++)
        {
            eyeSprites[i].color = defaultColors[i];
        }
    }
}