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

    public void SetStageData(int stage, StageData currentStage)
    {
        stageData[stage] = currentStage;
    }


}
