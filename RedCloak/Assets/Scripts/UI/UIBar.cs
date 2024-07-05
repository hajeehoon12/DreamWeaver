using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIBar : MonoBehaviour
{
    [SerializeField] private Image healthIcon;
    [SerializeField] private Slider manaBar;
    [SerializeField] private Slider bossHealthBar;
    [SerializeField] private Slider damageBar;
    [SerializeField] private Transform BossBarPos;
    public RectTransform damageEffect;
    
    public static UIBar Instance;

    public Monster monster;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;

    }
    void Start()
    {
        BossBarPos.DOMoveY(-50, 0f);
        //CallBossBar();
        if (damageEffect == null)
        {
            Debug.Log("이펙트없음");
            return;
        }
        // 플레이어 체력 가져와서 
    }

    // Update is called once per frame
    void Update()
    {
        // 체력 시스템 추가 후 반영
        //bossHealthBar.value = monster.currentHealth / monster.maxHealth;
        
        // 플레이어 마나 시스템 추가 후 반영

        //매개변수로 대미지 전달
        //BossSetDamageEffect(5f);
    }

    public void SetBossBar(float currentHealth, float maxHealth, float Damage)
    {
        bossHealthBar.value = currentHealth / maxHealth;
        BossSetDamageEffect(Damage);
    }


    private void BossSetDamageEffect(float damage)
    {
        float fillWidth = bossHealthBar.fillRect.rect.width;
        float endPosition = (bossHealthBar.fillRect.anchoredPosition.x + fillWidth - 10f);

        damageEffect.anchoredPosition = new Vector2(endPosition, damageEffect.anchoredPosition.y);

        float width = (damage / monster.maxHealth) * bossHealthBar.fillRect.rect.width;
        damageEffect.sizeDelta = new Vector2(width, damageEffect.sizeDelta.y);
    }

    public void CallBossBar()
    {
        BossBarPos.DOMoveY(50, 1f);
    }

    public void CallBackBossBar()
    {
        BossBarPos.DOMoveY(-50, 1f);
    }

    private void SetPlayerHealth()
    {
        // 플레이어 체력 시스템 추가 후 반영 최대 체력을 참고해 반복문으로 이미지 생성, 배열이나 리스트를 사용할 것
        // 최대 체력을 참고해 healthIcon 생성
        // GemFront의 setactive를 false로
    }

    private void LowHealthEffect()
    {
        //플레이어 체력시스템 참고

    }
}
