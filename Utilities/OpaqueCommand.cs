using FX.MainForms.Public.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FX.MainForms.Public.Utilities
{
    class OpaqueCommand
    {
        private static OpaqueLayer m_OpaqueLayer = null;//半透明蒙板层

        /// <summary>
        /// 显示遮罩层
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="alpha">透明度</param>
        /// <param name="isShowLoadingImage">是否显示图标</param>
        public static void ShowOpaqueLayer(Form control, int alpha, bool isShowLoadingImage)
        {
            try
            {
                if (m_OpaqueLayer == null)
                {
                    m_OpaqueLayer = new OpaqueLayer(alpha, isShowLoadingImage);
                    control.Controls.Add(m_OpaqueLayer);
                    m_OpaqueLayer.Dock = DockStyle.Fill;
                    m_OpaqueLayer.BringToFront();
                }
                m_OpaqueLayer.Enabled = true;
                m_OpaqueLayer.Visible = true;
            }
            catch { }
        }

        /// <summary>
        /// 隐藏遮罩层
        /// </summary>
        public static void HideOpaqueLayer()
        {
            try
            {
                if (m_OpaqueLayer != null)
                {
                    m_OpaqueLayer.Visible = false;
                    m_OpaqueLayer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                //MessageBox.Show(ex.Message);
            }
        }
    }
}
