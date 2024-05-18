namespace UIFlow.Examples.Example1
{
    using System.Collections;

    using UnityEngine;

    public class SettingsViewController : ViewController
    {
        public override void OnPresentTransition()
        {
            StartCoroutine(AppearAnimation());
        }

        private IEnumerator AppearAnimation()
        {
            CanvasGroup.alpha = 0;
            Content.anchoredPosition = new Vector2(0, -Content.rect.height);

            float time = 0;
            while(time < Transition.Appear)
            {
                float t = time / Transition.Appear;

                {
                    CanvasGroup.alpha = Mathf.Lerp(0, 1, t);
                }

                {
                    Content.anchoredPosition = new Vector2(0, Mathf.Lerp(-Content.rect.height, 0, t));
                }

                time += Time.deltaTime;
                yield return null;
            }
        }

        public override void OnDismissTransition()
        {
            StartCoroutine(DisappearAnimation());
        }

        private IEnumerator DisappearAnimation()
        {
            float time = 0;
            while (time < Transition.Appear)
            {
                float t = time / Transition.Appear;

                {
                    CanvasGroup.alpha = Mathf.Lerp(1, 0, t);
                }

                {
                    Content.anchoredPosition = new Vector2(0, Mathf.Lerp(0, -Content.rect.height, t));
                }

                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}