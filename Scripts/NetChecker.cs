using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace VMichalak.NetChecker
{
    public class NetChecker: MonoBehaviour
    {
        public enum Status { PENDING, NO_CONNECTION, WALLED_GARDEN, CONNECTED }
        
        public delegate void OnCheckStartedEventHandler();
        public delegate void OnCheckFinishedEventHandler(Status status);
        public delegate void OnConnectionStatusChangedEventHandler(Status status);
        public delegate void OnCheckTimeoutEventHandler();

        public event OnCheckStartedEventHandler OnCheckStarted;
        public event OnCheckFinishedEventHandler OnCheckFinished;
        public event OnConnectionStatusChangedEventHandler OnConnectionStatusChanged;
        public event OnCheckTimeoutEventHandler OnCheckTimeout;
        public NetCheckerRule Rule = NetCheckerRule.PLATFORM;
        public Status LastStatus { get; private set; } = Status.PENDING;
        
        [Tooltip("In Seconds")]
        public float CheckInterval = 30;
        [Tooltip("In Seconds")]
        public float Timeout = 30;

        public bool IsRunning { get; private set; } = false;

        public void CheckConnection()
        {
            StartCoroutine(CheckConnectionCoroutine());
        }

        public void StartConnectionCheck()
        {
            IsRunning = true;
            StartCoroutine(StartConnectionCheckCoroutine());
        }

        public void StopConnectionCheck()
        {
            IsRunning = false;
        }

        private IEnumerator StartConnectionCheckCoroutine()
        {
            while (IsRunning)
            {
                yield return CheckConnectionCoroutine();
                yield return new WaitForSeconds(CheckInterval);
            }
        }

        private IEnumerator CheckConnectionCoroutine()
        {
            UnityWebRequest request = UnityWebRequest.Get(Rule.Target);
            request.timeout = (int)Timeout;
            yield return request.SendWebRequest();
            OnCheckStarted?.Invoke();

            if (request.isNetworkError)
            {
                SendFinishedCallback(Status.NO_CONNECTION);
            }
            else if (
                Rule.ExpectedHttpCode == request.responseCode && 
                request.downloadHandler.text == Rule.ExpectedContent) 
            {
                SendFinishedCallback(Status.CONNECTED);
            }
            else if (request.isDone)
            {
                SendFinishedCallback(Status.WALLED_GARDEN);
            }
            else
            {
                OnCheckTimeout?.Invoke();
            }
        }

        private void SendFinishedCallback(Status status)
        {
            OnCheckFinished?.Invoke(status);
            if (status != LastStatus) { OnConnectionStatusChanged?.Invoke(status); }
            LastStatus = status;
        }
    }
}