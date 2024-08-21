using UnityEngine;

public class SavePoint : MonoBehaviour, IDamage
{    private void ShowSavePopup()
    {
        //저장하기 메세지 보여주기
        //Debug.Log("저장하기");

        CameraManager.Instance.SaveStage();
        //CharacterManager.Instance.Player.controller.
        CharacterManager.Instance.Player.stats.playerHP = CharacterManager.Instance.Player.stats.playerMaxHP;
        CharacterManager.Instance.Player.battle.ChangeHealth(0);
        UIManager.Instance.uiBar.SetCurrentHP();
        CharacterManager.Instance.SaveInfo();
        SaveLoad.Save("MobData", MonsterDataManager.mobArray);
        SaveLoad.Save("DoorData", CharacterManager.Instance.doorData);
        SaveLoad.Save("RewardBoxData", RewardBoxDataManager.array);
        SaveLoad.Save("PlayerData", CharacterManager.Instance.playerData);
    }

    public void GetDamage(float damage)
    {
        AudioManager.instance.PlaySFX("Save", 0.2f);
        ShowSavePopup();
    }
}
