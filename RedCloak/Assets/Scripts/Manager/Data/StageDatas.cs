using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StageDatas 
{
    List<StageData> stageData = new List<StageData>();

    public StageData GetStageData(int stage)
    { 
        return stageData[stage];
    }

    public void SetStageData()
    {
        for (int i = 0; i < stageData.Count; i++)
        {
            stageData[i] = new StageData();
        }
    }


}
