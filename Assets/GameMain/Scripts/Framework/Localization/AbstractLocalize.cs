using UnityEngine;

namespace Localizetion
{
    public abstract class AbstractLocalize : MonoBehaviour
    {
        [SerializeField] protected string Term;

        protected string CurrLocalize = string.Empty;

        protected abstract void Localize();

        private void OnEnable()
        {
            if (NeedChangeLocalize())
                Localize();

            //LocalizationManager.OnLocalization += Localize;
        }
        private void Start()
        {
            Localize();
        }

        public void SetTerm(string term)
        {
            this.Term = term;
            Localize();
        }

        public string GetTerm()
        {
            return this.Term;
        }

        private bool NeedChangeLocalize()
        {
            return CurrLocalize != LocalizationManager.Instance.GetCurrentLocale();
        }
    }
}