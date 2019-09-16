using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Resource;
using GameFramework.UI;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    public static class UIExtension
    {

        public static IEnumerator FadeToAlpha(this GComponent canvasGroup, float alpha, float duration)
        {
            float time = 0f;
            float originalAlpha = canvasGroup.alpha;
            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            canvasGroup.alpha = alpha;
        }

        public static IEnumerator SmoothValue(this GSlider slider, float value, float duration)
        {
            float time = 0f;
            float originalValue = (float) slider.value;
            while (time < duration)
            {
                time += Time.deltaTime;
                slider.value = Mathf.Lerp(originalValue, value, time / duration);
                yield return new WaitForEndOfFrame();
            }

            slider.value = value;
        }

        public static bool HasUIForm(this UIComponent uiComponent, UIFormId uiFormId, string uiGroupName = null)
        {
            return uiComponent.HasUIForm((int) uiFormId, uiGroupName);
        }

        public static bool HasUIForm(this UIComponent uiComponent, int uiFormId, string uiGroupName = null)
        {
            IDataTable<DRUIForm> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
            if (dtUIForm == null)
            {
                return false;
            }
            DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                return false;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            if (string.IsNullOrEmpty(uiGroupName))
            {
                return uiComponent.HasUIForm(assetName);
            }

            IUIGroup uiGroup = uiComponent.GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                return false;
            }

            return uiGroup.HasUIForm(assetName);
        }

        public static UIForm GetUIForm(this UIComponent uiComponent, UIFormId uiFormId, string uiGroupName = null)
        {
            return uiComponent.GetUIForm((int) uiFormId, uiGroupName);
        }

        public static UIForm GetUIForm(this UIComponent uiComponent, int uiFormId, string uiGroupName = null)
        {
            IDataTable<DRUIForm> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
            DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                return null;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            UIForm uiForm = null;
            if (string.IsNullOrEmpty(uiGroupName))
            {
                uiForm = uiComponent.GetUIForm(assetName);
                if (uiForm == null)
                {
                    return null;
                }

                return uiForm;
            }

            IUIGroup uiGroup = uiComponent.GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                return null;
            }

            uiForm = (UIForm) uiGroup.GetUIForm(assetName);
            if (uiForm == null)
            {
                return null;
            }

            return uiForm;
        }

        public static void CloseUIForm(this UIComponent uiComponent, FGUIForm uiForm)
        {
            uiComponent.CloseUIForm(uiForm.UIForm);
        }

        public static void CloseUIForm(this UIComponent uiComponent, UIFormId uiFormId)
        {
            if (GameEntry.UI.HasUIForm(uiFormId))
            {
                uiComponent.CloseUIForm(uiComponent.GetUIForm(uiFormId));
                return;
            }
            else
            {
                return;
            }
        }

        public static int? OpenUIForm(this UIComponent uiComponent, UIFormId uiFormId, object userData = null)
        {
            if (GameEntry.UI.HasUIForm(uiFormId))
            {
                var form = GameEntry.UI.GetUIForm(uiFormId).Logic as FGUIForm;
                form.ForceRefresh(userData);
                return 0;
            }
            int uiFormIdInt = (int) uiFormId;
            IDataTable<DRUIForm> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
            DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormIdInt);
            GameEntry.FGUIData.LoadFGuiModel(drUIForm.FGuiName, null, (name, k) =>
            {
                uiComponent.OpenUIForm(uiFormIdInt, userData);
            });
            return 0;
        }

        public static int? OpenUIForm(this UIComponent uiComponent, int uiFormId, object userData = null)
        {
            IDataTable<DRUIForm> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
            DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return null;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            if (!drUIForm.AllowMultiInstance)
            {
                if (uiComponent.IsLoadingUIForm(assetName))
                {
                    return null;
                }

                if (uiComponent.HasUIForm(assetName))
                {
                    return null;
                }
            }

            return uiComponent.OpenUIForm(assetName, drUIForm.UIGroupName, Constant.AssetPriority.UIFormAsset, drUIForm.PauseCoveredUIForm, userData);
        }

        public static void OpenDialog(this UIComponent uiComponent, DialogParams dialogParams)
        {
            if (((ProcedureBase) GameEntry.Procedure.CurrentProcedure).UseNativeDialog)
            {
                OpenNativeDialog(dialogParams);
            }
            else
            {
                uiComponent.OpenUIForm(UIFormId.DialogShowForm, dialogParams);
            }
        }

        private static void OpenNativeDialog(DialogParams dialogParams)
        {
            throw new System.NotImplementedException("OpenNativeDialog");
        }

        public static void CloseGroup(this UIComponent uiComponent, string uiGroupName)
        {
            var group = uiComponent.GetUIGroup(uiGroupName);
            if (group == null)
            {
                return;
            }
            var forms = group.GetAllUIForms();

            for (int i = forms.Length - 1; i > -1; --i)
            {
                var f = forms[i];
                uiComponent.CloseUIForm(f.SerialId);
            }
        }
    }
}