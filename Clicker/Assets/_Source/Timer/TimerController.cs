using UnityEngine;
using Zenject;
using SaveSystem;

namespace Timer
{
    public class TimerController : AJsonSaver
    {
        private const string TIME_KEY = "TimePlayed";

        private TimerView _view;
        private float _elapsedTime;
        private int _seconds = 0;
        private int _minutes = 0;

        [Inject]
        public void Construct(TimerView timerView)
        {
            _view = timerView;
            SaveKey = TIME_KEY;
        }

        void Update()
        {
            _elapsedTime += Time.deltaTime;
            SessionInfo.Instance.timePlayed += Time.deltaTime;
            ConvertToTime();
        }

        public override void SetLoadedData(object data) =>
            _elapsedTime = float.Parse(data.ToString());

        public override object GetDataForSave() => _elapsedTime;

        private void ConvertToTime()
        {
            int hours = Mathf.FloorToInt(_elapsedTime /3600f);
            int minutes = Mathf.FloorToInt((_elapsedTime - hours * 3600f) / 60f);
            int seconds = Mathf.FloorToInt((_elapsedTime - hours * 3600f) - (minutes * 60f));

            if (seconds == _seconds + 10)
            {
                AppMetrica.Instance.ReportEvent("10SenodsPlayed");
                _seconds += 10;
            }
            if (seconds == 0)
                _seconds = 0;

            if (minutes == _minutes + 1)
            {
                AppMetrica.Instance.ReportEvent("1MinutePlayed");
                _minutes += 1;
            }

            if(minutes == 0)
                _minutes = 0;

            _view.SetTimeText(hours, minutes, seconds);
        }
    }
}