using System;
using System.Collections;
using DG.Tweening;
using Game.TrashBin;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Game.Trash.Manager
{
    public class TrashManager : MonoBehaviour
    {
        internal static Action<bool, TrashBinManager.e_DustType> onCollectTrash;
        [SerializeField] internal TrashBinManager.e_DustType dustType;
        [SerializeField] bool isDisposing;
        [SerializeField] private bool isCorrectDispose;
        [SerializeField] internal bool isDisposed;
        [SerializeField] private Vector3 initialPosition;
        [SerializeField] private Rigidbody rb;
        [Header("Colors")] [SerializeField] Color redColor, greenColor;
        [SerializeField] Outline outline;
        [SerializeField] private XRGrabInteractable grabIn;

        private void Start()
        {
            initialPosition = transform.position;
        }

        private void OnEnable()
        {
            grabIn.hoverEntered.AddListener((value) => { ShowHover(true); });
            grabIn.hoverExited.AddListener((value) => { ShowHover(false); });

            grabIn.selectExited.AddListener((value) => { OnDeactivated(); });
        }

        internal void CollectTrash(bool isCorrectDispose)
        {
            ChangeColor(isCorrectDispose);
            this.isCorrectDispose = isCorrectDispose;
            isDisposing = true;
        }

        void OnDeactivated()
        {
            if (isDisposing)
            {
                if (isCorrectDispose)
                {
                    StartCoroutine(DelayHide());

                    IEnumerator DelayHide()
                    {
                        yield return new WaitForSeconds(1f);
                        gameObject.SetActive(false);

                        isDisposed = true;
                        onCollectTrash?.Invoke(true, dustType);
                    }
                }
                else
                {
                    StartCoroutine(ResetPosition());
                }
            }
            else
            {
                StartCoroutine(ResetPosition());
            }
        }

        IEnumerator ResetPosition()
        {
            rb.isKinematic = true;
            outline.enabled = true;
            outline.OutlineColor = Color.red;

            yield return new WaitForSeconds(0.5f);
            rb.position = initialPosition;
            onCollectTrash?.Invoke(false, dustType);
            yield return new WaitForSeconds(0.5f);
            rb.isKinematic = false;
        }

        void ChangeColor(bool correct)
        {
            Color originalColor = Color.white;
            Color targetColor = correct ? greenColor : redColor;
            outline.OutlineColor = originalColor;
            DOTween.Kill("trashId");
            Sequence seq = DOTween.Sequence();
            seq.SetId("trashId");
            seq.OnStart(() => { outline.enabled = true; });
            seq.OnComplete(() => { outline.enabled = false; });
            seq.Append(DOTween.To(() => outline.OutlineColor, x => outline.OutlineColor = x, targetColor, 0.5f));
        }

        internal void CancelDispose()
        {
            isDisposing = false;
        }

        void ShowHover(bool show)
        {
            outline.OutlineColor = show ? Color.yellow : Color.white;
            outline.enabled = show;
        }
    }
}