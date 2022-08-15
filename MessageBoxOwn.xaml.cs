using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArionControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для MessageBoxOwn.xaml
    /// </summary>
    public partial class MessageBoxOwn : Window
    {
        private MessageBoxResult _result = MessageBoxResult.None;

        public MessageBoxOwn()
        {
            InitializeComponent();
        }

        private void AddButtons(MessageBoxButton buttons)
        {
            switch (buttons)
            {
                case MessageBoxButton.OK:
                    AddButton("OK", MessageBoxResult.OK);
                    break;
                case MessageBoxButton.OKCancel:
                    AddButton("OK", MessageBoxResult.OK);
                    AddButton("Отмена", MessageBoxResult.Cancel, isCancel: true);
                    break;
                case MessageBoxButton.YesNo:
                    AddButton("Да", MessageBoxResult.Yes);
                    AddButton("Нет", MessageBoxResult.No);
                    break;
                case MessageBoxButton.YesNoCancel:
                    AddButton("Да", MessageBoxResult.Yes);
                    AddButton("Нет", MessageBoxResult.No);
                    AddButton("Отмена", MessageBoxResult.Cancel, isCancel: true);
                    break;
                default:
                    throw new ArgumentException("Неизвестное значение", "buttons");
            }
        }

        private void AddButton(string text, MessageBoxResult result, bool isCancel = false)
        {
            var button = new Button
            {
                Content = text,
                IsCancel = isCancel,
                Style = FindResource("BlueWhiteButton") as Style,
                Width=140,
                Height=35,
                Margin = new Thickness(10, 0, 0, 0)
            };

            // Устанавливает фокус на одну из кнопок
            if (result == MessageBoxResult.OK || result == MessageBoxResult.Yes)
            {
                button.IsDefault = true;
                FocusManager.SetFocusedElement(this, button);
            }
            button.Click += (o, args) => { _result = result; DialogResult = true; };
            ButtonContainer.Children.Add(button);
        }
        public static MessageBoxResult Show(string caption, string message,
            MessageBoxButton buttons)
        {
            var dialog = new MessageBoxOwn
            {
                Title = caption,
                MessageContainer = { Text = message }
            };
            dialog.AddButtons(buttons);
            dialog.ShowDialog();
            return dialog._result;
        }
        public static MessageBoxResult Show(string message)
        {
            var dialog = new MessageBoxOwn { MessageContainer = { Text = message } };
            dialog.AddButtons(MessageBoxButton.OK);
            dialog.ShowDialog();
            return dialog._result;
        }
    }
}
