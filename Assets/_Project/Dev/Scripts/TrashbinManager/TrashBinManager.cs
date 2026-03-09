using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Trash.Manager;
using TMPro;
using UnityEngine;

namespace Game.TrashBin
{
    public class TrashBinManager : MonoBehaviour
    {
        public enum e_DustType
        {
            Paper,
            Plastic,
            Glass
        }

        [System.Serializable]
        public class DustbinObject
        {
            public e_DustType dustType;
            public Core_MiniTriggerHelper dustbinTrigger;
            [Header("UI")] public TMP_Text scoreText;
            public int ScoreValue = 0;

            public void SetDefaultValue()
            {
                scoreText.text = ScoreValue.ToString();
            }
        }

        [SerializeField] DustbinObject m_dustbinObject;

        private void Start()
        {
            m_dustbinObject.SetDefaultValue();
        }

        private void OnEnable()
        {
            m_dustbinObject.dustbinTrigger.onTriggerEnter +=
                OnDustbinTriggerEnter;
            m_dustbinObject.dustbinTrigger.onTriggerExit +=
                OnDustbinTriggerExit;
        }

        private void OnDisable()
        {
            m_dustbinObject.dustbinTrigger.onTriggerEnter -=
                OnDustbinTriggerEnter;
            m_dustbinObject.dustbinTrigger.onTriggerExit -=
                OnDustbinTriggerExit;
        }


        private void OnDustbinTriggerEnter(Collider dustManager)
        {
            TrashManager trash = dustManager.GetComponentInParent<TrashManager>();
            if (trash != null)
            {
                bool isCorrect = trash.dustType == m_dustbinObject.dustType;
                trash.CollectTrash(isCorrect);
            }
        }

        private void OnDustbinTriggerExit(Collider dustManager)
        {
            TrashManager trash = dustManager.GetComponentInParent<TrashManager>();
            if (trash != null)
            {
                trash.CancelDispose();
            }
        }

        #region Score

        internal void UpdateScore()
        {
            m_dustbinObject.ScoreValue += 1;
            m_dustbinObject.scoreText.text = m_dustbinObject.ScoreValue.ToString();
            m_dustbinObject.scoreText.transform.DOComplete(false);
            m_dustbinObject.scoreText.transform.DOScale(1.2f, 0.2f).onComplete += () =>
            {
                m_dustbinObject.scoreText.transform.DOScale(1f, 0.2f);
            };
        }

        #endregion
    }
}