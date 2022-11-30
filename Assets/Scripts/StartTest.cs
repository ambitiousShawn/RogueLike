using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTest : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.ShowPanel<MainPanel>("MainPanel");
        AudioManager.Instance.PlayBGM("���³�");
    }

    private void Update()
    {
        //�����������
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance.GetPanel<InstructionPanel>("InstructionPanel") != null)
            {
                UIManager.Instance.HidePanel("InstructionPanel");
                UIManager.Instance.ShowPanel<SettingPanel>("SettingPanel");
            }
            else if (UIManager.Instance.GetPanel<SettingPanel>("SettingPanel") != null)
            {
                UIManager.Instance.HidePanel("SettingPanel");
                UIManager.Instance.ShowPanel<MainPanel>("MainPanel");
            }  
        }
    }
}
