using System;
using TMPro;
using UnityEngine;

namespace nmxi.easylogview
{
    public class EasyLogView : MonoBehaviour
    {
        [SerializeField] private static TextMeshProUGUI m_textUI;

        private void Awake()
        {
            m_textUI = GetComponent<TextMeshProUGUI>();
            Application.logMessageReceived += OnLogMessage;

            m_textUI.text = String.Empty;
        }

        private void Update()
        {
            if (m_textUI.text.Length > 1500)
            {
                m_textUI.text = String.Empty;
            }
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= OnLogMessage;
        }

        public static void AddLog(string log)
        {
            m_textUI.text += log;
        }

        private void OnLogMessage(string i_logText, string i_stackTrace, LogType type)
        {
            var tmp = m_textUI.text;
            m_textUI.text = "[" + DateTime.Now.ToLongTimeString() + "] ";
            switch (type)
            {
                case LogType.Warning:
                    m_textUI.text += "<color=#ffff00>" + i_logText + "</color>" + Environment.NewLine;
                    break;
                case LogType.Error:
                    m_textUI.text += "<color=#ff0000>" + i_logText + "</color>" + Environment.NewLine;
                    break;
                default:
                    m_textUI.text += i_logText + Environment.NewLine;
                    break;
            }

            m_textUI.text += tmp;
        }

        public void ResetLog()
        {
            m_textUI.text = "[" + DateTime.Now.ToLongTimeString() + "] Clear log";
        }
    }
}