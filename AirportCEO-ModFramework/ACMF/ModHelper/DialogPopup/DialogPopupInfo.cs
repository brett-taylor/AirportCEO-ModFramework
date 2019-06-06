using System;

namespace ACMF.ModHelper.DialogPopup
{
    public struct DialogPopupInfo
    {
        public bool PrintToLog;
        public bool IsQuestion;
        public string Message;
        public Action<bool> QuestionAction;
        public Action MessageAction;
        public bool ShowCloseButton;
    }
}
