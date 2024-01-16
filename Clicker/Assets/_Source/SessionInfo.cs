using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SessionInfo : MonoBehaviour
{
    public int clickPerSession { get; set; }
    public float pointsPerSession { get; set; }
    public float timePlayed { get; set; }

    public static SessionInfo Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SendReport()
    {
        string json = Instance.ToJSON();
        AppMetrica.Instance.ReportEvent("SessionInfo", json);
    }

    public string ToJSON()
    {
        string json = "";
        json += "{";
        json += JSONConvector.Convert("clickPerSession", clickPerSession);
        json += ",";
        json += JSONConvector.Convert("pointsPerSession", pointsPerSession);
        json += ",";
        json += JSONConvector.Convert("timePlayed", timePlayed);
        json += "}";
        return json;
    }
}
