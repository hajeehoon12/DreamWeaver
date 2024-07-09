using UnityEngine;

public class SavePoint : MonoBehaviour, IDamage
{    private void ShowSavePopup()
    {
        //저장하기 메세지 보여주기
        Debug.Log("저장하기");
    }

    public void GetDamage(float damage)
    {
        AudioManager.instance.PlaySFX("Success", 0.2f);
        ShowSavePopup();
    }
}
