using Company.Cfg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticData;

public class EventLive2dAnim : MonoBehaviour
{

    public async void OnVideoSkipToFixedTimeOrFrame(int idSkip)
    {
        var skipDefine = StaticData.configExcel.GetSkipToFixedTimeOrFrameByID(idSkip);
        //1.获取自己的MonitorController
        MonitorController monitorController = GetComponentInParent<MonitorController>();
        EachMonitorController eachUIController = StaticData.GetEachUIController(monitorController);
        GameObject goEffect= await eachUIController.TransMonitorFragment(skipDefine.monitorTransType);
        foreach (var eachAnimId in skipDefine.WillSkipAnim)
        {
            MonitorStepsDefine stepMonitor = StaticData.configExcel.GetMonitorStepsByID(eachAnimId);
            Animator anim = monitorController.CurrMonitorVideo.listLive2dAnimator.Find(x => x.name == stepMonitor.NameLive2dAnimator);
            if (anim != null)
            {
                if (!skipDefine.isSkipToFrame)
                {
                    PlayAnimByTimeseconds(anim, skipDefine.TimeToSkip / 1000f);
                }
                else
                {
                    PlayAnimByFrame(anim, skipDefine.TimeToFrame);
                }
            }
        }
        if (goEffect != null)
        {
            await UniRx.Async.UniTask.Delay(300);
            goEffect.SetActive(false);
        }
    }
    private void PlayAnimByTimeseconds(Animator anim, float seconds)
    {
        anim.PlayInFixedTime("Clip1", 0, seconds);
    }
    private void PlayAnimByFrame(Animator anim, int frames)
    {
        int framesPerSecond = 60;
        float onceFrameTime = 1f / framesPerSecond;
        anim.PlayInFixedTime("Clip1", 0, frames * onceFrameTime);
    }
}
