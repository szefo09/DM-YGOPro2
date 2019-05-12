﻿using UnityEngine;
using System;
public class Setting : WindowServant2D
{
    private EventDelegate onChange;

    public LAZYsetting setting;
    public UIToggle isBGMMute;
    public bool batterySaving;
    public bool autoPicDownload;
    public bool autoDeckUpdate;

    public override void initialize()
    {
        gameObject = createWindow(this, Program.I().new_ui_setting);
        setting = gameObject.GetComponentInChildren<LAZYsetting>();
        UIHelper.registEvent(gameObject, "exit_", onClickExit);
        UIHelper.registEvent(gameObject, "screen_", resizeScreen);
        UIHelper.registEvent(gameObject, "full_", resizeScreen);
        UIHelper.registEvent(gameObject, "resize_", resizeScreen);
        UIHelper.registEvent(gameObject, "resize_", resizeScreen);
        UIHelper.registEvent(gameObject, "batterySaving", batterySavingMode);
        isBGMMute = UIHelper.getByName<UIToggle>(gameObject, "muteBGM");
        UIHelper.getByName<UIToggle>(gameObject, "muteBGM").value = UIHelper.fromStringToBool(Config.Get("muteBGMAudio", "0"));
        batterySaving = UIHelper.getByName<UIToggle>(gameObject, "batterySaving").value = UIHelper.fromStringToBool(Config.Get("batterySaving", "0"));
        UIHelper.getByName<UIToggle>(gameObject, "full_").value = Screen.fullScreen;
        autoPicDownload = UIHelper.fromStringToBool(Config.Get("autoPicDownload_", "1"));
        GameTextureManager.AutoPicDownload = autoPicDownload;
        autoDeckUpdate = UIHelper.fromStringToBool(Config.Get("autoDeckUpdate_", "1"));
        UIHelper.getByName<UIPopupList>(gameObject, "screen_").value = Config.Get("resolution_",
#if UNITY_ANDROID || UNITY_IOS
            "1280*720" //Gives people the freedom to change their resolution on mobile.
#else
            Screen.width.ToString() + "*" + Screen.height.ToString()
#endif
            );
        UIHelper.getByName<UIToggle>(gameObject, "ignoreWatcher_").value = UIHelper.fromStringToBool(Config.Get("ignoreWatcher_", "0"));
        UIHelper.getByName<UIToggle>(gameObject, "ignoreOP_").value = UIHelper.fromStringToBool(Config.Get("ignoreOP_", "0"));
        UIHelper.getByName<UIToggle>(gameObject, "smartSelect_").value = UIHelper.fromStringToBool(Config.Get("smartSelect_", "1"));
        UIHelper.getByName<UIToggle>(gameObject, "autoChain_").value = UIHelper.fromStringToBool(Config.Get("autoChain_", "1"));
        UIHelper.getByName<UIToggle>(gameObject, "handPosition_").value = UIHelper.fromStringToBool(Config.Get("handPosition_", "1"));
        UIHelper.getByName<UIToggle>(gameObject, "handmPosition_").value = false;
        UIHelper.getByName<UIToggle>(gameObject, "spyer_").value = UIHelper.fromStringToBool(Config.Get("spyer_", "1"));
        UIHelper.getByName<UIToggle>(gameObject, "resize_").value = UIHelper.fromStringToBool(Config.Get("resize_", "1"));
        UIHelper.registEvent(gameObject, "muteBGM", muteBGM);
        UIHelper.registEvent(gameObject, "vol_", onVolChange);
        if (QualitySettings.GetQualityLevel() < 3)
        {
            UIHelper.getByName<UIToggle>(gameObject, "high_").value = false;
        }
        else
        {
            UIHelper.getByName<UIToggle>(gameObject, "high_").value = true;
        }
        UIHelper.registEvent(gameObject, "ignoreWatcher_", save);
        UIHelper.registEvent(gameObject, "ignoreOP_", save);
        UIHelper.registEvent(gameObject, "smartSelect_", save);
        UIHelper.registEvent(gameObject, "autoChain_", save);
        UIHelper.registEvent(gameObject, "handPosition_", save);
        UIHelper.registEvent(gameObject, "handmPosition_", save);
        UIHelper.registEvent(gameObject, "spyer_", save);
        UIHelper.registEvent(gameObject, "high_", save);
        UIHelper.registEvent(gameObject, "size_", onChangeSize);
        UIHelper.registEvent(gameObject, "alpha_", onChangeAlpha);
        UIHelper.registEvent(gameObject, "vSize_", onChangeVsize);
        sliderSize = UIHelper.getByName<UISlider>(gameObject, "size_");
        sliderAlpha = UIHelper.getByName<UISlider>(gameObject, "alpha_");
        sliderVsize = UIHelper.getByName<UISlider>(gameObject, "vSize_");
        Program.go(2000, readVales);
        var collection = gameObject.GetComponentsInChildren<UIToggle>();
        for (int i = 0; i < collection.Length; i++)
        {
            if (collection[i].name.Length > 0 && collection[i].name[0] == '*')
            {
                if (collection[i].name == "*mouseParticle" || collection[i].name == "*showOff" || collection[i].name == "*Efield")
                {
                    collection[i].value = UIHelper.fromStringToBool(Config.Get(collection[i].name, "1"));
                }
                else
                {
                    collection[i].value = UIHelper.fromStringToBool(Config.Get(collection[i].name, "0"));
                }
            }
        }
        setting.showoffATK.value = Config.Get("showoffATK", "1800");
        setting.showoffStar.value = Config.Get("showoffStar", "5");
        UIHelper.registEvent(setting.showoffATK.gameObject, onchangeClose);
        UIHelper.registEvent(setting.showoffStar.gameObject, onchangeClose);
        UIHelper.registEvent(setting.mouseEffect.gameObject, onchangeMouse);
        UIHelper.registEvent(setting.closeUp.gameObject, onchangeCloseUp);
        UIHelper.registEvent(setting.cloud.gameObject, onchangeCloud);
        UIHelper.registEvent(setting.Vpedium.gameObject, onCP);
        UIHelper.registEvent(setting.Vfield.gameObject, onCP);
        UIHelper.registEvent(setting.Vlink.gameObject, onCP);
        onchangeMouse();
        onchangeCloud();
    }

    private void batterySavingMode()
    {
        if (batterySaving)
        {
            Application.targetFrameRate = 30;
        }
        else
        {
            Application.targetFrameRate = 144;
        }
        batterySaving = !batterySaving;
        save();
    }

    private void muteBGM()
    {
        if (!isBGMMute.value)
        {
            if (Program.I().bgm != null)
            {
                Program.I().bgm.Start();
            }
        }
        else
        {
            if (Program.I().bgm != null && Program.I().bgm.audioSource != null)
            {
                Program.I().bgm.audioSource.Stop();
            }

        }
        save();
    }

    private void onVolChange()
    {
        try
        {
            Program.I().bgm.changeBGMVol(UIHelper.getByName<UISlider>(gameObject, "vol_").value);
        }
        catch { }
    }

    private void readVales()
    {
        try
        {
            setting.sliderVolum.forceValue(((float)(int.Parse(Config.Get("vol_", "750")))) / 1000f);
            setting.sliderSize.forceValue(((float)(int.Parse(Config.Get("size_", "500")))) / 1000f);
            setting.sliderSizeDrawing.forceValue(((float)(int.Parse(Config.Get("vSize_", "500")))) / 1000f);
            setting.sliderAlpha.forceValue(((float)(int.Parse(Config.Get("alpha_", "666")))) / 1000f);
            onChangeAlpha();
            onChangeSize();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void onchangeCloud()
    {
        Program.MonsterCloud = setting.cloud.value;
    }

    public void onchangeMouse()
    {
        Program.I().mouseParticle.SetActive(setting.mouseEffect.value);
    }

    void onCP()
    {
        try
        {
            Program.I().ocgcore.realize(true);
        }
        catch
        {
        }
    }


    public void onchangeCloseUp()
    {
        if (setting.closeUp.value == false)
        {
            setting.sliderAlpha.forceValue(0);
            setting.sliderSize.forceValue(0);
        }
        else
        {
            setting.sliderAlpha.forceValue(0.6666f);
            setting.sliderSize.forceValue(1f);
        }
        onChangeSize();
        onChangeAlpha();
    }

    public int atk = 1800;
    public int star = 5;

    void onchangeClose()
    {
        atk = 1800;
        star = 5;
        try
        {
            atk = int.Parse(setting.showoffATK.value);
        }
        catch (Exception)
        {

        }
        try
        {
            star = int.Parse(setting.showoffStar.value);
        }
        catch (Exception)
        {

        }
    }


    UISlider sliderAlpha;
    void onChangeAlpha()
    {
        if (sliderAlpha != null)
        {
            Program.transparency = 1.5f * sliderAlpha.value;
        }
    }


    UISlider sliderVsize;
    void onChangeVsize()
    {
        if (sliderVsize != null)
        {
            Program.verticleScale = 4f + 2f * sliderVsize.value;
        }
    }

    UISlider sliderSize;
    void onChangeSize()
    {
        if (sliderSize != null)
        {
            Program.fieldSize = 1f + sliderSize.value * 0.21f;
        }
    }

    public float vol()
    {
        return UIHelper.getByName<UISlider>(gameObject, "vol_").value;
    }

    public override void preFrameFunction()
    {
        base.preFrameFunction();
    }

    void onClickExit()
    {
        Program.I().SaveConfig();
        hide();
    }

    void resizeScreen()
    {
        string[] mats = UIHelper.getByName<UIPopupList>(gameObject, "screen_").value.Split(new string[] { "*" }, StringSplitOptions.RemoveEmptyEntries);
        if (mats.Length == 2)
        {
            Screen.SetResolution(int.Parse(mats[0]), int.Parse(mats[1]), UIHelper.getByName<UIToggle>(gameObject, "full_").value);
            Config.Set("resolution_", UIHelper.getByName<UIPopupList>(gameObject, "screen_").value);
        }
        Program.go(100, () => { Program.I().fixScreenProblems(); });
    }

    public void saveWhenQuit()
    {

        Config.Set("vol_", ((int)(UIHelper.getByName<UISlider>(gameObject, "vol_").value * 1000)).ToString());
        Config.Set("size_", ((int)(UIHelper.getByName<UISlider>(gameObject, "size_").value * 1000)).ToString());
        Config.Set("vSize_", ((int)(UIHelper.getByName<UISlider>(gameObject, "vSize_").value * 1000)).ToString());
        Config.Set("alpha_", ((int)(UIHelper.getByName<UISlider>(gameObject, "alpha_").value * 1000)).ToString());
        var collection = gameObject.GetComponentsInChildren<UIToggle>();
        for (int i = 0; i < collection.Length; i++)
        {
            if (collection[i].name.Length > 0 && collection[i].name[0] == '*')
            {
                Config.Set(collection[i].name, UIHelper.fromBoolToString(collection[i].value));
            }
        }
        Config.Set("showoffATK", setting.showoffATK.value.ToString());
        Config.Set("muteBGMAudio", UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "muteBGM").value));
        Config.Set("showoffStar", setting.showoffStar.value.ToString());
        Config.Set("resize_", UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "resize_").value));
    }

    public void save()
    {
        UIHelper.getByName<UIToggle>(gameObject, "handmPosition_").value = false;
        Config.Set("batterySaving", UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "batterySaving").value));
        Config.Set("ignoreWatcher_", UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "ignoreWatcher_").value));
        Config.Set("ignoreOP_", UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "ignoreOP_").value));
        Config.Set("muteBGMAudio", UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "muteBGM").value));
        Config.Set("smartSelect_", UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "smartSelect_").value));
        Config.Set("autoChain_", UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "autoChain_").value));
        Config.Set("handPosition_", UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "handPosition_").value));
        Config.Set("handmPosition_", UIHelper.fromBoolToString(false));
        
        Config.Set("spyer_", UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "spyer_").value));
        if (UIHelper.getByName<UIToggle>(gameObject, "high_").value)
        {
            QualitySettings.SetQualityLevel(5);
        }
        else
        {
            QualitySettings.SetQualityLevel(0);
        }
    }

    public float soundValue()
    {
        return UIHelper.getByName<UISlider>(gameObject, "vol_").value;
    }
}
