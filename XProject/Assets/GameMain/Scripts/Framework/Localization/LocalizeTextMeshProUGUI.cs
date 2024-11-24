using System;
using UnityEngine;
using TMPro;
using System.Collections;

namespace Localizetion
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizeTextMeshProUGUI : AbstractLocalize
    {
        [SerializeField] private string m_Matrial;

        private TextMeshProUGUI m_TmpText;

        public string M_Matrial => m_Matrial;
        public TextMeshProUGUI TmpText
        {
            get => m_TmpText;
            set => m_TmpText = value;
        }

        private void Awake()
        {
            m_TmpText = transform.GetComponent<TextMeshProUGUI>();
        }

        protected override void Localize()
        {
            SetText();
        }

        public TextMeshProUGUI GetTmpText()
        {
            return m_TmpText;
        }

        private void SetText()
        {
            if (string.IsNullOrEmpty(Term)) return;

            string translation = LocalizationManager.Instance.GetLocalizedString(Term);
            if (!String.IsNullOrEmpty(translation))
            {
                SetText(translation);
            }
            else
            {
                SetText(Term);
                Debug.Log(transform.name + " ### LocalizeTextMeshProUGUI Term error: " + Term + " ###");
            }
        }

        public void SetText(string str)
        {
            if (str == null)
                return;

            if (m_TmpText == null)
                m_TmpText = transform.GetComponent<TextMeshProUGUI>();

            if (m_TmpText != null)
            {
                if (str != m_TmpText.text) m_TmpText.SetText(str);
                SetFont();
                CurrLocalize = LocalizationManager.Instance.GetCurrentLocale();
            }
            else
                Debug.Log("### LocalizeTextMeshProUGUI Component error: " + str + " ###");
        }

        public string GetText()
        {
            if (m_TmpText == null)
                Awake();
            if (m_TmpText != null)
                return m_TmpText.text;
            else
            {
                Debug.Log("### LocalizeTextMeshProUGUI Component error: " + gameObject.name + " ###");
                return string.Empty;
            }
        }

        public void SetColor(Color color)
        {
            if (m_TmpText == null)
                Awake();
            if (m_TmpText != null)
                m_TmpText.color = color;
            else
                Debug.Log("### LocalizeTextMeshProUGUI Component error: " + gameObject.name + " ###");
        }

        public Color GetColor()
        {
            if (m_TmpText == null)
                Awake();
            if (m_TmpText != null)
                return m_TmpText.color;

            return Color.white;
        }

        public void HideAlpha()
        {
            var c = GetColor();
            c.a = 0;
            SetColor(c);
        }

        public void ShowAlpha()
        {
            var c = GetColor();
            c.a = 1;
            SetColor(c);
        }

        public void SetEffectColor(Color color)
        {
            //GetComponent<TextMeshProUGUI>().e = color;
        }

        public void DoFade(float endvalue, float duration, Action cb = null)
        {
            StartCoroutine(dofade(endvalue, duration, cb));
        }

        private IEnumerator dofade(float endvalue, float time, Action cb)
        {
            int count = (int) (30 * time); // 1秒变化30次
            float timeDelta = time / count;
            float valueDelta = (float) (endvalue - m_TmpText.color.a) / count;
            var c = GetColor();
            float startvalue = endvalue > 0 ? 0 : 1f;
            for (int i = 0; i <= count; i++)
            {
                if (i == count)
                {
                    c.a = endvalue;
                    SetColor(c);
                }
                else
                {
                    c.a = startvalue + i * valueDelta;
                    SetColor(c);
                }

                //yield return new WaitForSeconds(timeDelta);
                yield return new WaitForEndOfFrame();
            }

            cb?.Invoke();
        }

        /// <summary>
        /// 设置字体
        /// </summary>
        /// <param name="language"></param>
        public void SetFont(string language)
        {
            SetFont(LocalizationManager.Instance.GetFont(language));
            if (!string.IsNullOrEmpty(m_Matrial) && !m_Matrial.Equals("Material"))
            {
                SetMaterial(LocalizationManager.Instance.GetMaterial(language, m_Matrial));
            }
        }

        /// <summary>
        /// 设置字体
        /// </summary>
        /// <param name="font"></param>
        private void SetFont(TMP_FontAsset font)
        {
            if (font == null) return;
            if (m_TmpText.font == font) return;
            m_TmpText.font = font;
        }

        /// <summary>
        /// 设置材质
        /// </summary>
        /// <param name="material"></param>
        private void SetMaterial(Material material)
        {
            if (material != null)
            {
                m_TmpText.fontMaterial = material;
            }
            else
            {
                Debug.Log(transform.name + "  ######  " + m_Matrial);
            }
        }

        private void SetFont()
        {
            if (!String.IsNullOrEmpty(m_Matrial))
            {
                LocalizeFont();
            }

            LocalizeMaterial();
        }

        private void LocalizeFont()
        {
            SetFont(LocalizationManager.Instance.GetLocaleFont());
        }

        private void LocalizeMaterial()
        {
            if (!string.IsNullOrEmpty(m_Matrial) && !m_Matrial.Equals("Material"))
            {
                SetMaterial(LocalizationManager.Instance.GetLocaleMaterial(m_Matrial));
            }
        }
    }
}