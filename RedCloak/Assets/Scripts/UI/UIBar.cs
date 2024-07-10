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
    [SerializeField] private GameObject damageEffect;
    [SerializeField] private GameObject lowHealthEffect;
    [SerializeField] private Slider bossHealthBar;
    //[SerializeField] private Slider maxHealthBar;
    [SerializeField] private Image manaBar;
    [SerializeField] private Transform BossBarPos;
    public RectTransform damageEffectRect;
    
    public static UIBar Instance;

    private float maxBossHealthBarWidth;
    private float playerCurrnetMana;
    private float playerMaxMana;

    public List<GameObject> heartsFront = new List<GameObject>();

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

        if (damageEffectRect == null)
            maxBossHealthBarWidth = bossHealthBar.fillRect.rect.width;
        SetPlayerHealth();
        lowHealthEffect.SetActive(false);
    }
    private void Update()
    {
        UpdateMana();
        //ApplyDamage();
    }

    public void SetBossBar(float currentHealth, float maxHealth, float Damage)
    {
        bossHealthBar.value = currentHealth / maxHealth;
        BossSetDamageEffect(currentHealth, maxHealth, Damage);
    }


    private void BossSetDamageEffect(float currentHealth, float maxHealth, float damage)
    {
        float fillWidth = bossHealthBar.fillRect.rect.width;
        float endPosition = (bossHealthBar.fillRect.anchoredPosition.x + fillWidth - 10f);



        damageEffectRect.anchoredPosition = new Vector2(endPosition, damageEffectRect.anchoredPosition.y);

        float width = damage % maxHealth * 2;// * 100;
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
        //int healthCount = (int)CharacterManager.Instance.Player.stats.playerMaxHP;
        int healthCount = 4;
        GameObject heartInstantiate;
        for(int i = 0; i < healthCount; i++)
        {
            heartInstantiate = Instantiate(heart, heartParent.transform);
            heartFront = heartInstantiate.transform.Find("GemFront").gameObject;
            heartsFront.Add(heartFront);
            //heartInstantiate.transform.SetParent(heartParent.transform);
        }
    }

    public void ApplyDamage()
    {
        float currntPlayerHealth = CharacterManager.Instance.Player.stats.playerHP;

        for (int i = heartsFront.Count - 1; i >= 0; i--)
        {
            heartsFront[i].SetActive(i < currntPlayerHealth);
        }

        LowHealthEffect();
    }

    private void LowHealthEffect()
    {
        if(CharacterManager.Instance.Player.stats.playerHP == 1)
        {
            lowHealthEffect.SetActive(true);
        }

        else
        {
            lowHealthEffect.SetActive(false);
        }
        // 화면 효과 추가
    }

    public void SetMana(float currntMana, float maxMana)
    {
        manaBar.fillAmount = currntMana / maxMana;
    }

    private void UpdateMana()
    {
        playerCurrnetMana = CharacterManager.Instance.Player.stats.playerMP;
        playerMaxMana = CharacterManager.Instance.Player.stats.playerMaxMP;
        SetMana(playerCurrnetMana, playerMaxMana);
    }

    IEnumerator disableEffect()
    {
        damageEffect.GetComponent<Image>().DOFade(1, 0f);
        damageEffect.GetComponent<Image>().DOFade(0, 0.5f);

        yield return new WaitForSeconds(0.5f);

        damageEffect.SetActive(false);
    }
}
