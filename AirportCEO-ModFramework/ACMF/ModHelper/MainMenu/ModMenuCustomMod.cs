using ACMF.ModLoader;
using UnityEngine;
using UnityEngine.UI;

namespace ACMF.ModHelper.MainMenu
{
    public class ModMenuCustomMod : MonoBehaviour
    {
        private Mod mod = null;
        private Text headerText = null;
        private Text description = null;
        private Button buttonPlay = null;
        private Button buttonUnsubscribe = null;

        public void Awake()
        {
            headerText = transform.Find("HeaderText")?.gameObject?.GetComponent<Text>();
            description = transform.Find("Description")?.gameObject?.GetComponent<Text>();
            buttonPlay = transform.Find("ButtonPlay")?.gameObject?.gameObject?.GetComponent<Button>();
            buttonUnsubscribe = transform.Find("ButtonUnsubscribe")?.gameObject?.GetComponent<Button>();

            GetComponent<Image>().color = new Color(0.08f, 0.35f, 0.44f);
        }

        public void SetMod(Mod mod)
        {
            this.mod = mod;
            headerText.text = mod.ModInfo.Name + " (ACML Mod)";
            description.text = "ACML mods do not have descriptions yet.";
        }
    }
}
