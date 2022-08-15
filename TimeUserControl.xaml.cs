using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArionLibrary.Utilities;
using Timer = System.Timers.Timer;

namespace ArionControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для TimeUserControl.xaml
    /// </summary>
    public partial class TimeUserControl : UserControl
    {
        #region Variables

        private int _value;
        private double _interval;
        private int _originalDelta = 1;
        private int _repeatClickCounter;
        private bool _isHide;
        private bool _send;
        private Timer _timerShow;

        public int Max { get; set; }
        public int Min { get; set; }
        public int Delta { get; set; }
        public bool Change { get; set; }
        public event EventHandler PlusClick;
        public event EventHandler MinusClick;
        public event EventHandler ValueChange;
        public event EventHandler SendChange;
        public bool Send
        {
            get => _send;
            set
            {
                _send = value;

                SendChange?.Invoke(this, EventArgs.Empty);
            }
        }
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

        #endregion

        private void BtnPlus_Click(object sender, RoutedEventArgs e)
        {
            _send = true;
            Change = true;
            LblActualValue.ChangeVisible(false);
            LblTargetValue.ChangeVisible(true);
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
            LblActualValue.ChangeVisible(false);
            LblTargetValue.ChangeVisible(true);
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
            LblActualValue.ChangeContentAsync(Value.ConvertTime());
            LblTargetValue.ChangeContentAsync(Value.ConvertTime());
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
            LblTargetValue.ChangeVisible(false);
            LblActualValue.ChangeVisible(true);

            Change = false;
            SendValue(Value);
        }

        private void SendValue(int value)
        {
            //if (_send)
            //{
            //    XRayControllerRs232.Send($"TP:{value / 6} ");
            //    _send = false;
            //}
        }

        public void UpdateActualValue(string actualValue)
        {
            if (Change)
                return;
            LblActualValue.ChangeContentAsync(actualValue);
        }
    }
}
