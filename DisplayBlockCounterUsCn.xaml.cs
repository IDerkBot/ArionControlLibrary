using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArionLibrary.Controllers;

namespace ArionControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для DisplayBlockCounterUsCn.xaml
    /// </summary>
    public partial class DisplayBlockCounterUsCn : UserControl
    {
        private double _value;
        private bool _send;
        private string _valueString;
        private readonly double _originalDelta = 1;
        private int _repeatClickCounter;
        private double _interval = 1.5;
        private bool _isHide;
        private Timer _timerShow;

        /// <summary>
        /// Максимальное значение
        /// </summary>
        public double Max { get; set; }

        /// <summary>
        /// Минимальное значение
        /// </summary>
        public double Min { get; set; }

        /// <summary>
        /// Шаг
        /// </summary>
        public double Delta { get; set; }

        public string Format { get; set; }
        public bool Change;
        public bool Send
        {
            get => _send;
            set
            {
                _send = value;

                SendChange?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler PlusClick;
        public event EventHandler MinusClick;
        public event EventHandler ValueChange;
        public event EventHandler ValueLabelChange;
        public event EventHandler SendChange;
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
        /// <summary>
        /// Значение
        /// </summary>
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

        /// <summary>
        /// Перевод значения в строку
        /// </summary>
        /// <returns>Значение в строковом формате</returns>
        public override string ToString()
        {
            return Value.ToString(format: Format);
        }

        public DisplayBlockCounterUsCn()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Событие при нажатии на кнопку плюс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPlus_Click(object sender, RoutedEventArgs e)
        {
            Send = true;
            Change = true;
            LblActualValue.Dispatcher.BeginInvoke(new Action(() => LblActualValue.Visibility = Visibility.Hidden));
            LblTargetValue.Dispatcher.BeginInvoke(new Action(() => LblTargetValue.Visibility = Visibility.Visible));
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

        /// <summary>
        /// Событие при нажатии на кнопку минус
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            Send = true;
            Change = true;
            LblActualValue.Dispatcher.BeginInvoke(new Action(() => LblActualValue.Visibility = Visibility.Hidden));
            LblTargetValue.Dispatcher.BeginInvoke(new Action(() => LblTargetValue.Visibility = Visibility.Visible));
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

        private void TimerShowOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (Change) VisibilityChange();
            _timerShow.Dispose();
        }

        public void ShowValue()
        {
            LblTargetValue.ChangeContentAsync(Value.ToString(Format));
            LblActualValue.ChangeContentAsync(Value.ToString(Format));
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

            Send = false;
            Change = false;

            LblTargetValue.Dispatcher.BeginInvoke(new Action(() => LblTargetValue.Visibility = Visibility.Hidden));
            LblActualValue.Dispatcher.BeginInvoke(new Action(() => LblActualValue.Visibility = Visibility.Visible));
        }
    }
}
