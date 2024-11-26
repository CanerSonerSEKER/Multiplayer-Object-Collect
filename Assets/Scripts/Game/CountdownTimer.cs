using System;
using System.Collections;
using System.Timers;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Game
{
    public class CountdownTimer : MonoBehaviour
    {
        [SerializeField] private float _time = 30f;
        [SerializeField] private TextMeshProUGUI _timeText;
        private PhotonView _photonView;

        private bool _isTimeFinished = true;
            
        private bool _startTimer = false;
        private float _timerDecrementValue;
        private float _startTime;
        private Hashtable _customValue;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        private void Start()
        {

            StartCountDown();
            /*if (PhotonNetwork.IsMasterClient)
            {
                _customValue = new Hashtable();
                _startTime = (float)PhotonNetwork.Time;
                _startTimer = true;
                _customValue.Add("StartTime", _startTime);
                PhotonNetwork.CurrentRoom.SetCustomProperties(_customValue);
            }
            else
            {
                _startTime = float.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
            }*/
        }

        private void StartCountDown()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _isTimeFinished = false;
                StartCoroutine(nameof(LoseTime));
                //float minutes = Mathf.FloorToInt(_time / 60);
                //float seconds = Mathf.FloorToInt(_time % 60);
                //_timeText.text = $"{minutes : 00} : {seconds : 00}";
            }
        }

        private IEnumerator LoseTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                _photonView.RPC(nameof(ReduceTime), RpcTarget.AllBuffered);
            }
        }
        
        [PunRPC]
        private void ReduceTime()
        {
            _time--;
            float minutes = Mathf.FloorToInt(_time / 60);
            float seconds = Mathf.FloorToInt(_time % 60);
            _timeText.text = $"{minutes : 00} : {seconds : 00}";

            if (_time <= 0)
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

        private void Update()
        {
            /*
            if (!_startTimer) return;

            _timerDecrementValue = _time - ((float)PhotonNetwork.Time - _startTime);
            float minutes = Mathf.FloorToInt(_timerDecrementValue / 60);
            float seconds = Mathf.FloorToInt(_timerDecrementValue % 60);

            _timeText.text = $"{minutes:00} : {seconds:00}";
            
            if (_timerDecrementValue <= 0)
            {
                _timeText.text = $"IT'S DONE!!!";
                Time.timeScale = 0f;
                //Timer Completed
                //ScoreBoard Yapılacak gösterilecektiiiiir.
            }
        */
        }
    }
}