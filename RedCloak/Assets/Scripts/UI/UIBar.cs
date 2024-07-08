using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIBar : MonoBehaviour
{
    [SerializeField] private GameObject heartParent;
    [SerializeField] private GameObject heart;
    [SerializeField] private GameObject heartFront;
    [SerializeField] private Slider bossHealthBar;
    //[SerializeField] private Slider maxHealthBar;
    [SerializeField] private Image manaBar;
    [SerializeField] private GameObject damageEffect;
    public RectTransform damageEffectRect;
    [SerializeField] private Slider damageBar;
    [SerializeField] private Transform BossBarPos;
    
    public static UIBar Instance;
    public Archer archer;
    public List<GameObject> heartsFront = new List<GameObject>();

    private float maxBossHealthBarWidth;
    private float playerMaxMana;
    private float playerCurrnetMana;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;

    }
    void Start()
    {
        BossBarPos.DOMoveY(-50, 0f);
        //CallBackBossBar();
        //CallBossBar();

        // 플레이어 체력 가져와서 

        if (damageEffectRect == null)
            maxBossHealthBarWidth = bossHealthBar.fillRect.rect.width;

        SetPlayerHealth();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMana();
        SetMana(playerCurrnetMana, playerMaxMana);
        UpdateHealth();


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
        int healthCount = CharacterManager.Instance.Player.stats.playerMaxHP;

        GameObject heartInstantiate;
        for(int i = 0; i < healthCount; i++)
        {
            heartInstantiate = Instantiate(heart, heartParent.transform);
            heartFront = heartInstantiate.transform.Find("GemFront").gameObject;
            heartsFront.Add(heartFront);
            //heartInstantiate.transform.SetParent(heartParent.transform);
        }
        // 피격시 GemFront의 setactive를 false로
    }

    public void UpdateHealth()
    {
        int playerCurrentHealth = CharacterManager.Instance.Player.stats.playerHP;

        for (int i = heartsFront.Count - 1; i >= 0; i--)
        {
            heartsFront[i].SetActive(i < playerCurrentHealth);
        }
    }

    private void LowHealthEffect()
    {
        //플레이어 체력시스템 참고

    }

    private void UpdateMana()
    {
        playerMaxMana = CharacterManager.Instance.Player.stats.playerMaxMP;
        playerCurrnetMana = CharacterManager.Instance.Player.stats.playerMP;
    }

    private void SetMana(float currntMana, float maxMana)
    {
        float manaRate = currntMana / maxMana;
        
        manaBar.fillAmount = manaRate;
    }

    IEnumerator disableEffect()
    {
        yield return new WaitForSeconds(0.5f);

        damageEffect.SetActive(false);
    }
}
