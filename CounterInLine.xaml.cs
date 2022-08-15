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
        #region Vars

        #region Private

        private string _valueString;
        private double _value;
        private const double ORIGINAL_DELTA = 1;
        private int _repeatClickCounter;
        private bool _send;

        #endregion

        #region Events

        public event EventHandler PlusClick;
        public event EventHandler MinusClick;
        public event EventHandler ValueChange;
        public event EventHandler ValueLabelChange;
        public event EventHandler SendChange;

        #endregion

        #region Public

        public bool Send
        {
            get => _send;
            set
            {
                _send = value;

                SendChange?.Invoke(this, EventArgs.Empty);
            }
        }
        public double Max { get; set; }
        public double Min { get; set; }
        public double Delta { get; set; }
        public double Divider { get; set; }
        public string Format { get; set; }
        public bool Change { get; set; }
        public string ValueString
        {
            get => _valueString;
            set
            {
                _valueString = value;
                LabelValue.Content = value;

                ValueLabelChange?.Invoke(this, null);
            }
        }
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

        #endregion

        #endregion

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
            Delta = ORIGINAL_DELTA;
        }

        private void BtnNegative_OnClick(object sender, RoutedEventArgs e)
        {
            Send = false;
            Change = true;
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
            Send = false;
            Change = true;
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

        private void VisibilityChange()
        {
            Send = true;
            Change = false;
        }
    }
}
