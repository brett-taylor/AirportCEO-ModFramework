using System;
using System.Collections;
using System.Collections.Generic;

namespace ACMF.ModHelper.DialogPopup
{
    public static class DialogManager
    {
        private static Queue<DialogPopupInfo> ToShow = new Queue<DialogPopupInfo>();

        public static void QueueMessagePanel(string message, Action action = null)
        {
            ToShow.Enqueue(new DialogPopupInfo()
            {
                IsQuestion = false,
                Message = message,
                QuestionAction = null,
                MessageAction = action,
                ShowCloseButton = false
            });

            if (DialogPanel.Instance.gameObject.activeSelf == false)
                ShowNext();
        }

        public static void QueueQuestionPanel(string message, Action<bool> action, bool showCloseButton = false)
        {
            ToShow.Enqueue(new DialogPopupInfo()
            {
                IsQuestion = true,
                Message = message,
                QuestionAction = action,
                MessageAction = null,
                ShowCloseButton = showCloseButton
            });

            if (DialogPanel.Instance.gameObject.activeSelf == false)
                ShowNext();
        }

        public static void ShowNext()
        {
            if (ToShow.Count == 0 || DialogPanel.Instance == null)
                return;

            PlayerInputController.Instance.StartCoroutine(DelayShow(ToShow.Dequeue()));
        }

        private static IEnumerator DelayShow(DialogPopupInfo info)
        {
            yield return Utils.veryShortWait;

            if (info.IsQuestion)
                DialogPanel.Instance.ShowQuestionPanel(info.QuestionAction, info.Message, false);
            else
            {
                if (info.MessageAction == null)
                    DialogPanel.Instance.ShowMessagePanel(info.Message);
                else
                    DialogPanel.Instance.ShowMessagePanel(info.Message);
            }
        }
    }
}
