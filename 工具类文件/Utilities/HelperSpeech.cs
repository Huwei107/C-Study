//-----------------------------------------------------------------------
// <copyright company="工品一号" file="HelperSpeech.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   王健
//  创建时间:   2023/7/5 16:17:07 
//  功能描述:   
//  历史版本:
//          2023/7/5 16:17:07 王健 创建HelperSpeech类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;

namespace FX.MainForms
{
    /// <summary>
    /// 语音工具类
    /// </summary>
    public class HelperSpeech
    {
        /// <summary>
        /// 发声
        /// </summary>
        /// <param name="text"></param>
        public static void Speech(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            try
            {

                SpeechSynthesizer synth = new SpeechSynthesizer();
                PromptBuilder talk = new PromptBuilder();
                //talk.AppendText(text);
                talk.AppendTextWithHint(text, SayAs.SpellOut);
                talk.Culture = CultureInfo.CreateSpecificCulture("zh-CN");
                synth.SpeakAsync(talk);

            }
            catch (Exception ex) { }

        }
    }
}
