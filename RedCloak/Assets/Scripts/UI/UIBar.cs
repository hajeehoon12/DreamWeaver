using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    [SerializeField] private GameObject heartParent;
    [SerializeField] private GameObject heart;
    [SerializeField] private GameObject heartFront;
    [SerializeField] private Slider manaBar;
    [SerializeField] private Slider bossHealthBar;
    //[SerializeField] private Slider maxHealthBar;
    [SerializeField] private GameObject damageEffect;
    public RectTransform damageEffectRect;
    
    public static UIBar Instance;

    public Archer archer;

    private float maxBossHealthBarWidth;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if (damageEffectRect == null)
        {
            Debug.Log("이펙트없음");
            return;
        }
        // 플레이어 체력 가져와서 

        maxBossHealthBarWidth = bossHealthBar.fillRect.rect.width;
        SetPlayerHealth();
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

        damageEffectRect.anchoredPosition = new Vector2(endPosition, damageEffectRect.anchoredPosition.y);

        //float width = (damage / archer.bossMaxHealth) * maxBossHealthBarWidth;
        float width = damage % archer.bossMaxHealth;
        damageEffectRect.sizeDelta = new Vector2(width, damageEffectRect.sizeDelta.y);
        damageEffect.SetActive(true);
        StartCoroutine(disableEffect());
    }

    private void SetPlayerHealth()
    {
        //int healthCount = (int)CharacterManager.Instance.Player.stats.playerMaxHP;
        int healthCount = 4;
        GameObject heartInstantiate;
        for(int i = 0; i < healthCount; i++)
        {
            heartInstantiate = Instantiate(heart);
            heartInstantiate.transform.SetParent(heartParent.transform);
        }
        // 피격시 GemFront의 setactive를 false로
    }

    private void LowHealthEffect()
    {
        //플레이어 체력시스템 참고

    }

    IEnumerator disableEffect()
    {
        yield return new WaitForSeconds(0.5f);

        damageEffect.SetActive(false);
    }
}
