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
        //�����ʼ��
        Btn_Continue = GetControl<Button>("Btn_Continue");
        Btn_Instruction = GetControl<Button>("Btn_Instruction");
        Btn_Exit = GetControl<Button>("Btn_Exit");
        Slider_Volume = GetControl<Slider>("Slider_Volume");
        Slider_Sound = GetControl<Slider>("Slider_Sound");

        //������Ϸ
        Btn_Continue.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("SettingPanel");
        });

        //�������
        Btn_Instruction.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<InstructionPanel>("InstructionPanel");
            UIManager.Instance.HidePanel("SettingPanel");
        });
        //�˳���Ϸ
        Btn_Exit.onClick.AddListener(() =>
        {
            //�˳���Ϸ
            print("�˳���Ϸ");
            Application.Quit();
        });
        //�϶��������ı�����
        Slider_Volume.onValueChanged.AddListener((value) =>
        {
            DataManager.Instance.BGM_Volume = value;
            AudioManager.Instance.SwitchVolume(value);
        });
        //�ı���Ч
        Slider_Sound.onValueChanged.AddListener((value) =>
        {
            DataManager.Instance.Sound_Volume = value;
        });

        //����������ʼ��
        Slider_Volume.value = DataManager.Instance.BGM_Volume;
        Slider_Sound.value = DataManager.Instance.Sound_Volume;
    }
}
