using System;
using System.Collections.Generic;
using System.Linq;
using Game.Trash.Manager;
using Game.TrashBin;
using TMPro;
using UnityEngine;

namespace Game.Manager
{
    public class GameManager : MonoBehaviour
    {
        #region Audio Manager

        [System.Serializable]
        public class AudioManager
        {
            [Header("Audios")] [SerializeField] private AudioSource audioSource;
            [SerializeField] private AudioClip correctAudio, wrongAudio, completeAudio;


            public void PlayCorrectAudio() => audioSource.PlayOneShot(correctAudio);
            public void PlayWrongAudio() => audioSource.PlayOneShot(wrongAudio);
            public void PlayCompleteAudio() => audioSource.PlayOneShot(completeAudio);
        }

        #endregion

        [Header("TrashBin")] [SerializeField] private TrashBinManager paperTrashBin;
        [SerializeField] private TrashBinManager plasticTrashBin;
        [SerializeField] private TrashBinManager glassTrashBin;
        [SerializeField] List<TrashManager> allTrash;

        [Header("Audio Manger")] [SerializeField]
        private AudioManager _audioManager;


        private void OnEnable()
        {
            TrashManager.onCollectTrash += CheckIfTrashIsCorrect;
        }


        private void OnDisable()
        {
            TrashManager.onCollectTrash -= CheckIfTrashIsCorrect;
        }

        private void CheckIfTrashIsCorrect(bool state, TrashBinManager.e_DustType trashType)
        {
            Debug.Log($"{trashType} : {state}");
            if (state)
            {
                switch (trashType)
                {
                    case TrashBinManager.e_DustType.Paper:
                        paperTrashBin.UpdateScore();
                        break;
                    case TrashBinManager.e_DustType.Plastic:
                        plasticTrashBin.UpdateScore();
                        break;
                    case TrashBinManager.e_DustType.Glass:
                        glassTrashBin.UpdateScore();
                        break;
                }

                _audioManager.PlayCorrectAudio();
                if (allDisPosed())
                    _audioManager.PlayCompleteAudio();
            }
            else
            {
                _audioManager.PlayWrongAudio();
            }
        }

        internal bool allDisPosed()
        {
            return allTrash.All(trash => trash.isDisposed);
        }
    }
}