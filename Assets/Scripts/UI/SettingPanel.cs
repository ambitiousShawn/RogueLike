using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel :BasePanel
{
    private Button Btn_Continue;
    private Button Btn_Instruction;
    private Button Btn_Exit;
    private Slider Slider_Volume;
    private Slider Slider_Sound;

    protected override void Init()
    {
        //组件初始化
        Btn_Continue = GetControl<Button>("Btn_Continue");
        Btn_Instruction = GetControl<Button>("Btn_Instruction");
        Btn_Exit = GetControl<Button>("Btn_Exit");
        Slider_Volume = GetControl<Slider>("Slider_Volume");
        Slider_Sound = GetControl<Slider>("Slider_Sound");

        //继续游戏
        Btn_Continue.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("SettingPanel");
        });

        //操作面板
        Btn_Instruction.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<InstructionPanel>("InstructionPanel");
            UIManager.Instance.HidePanel("SettingPanel");
        });
        //退出游戏
        Btn_Exit.onClick.AddListener(() =>
        {
            //退出游戏
            print("退出游戏");
            Application.Quit();
        });
        //拖动滑动条改变音量
        Slider_Volume.onValueChanged.AddListener((value) =>
        {
            DataManager.Instance.BGM_Volume = value;
            AudioManager.Instance.SwitchVolume(value);
        });
        //改变音效
        Slider_Sound.onValueChanged.AddListener((value) =>
        {
            DataManager.Instance.Sound_Volume = value;
        });

        //给滑动条初始化
        Slider_Volume.value = DataManager.Instance.BGM_Volume;
        Slider_Sound.value = DataManager.Instance.Sound_Volume;
    }
}
