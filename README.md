NetCheck - Reliable Internet Detection for Unity
================================================

This library implement [Captive Portal Detection](https://success.tanaza.com/s/article/How-Automatic-Detection-of-Captive-Portal-works) for Unity.

## How To Use

### One time Check

    NetChecker netChecker = GetComponent<NetChecker>();
    netChecker.OnCheckFinished += status => Debug.Log("Internet Check Result: " + status);
    netChecker.CheckConnection();

### Background Check

    NetChecker netChecker = GetComponent<NetChecker>();
    netChecker.OnConnectionStatusChanged += status => Debug.Log(status);
    netChecker.StartConnectionCheck();