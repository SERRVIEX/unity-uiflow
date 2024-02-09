namespace UIFlow.Examples.Example1
{
    using System.Collections;

    using UnityEngine;
    using UnityEngine.UI;

    public class StartupViewController : ViewController
    {
        [SerializeField] private MenuViewController _menuViewController;
        [SerializeField] private Image _icon;
        [SerializeField] private Text _text;

        // Methods

        protected override void Start()
        {
            base.Start();

            StartCoroutine(Load());
        }

        public IEnumerator Load()
        {
            _text.text = "Loading.. please wait...";

            yield return new WaitForSeconds(1f);

            _text.text = "Almost done...";

            yield return new WaitForSeconds(1f);

            Storyboard.Present(_menuViewController);
            Dismiss();
        }
    }
}