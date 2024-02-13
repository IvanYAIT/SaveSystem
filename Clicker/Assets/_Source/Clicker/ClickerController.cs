using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using SaveSystem;
using UniRx;
using UnityEngine.UI;
using System;

namespace Clicker
{
    public class ClickerController : APlayerPrefsSaver, IPointerDownHandler, IPointerUpHandler
    {
        private const string POINT_KEY = "Points";

        [SerializeField] private float pointMult;
        [SerializeField] private Button btn;

        private ClickerView _view;
        private ReactiveProperty<float> _points;
        private float _pointsPerPress;
        private bool isDown;
        private AsyncReactiveCommand _command;

        [Inject]
        public void Construct(ClickerView clickerView)
        {
            _view = clickerView;
            SaveKey = POINT_KEY;
            _points = new ReactiveProperty<float>();
            _command = new AsyncReactiveCommand();

            _command.Subscribe(_ =>
            {
                _points.Value++;

                return Observable.Timer(TimeSpan.FromSeconds(5)).AsUnitObservable();
            });
            _command.BindTo(btn);
        }

        void Start()
        {
            _view.SetText($"{_points:F2}");
            _points.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                    {
                        _view.SetText($"{_points:F2}");
                    }).AddTo(this);
        }

        private void Update()
        {
            if (isDown)
                _pointsPerPress += Time.deltaTime;

        }

        public void OnPointerDown(PointerEventData eventData) =>
            isDown = true;

        public void OnPointerUp(PointerEventData eventData)
        {
            isDown = false;
            _points.Value += pointMult * _pointsPerPress;
            _pointsPerPress = 0;
        }

        public override void SetLoadedData(object data) =>
            _points.Value = float.Parse(data.ToString());

        public override object GetDataForSave() => _points.Value;
    }
}