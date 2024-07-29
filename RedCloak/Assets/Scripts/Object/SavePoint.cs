using UnityEngine;

public class SavePoint : MonoBehaviour, IDamage
{    private void ShowSavePopup()
    {
        //저장하기 메세지 보여주기
        Debug.Log("저장하기");

        CharacterManager.Instance.SavePoint = this.transform.position;
        CameraManager.Instance.SaveStage();
        //CharacterManager.Instance.Player.controller.
        CharacterManager.Instance.Player.stats.playerHP = CharacterManager.Instance.Player.stats.playerMaxHP;
        CharacterManager.Instance.Player.battle.ChangeHealth(0);
        UIBar.Instance.SetCurrentHP();

    }

    public void GetDamage(float damage)
    {
        AudioManager.instance.PlaySFX("Save", 0.2f);
        ShowSavePopup();
    }
}
