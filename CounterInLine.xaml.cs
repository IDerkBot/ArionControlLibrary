using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArionControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для CounterInLine.xaml
    /// </summary>
    public partial class CounterInLine : UserControl
    {
        public double Max = 100;
        public double Min = 0;
        public double Delta = 1;
        public double Divider = 1;
        private double _value = 0;
        public string Format;
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

        public event EventHandler PlusClick;

        public event EventHandler MinusClick;

        public event EventHandler ValueChange;
        public event EventHandler ValueLabelChange;

        private int _repeatClickCounter;
        internal bool Send;
        private bool _isControl;

        public double Value
        {
            get => _value;

            set
            {
                this._value = value;
                ShowValue();

                ValueChange?.Invoke(this, null);
            }
        }

        private void ShowValue()
        {
            LabelValue.Dispatcher.BeginInvoke(new Action(() => LabelValue.Content = Value.ToString(Format)));
        }

        public override string ToString()
        {
            return Value.ToString(format: Format);
        }

        public CounterInLine()
        {
            InitializeComponent();
        }

        private void Btn_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Change) VisibilityChange();
            _repeatClickCounter = 0;
            Delta = _originalDelta;
        }

        private void BtnNegative_OnClick(object sender, RoutedEventArgs e)
        {
            Send = true;
            Change = true;
            //_isHide = true;
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

        private void BtnPositive_OnClick(object sender, RoutedEventArgs e)
        {
            Send = true;
            Change = true;
            //_isHide = true;
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
        public void Init(double min, double max, double delta, double divider, double val, string valueString,
            string format)
        {
            Min = min;
            Max = max;
            _originalDelta = Delta = delta;
            Divider = divider;
            Value = val;
            ValueString = valueString;
            Format = format;
            //_interval = interval;
        }
        public void Init(double min, double max, double delta, double divider, double val)
        {
            Min = min;
            Max = max;
            _originalDelta = Delta = delta;
            Divider = divider;
            Value = val;
        }
        public void Init(double min, double max, double delta, double divider, double val, bool isControl)
        {
            Min = min;
            Max = max;
            _originalDelta = Delta = delta;
            Divider = divider;
            Value = val;
            _isControl = isControl;
        }

        private void TimerShowOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (Change) VisibilityChange();
            //_timerShow.Dispose();
        }

        private void VisibilityChange()
        {
            SendValue();
            Change = false;
        }

        private void SendValue()
        {
            if (_isControl)
            {
                //MainManager.Controller.SetSpeed((int)Value);
            }
        }
    }
}
