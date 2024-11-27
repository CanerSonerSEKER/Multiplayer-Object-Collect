using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Game
{
    public class CountdownTimer : MonoBehaviour
    {
        [SerializeField] private float _countdownTime = 30f;
        [SerializeField] private float _startingCountdown = 3f;
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private TextMeshProUGUI _startTimeTextTMP;

        private PhotonView _photonView;
        private bool _isTimeFinished = true;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        private void Start()
        {
            StartingCountdownTimer();
            StartCountDown();
        }

        private void StartingCountdownTimer()
        {
            StartCoroutine(DisplayTime());
        }

        private IEnumerator DisplayTime()
        {
            Time.timeScale = 0f;

            while (_startingCountdown > 0)
            {
                _startTimeTextTMP.text = _startingCountdown.ToString();
                
                yield return new WaitForSecondsRealtime(1f);

                _startingCountdown--;
            }

            _startTimeTextTMP.text = "GO!!";

            yield return new WaitForSecondsRealtime(1f);

            Time.timeScale = 1f;
            
            _startTimeTextTMP.gameObject.SetActive(false);
        }
        
        private void StartCountDown()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _isTimeFinished = false;
                StartCoroutine(nameof(LoseTime));
            }
        }
        
        private IEnumerator LoseTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                _photonView.RPC(nameof(ReduceTime), RpcTarget.AllBuffered);
            }
        }
        
        [PunRPC]
        private void ReduceTime()
        {
            _countdownTime--;
            float minutes = Mathf.FloorToInt(_countdownTime / 60);
            float seconds = Mathf.FloorToInt(_countdownTime % 60);
            _timeText.text = $"{minutes : 00} : {seconds : 00}";

            if (_countdownTime <= 0)
            {
                StopCoroutine(LoseTime());
                _timeText.text = $"TIME'S UP!!";
                TimesUp();
            }
        }
        
        /// <summary>
        /// Bu kısımda scoreboard gösterimi olacak şekilde ayarlanmalı. Zaman bitiyor çünkü.
        /// </summary>
        private void TimesUp()
        {
            _isTimeFinished = true;
            
        }
        
    }
}