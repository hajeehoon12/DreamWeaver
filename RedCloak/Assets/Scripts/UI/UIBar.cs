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
    public RectTransform endRect;
    

    public Monster monster;

    // Start is called before the first frame update
    void Start()
    {
        GetFillEndPosition();
    }

    // Update is called once per frame
    void Update()
    {
        // 체력 시스템 추가 후 반영
        //bossHealthBar.value = monster.currentHealth / monster.maxHealth;

        GetFillEndPosition();
    }

    private void GetFillEndPosition()
    {
        if(endRect == null)
        {
            return;
        }

        float fillWidth = bossHealthBar.fillRect.rect.width;
        float endPosition = (bossHealthBar.fillRect.anchoredPosition.x + fillWidth - 10f);

        endRect.anchoredPosition = new Vector2(endPosition, endRect.anchoredPosition.y);
    }

    private void SetDamageEffectWidth(float width)
    {

    }
}
