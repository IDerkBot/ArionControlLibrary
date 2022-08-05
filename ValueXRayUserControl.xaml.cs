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
    /// Логика взаимодействия для ValueXRayUserControl.xaml
    /// </summary>
    public partial class ValueXRayUserControl : UserControl
    {
        public double Max = 100;
        public double Min = 0;
        public double Delta = 1;
        public double Divider = 1;
        private double _value = 0;
        public string Format;
        private bool _send;
        public bool Change;

        private string _valueString;

        public string ValueString
        {
            get => _valueString;
            set
            {
                this._valueString = value;
                LabelValue.Content = value;

                ValueLabelChange?.Invoke(this, null);
            }
        }

        private double _originalDelta = 1;
        //private bool _getActual = true;

        public event EventHandler PlusClick;

        public event EventHandler MinusClick;

        public event EventHandler ValueChange;
        public event EventHandler ValueLabelChange;

        private int _repeatClickCounter = 0;
        private double _interval = 1.5;
        private bool _isHide;
        internal bool Send;

        private Timer _timerShow;

        public double Value
        {
            get => _value;

            set
            {
                _value = value;
                ShowValue();

                ValueChange?.Invoke(this, null);
            }
        }

        public override string ToString()
        {
            return Value.ToString(format: Format);
        }

        public ValueXRayUserControl()
        {
            InitializeComponent();
        }

        private void BtnPlus_Click(object sender, RoutedEventArgs e)
        {
            Send = true;
            Change = true;
            lblActualValue.Dispatcher.BeginInvoke(new Action(() => lblActualValue.Visibility = Visibility.Hidden));
            lblTargetValue.Dispatcher.BeginInvoke(new Action(() => lblTargetValue.Visibility = Visibility.Visible));
            _isHide = true;
            if (Value + Delta < Max)
                Value += Delta;
            else
                Value = Max;

            _repeatClickCounter++;
            _repeatClickCounter %= 5;
            if (_repeatClickCounter == 0)
                Delta += 2;

            PlusClick?.Invoke(this, e);
        }

        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            Send = true;
            Change = true;
            lblActualValue.Dispatcher.BeginInvoke(new Action(() => lblActualValue.Visibility = Visibility.Hidden));
            lblTargetValue.Dispatcher.BeginInvoke(new Action(() => lblTargetValue.Visibility = Visibility.Visible));
            _isHide = true;
            if (Value - Delta > Min && Value - Delta >= Delta)
                Value -= Delta;
            else
                Value = Min;

            _repeatClickCounter++;
            _repeatClickCounter %= 5;
            if (_repeatClickCounter == 0)
                Delta += 2;

            MinusClick?.Invoke(this, e);
        }

        public void Init(double min, double max, double delta, double divider, double val, string valueString,
            string format, bool send = true, double interval = 1.5)
        {
            Min = min;
            Max = max;
            _originalDelta = Delta = delta;
            Divider = divider;
            Value = val;
            ValueString = valueString;
            Format = format;
            _interval = interval;
            _send = send;
        }

        private void TimerShowOnElapsed(object sender, ElapsedEventArgs e)
        {
            if(Change) VisibilityChange();
            _timerShow.Dispose();
        }

        public void ShowValue()
        {
            lblTargetValue.ChangeContent(Value.ToString(Format));
            lblActualValue.ChangeContent(Value.ToString(Format));
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

        private void VisibilityChange()
        {
            if (_isHide) return;

            lblTargetValue.Dispatcher.BeginInvoke(new Action(() => lblTargetValue.Visibility = Visibility.Hidden));
            lblActualValue.Dispatcher.BeginInvoke(new Action(() => lblActualValue.Visibility = Visibility.Visible));

            if (_send)
            {
                SendValue();
            }
            Change = false;
        }

        private void SendValue()
        {
            if (Send)
            {
                if (ValueString == "kV")
                    XRayControllerRs232.SendKv(Value);
                else if (ValueString == "mA")
                    XRayControllerRs232.SendMa(Math.Round(Value * 10));
                Send = false;
            }
        }
    }
}
