using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIBar : MonoBehaviour
{
    [SerializeField] private GameObject heartParent;
    [SerializeField] private GameObject heart;
    [SerializeField] private GameObject heartFront;
    [SerializeField] private GameObject damageEffect;
    [SerializeField] private GameObject lowHealthEffect;
    [SerializeField] private GameObject playerHitEffect;
    [SerializeField] private Slider bossHealthBar;
    //[SerializeField] private Slider maxHealthBar;
    [SerializeField] private Image manaBar;
    [SerializeField] private Transform BossBarPos;
    [SerializeField] private TMP_Text goldTxt;
    public RectTransform damageEffectRect;
    
    public static UIBar Instance;

    private float maxBossHealthBarWidth;
    private float playerCurrnetMana;
    private float playerMaxMana;
    public TMP_Text bossText;

    public List<GameObject> heartsFront = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        BossBarPos.DOMoveY(-200, 0f);
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
        LowHealthEffect();

        //ApplyDamage();
    }

    public void UpdateGold() // update player current Gold
    {
        goldTxt.text = CharacterManager.Instance.Player.stats.playerGold.ToString("N0");
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

        float width = damage % maxHealth * 3;// * 100;
        damageEffectRect.sizeDelta = new Vector2(width, damageEffectRect.sizeDelta.y);
        damageEffect.SetActive(true);
        StartCoroutine(DisableBossDamageEffect());
    }

    public void CallBossBar(string bossName)
    {
        BossBarPos.DOMoveY(50, 1f);
        StartCoroutine(BossTextChange(bossName));

    }

    IEnumerator BossTextChange(string bossName)
    {
        bossText.text = "";
        yield return new WaitForSeconds(0.1f);
        string fillName = "";
        DOTween.To(() => "", str => fillName = str, bossName, 1f);
        float time = 0;
        while (time < 1)
        {
            bossText.text = fillName;
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }

    public void CallBackBossBar()
    {
        BossBarPos.DOMoveY(-200, 1f);
    }

    public void SetPlayerHealth()
    {
        int healthCount = (int)CharacterManager.Instance.Player.stats.playerMaxHP;
        //int healthCount = (int)CharacterManager.Instance.Player.stats.playerHP;
        GameObject heartInstantiate;
        for(int i = 0; i < healthCount; i++)
        {
            heartInstantiate = Instantiate(heart, heartParent.transform);
            heartFront = heartInstantiate.transform.Find("GemFront").gameObject;
            heartsFront.Add(heartFront);
            //heartInstantiate.transform.SetParent(heartParent.transform);
        }
    }

    public void UpdateMaxHP(int addMaxHP)
    {
        GameObject heartInstantiate;
        for (int i = 0; i < addMaxHP; i++)
        {
            heartInstantiate = Instantiate(heart, heartParent.transform);
            heartFront = heartInstantiate.transform.Find("GemFront").gameObject;
            CharacterManager.Instance.Player.stats.playerHP = CharacterManager.Instance.Player.stats.playerMaxHP;
            heartsFront.Add(heartFront);
            //heartInstantiate.transform.SetParent(heartParent.transform);
        }
         
        SetCurrentHP();
    }

    public void ApplyDamage()
    {
        float currntPlayerHealth = CharacterManager.Instance.Player.stats.playerHP;

        for (int i = heartsFront.Count - 1; i >= 0; i--)
        {
            heartsFront[i].SetActive(i < currntPlayerHealth);
        }

        StartCoroutine(HitEffectFade());
    }

    public void SetCurrentHP()
    {
        float currntPlayerHealth = CharacterManager.Instance.Player.stats.playerHP;

        for (int i = heartsFront.Count - 1; i >= 0; i--)
        {
            heartsFront[i].SetActive(i < currntPlayerHealth);
        }
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

    IEnumerator DisableBossDamageEffect()
    {
        damageEffect.GetComponent<Image>().DOFade(1, 0f);
        damageEffect.GetComponent<Image>().DOFade(0, 0.5f);

        yield return new WaitForSeconds(0.5f);

        damageEffect.SetActive(false);
    }

    IEnumerator HitEffectFade()
    {
        playerHitEffect.SetActive(true);

        Image playerHitEffectImage = playerHitEffect.GetComponent<Image>();

        playerHitEffectImage.DOFade(1, 0.3f);
        yield return new WaitForSeconds(0.3f);

        playerHitEffectImage.DOFade(0, 1f);

        yield return new WaitForSeconds(1f);

        playerHitEffect.SetActive(false);
    }
}
