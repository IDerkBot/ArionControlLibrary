using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArionLibrary.Controllers;
using Timer = System.Timers.Timer;

namespace ArionControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для TimeUserControl.xaml
    /// </summary>
    public partial class TimeUserControl : UserControl
    {
        #region Variables

        public int Max = 100;
        public int Min = 0;
        public int Delta = 6;
        private int _value = 0;
        private double _interval;
        private int _originalDelta = 1;
        public bool Change;
        public event EventHandler PlusClick;
        public event EventHandler MinusClick;
        public event EventHandler ValueChange;
        private int _repeatClickCounter = 0;
        private bool _isHide;
        private bool _send;
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                ShowValue();

                ValueChange?.Invoke(this, null);
            }
        }
        private Timer _timerShow;

        #endregion

        private void BtnPlus_Click(object sender, RoutedEventArgs e)
        {
            _send = true;
            Change = true;
            lblActualValue.ChangeVisible(false);
            lblTargetValue.ChangeVisible(true);
            _isHide = true;
            if (Value + Delta < Max)
                Value += Delta;
            else
                Value = Max;

            _repeatClickCounter++;
            _repeatClickCounter %= 5;
            if (_repeatClickCounter == 0)
                Delta += 6;

            PlusClick?.Invoke(this, e);
        }
        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            _send = true;
            Change = true;
            lblActualValue.ChangeVisible(false);
            lblTargetValue.ChangeVisible(true);
            _isHide = true;
            if (Value - Delta > Min && Value - Delta >= Delta)
                Value -= Delta;
            else
                Value = Min;

            _repeatClickCounter++;
            _repeatClickCounter %= 5;
            if (_repeatClickCounter == 0)
                Delta += 6;

            MinusClick?.Invoke(this, e);
        }

        public TimeUserControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="min">Минимальное</param>
        /// <param name="max">Максимальное</param>
        /// <param name="delta">Шаг</param>
        /// <param name="val">Значение</param>
        /// <param name="interval">Интервал обновления</param>
        public void Init(int min, int max, int delta, int val, double interval = 1)
        {
            Min = min;
            Max = max;
            _originalDelta = Delta = delta;
            Value = val;
            _interval = interval;
        }

        private void TimerShowOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (Change) VisibilityChange(_isHide);
            _timerShow.Dispose();
        }

        public void ShowValue()
        {
            var convertTargetTime = Value.ConvertTime();
            var convertActualTime = Value.ConvertTime();

            lblActualValue.ChangeContent(convertActualTime);
            lblTargetValue.ChangeContent(convertTargetTime);
        }

        private void Btn_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            _timerShow = new Timer { AutoReset = false, Interval = _interval * 1000 };
            _timerShow.Elapsed += TimerShowOnElapsed;
            _timerShow.Start();
            _repeatClickCounter = 0;
            Delta = _originalDelta;
            _isHide = false;
        }

        private void VisibilityChange(bool isHide = false)
        {
            if (_isHide) return;
            lblTargetValue.ChangeVisible(false);
            lblActualValue.ChangeVisible(true);

            Change = false;
            SendValue(Value);
        }

        private void SendValue(int value)
        {
            if (_send)
            {
                XRayControllerRs232.Send($"TP:{value / 6} ");
                _send = false;
            }
        }

        public void UpdateActualValue(string actualValue)
        {
            if (Change)
                return;
            lblActualValue.ChangeContent(actualValue);
        }
    }
}
