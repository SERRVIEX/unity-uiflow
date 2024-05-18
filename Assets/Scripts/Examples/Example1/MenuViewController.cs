namespace UIFlow.Examples.Example1
{
    using System.Collections;

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public class MenuViewController : ViewController
    {
        [SerializeField] private SettingsViewController _settingsViewController;
        [SerializeField] private PlayViewController _playViewController;
        [SerializeField] private Sprite _icon1;
        [SerializeField] private Sprite _icon2;
        [SerializeField] private Sprite _icon3;
        [SerializeField] private Sprite _icon4;

        [SerializeField] private Image _fade;

        // Methods

        public void Settings()
        {
            Storyboard.Present(_settingsViewController);
        }

        public void Notification()
        {
            ToastViewController.Present("notification_0", "You can show notifications through toast!");
        }

        public void Gift()
        {
            AlertViewController controller = AlertViewController.Present();

            AlertLabelButtonData receive = new AlertLabelButtonData();
            receive.SetLabel("Receive");
            receive.SetLabelColor(Color.yellow);
            receive.SetOnClick(() =>
            {
                ToastViewController.Present("gift", "Gift received!");
                controller.Dismiss();
            });

            AlertLabelButtonData cancel = new AlertLabelButtonData();
            cancel.SetLabel("Cancel");
            cancel.SetOnClick(() =>
            {
                controller.Dismiss();
            });

            controller.UpdateContent("Gift!!!", true, receive, cancel);
        }

        public void Alert()
        {
            AlertViewController controller = AlertViewController.Present();

            AlertLabelButtonData button1 = new AlertLabelButtonData();
            button1.SetLabel("Label 1");
            button1.SetLabelColor(Color.yellow);
            button1.SetOnClick(() =>
            {
                controller.Dismiss();
            });

            AlertIconButtonData button2 = new AlertIconButtonData();
            button2.SetIcon(_icon1);
            button2.SetOnClick(() =>
            {
                controller.Dismiss();
            });

            AlertComplexButtonData button3 = new AlertComplexButtonData();
            button3.SetLeftIcon(_icon2);
            button3.SetLeftIcon(_icon3);
            button3.SetOnClick(() =>
            {
                controller.Dismiss();
            });

            AlertDefaultButtonData button4 = new AlertDefaultButtonData();
            button4.SetLabel("Label 4");
            button4.SetIcon(_icon4);
            button4.SetOnClick(() =>
            {
                controller.Dismiss();
            });

            controller.UpdateContent("Custom Alert", false, button1, button2, button3, button4);
        }

        public void Play()
        {
            Dismiss();
            OnDidDisappearHandler.AddListener(() =>
            {
                Storyboard.Present(_playViewController);
                SceneManager.LoadScene("Example1_2");
            });
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
                    _fade.color = Color.Lerp(_fade.color, Color.clear, t);
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
                    _fade.color = Color.Lerp(_fade.color, Color.black, t);
                }

                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}