using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private Slider playerStaminaBar;
    [SerializeField] private Slider bossHealthBar;
    [SerializeField] private Slider damageBar;
    public RectTransform damageEffect;
    

    public Monster monster;

    // Start is called before the first frame update
    void Start()
    {
        if (damageEffect == null)
        {
            Debug.Log("이펙트없음");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 체력 시스템 추가 후 반영
        bossHealthBar.value = monster.currentHealth / monster.maxHealth;

        //매개변수로 대미지 전달
        SetDamageEffect(5f);
    }

    private void SetDamageEffect(float damage)
    {
        float fillWidth = bossHealthBar.fillRect.rect.width;
        float endPosition = (bossHealthBar.fillRect.anchoredPosition.x + fillWidth - 10f);

        damageEffect.anchoredPosition = new Vector2(endPosition, damageEffect.anchoredPosition.y);

        float width = (damage / monster.maxHealth) * bossHealthBar.fillRect.rect.width;
        damageEffect.sizeDelta = new Vector2(width, damageEffect.sizeDelta.y);
    }
}
