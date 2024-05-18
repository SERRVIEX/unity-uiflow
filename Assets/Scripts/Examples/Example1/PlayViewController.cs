namespace UIFlow.Examples.Example1
{
    using System.Collections;

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public class PlayViewController : ViewController
    {
        [SerializeField] private MenuViewController _menuViewController;
        [SerializeField] private SettingsViewController _settingsViewController;

        [SerializeField] private Image _fade;

        // Methods

        public void Pause()
        {
            AlertViewController controller = AlertViewController.Present();

            AlertLabelButtonData resume = new AlertLabelButtonData();
            resume.SetLabel("Resume");
            resume.SetLabelColor(Color.yellow);
            resume.SetOnClick(() =>
            {
                controller.Dismiss();
            });

            AlertLabelButtonData settings = new AlertLabelButtonData();
            settings.SetLabel("Settings");
            settings.SetOnClick(() =>
            {
                //controller.Dismiss();
                Storyboard.Present(_settingsViewController);
            });

            AlertLabelButtonData quit = new AlertLabelButtonData();
            quit.SetLabel("Quit");
            quit.SetOnClick(() =>
            {
                controller.Dismiss();
                Dismiss();
               
                OnDidDisappearHandler.AddListener(() =>
                {
                    Storyboard.Present(_menuViewController);
                    SceneManager.LoadScene("Example1_1");
                });
            });

            controller.UpdateContent("Pause", false, resume, settings, quit);
        }

        public override void OnPresentTransition()
        {
            StartCoroutine(AppearAnimation());
        }

        private IEnumerator AppearAnimation()
        {
            _fade.gameObject.SetActive(true);
            float time = 0;
            while (time < Transition.Appear)
            {
                float t = time / Transition.Appear;

                {
                    _fade.color = Color.Lerp(Color.black, Color.clear, t);
                }

                time += Time.deltaTime;
                yield return null;
            }

            _fade.gameObject.SetActive(false);
        }

        public override void OnDismissTransition()
        {
            StartCoroutine(DisappearAnimation());
        }

        private IEnumerator DisappearAnimation()
        {
            _fade.gameObject.SetActive(true);
            float time = 0;
            while (time < Transition.Appear)
            {
                float t = time / Transition.Appear;

                {
                    _fade.color = Color.Lerp(Color.clear, Color.black, t);
                }

                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}