using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ArionControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для VirtualKeyboard.xaml
    /// </summary>
    public partial class VirtualKeyboard : Window
    {
        private readonly Control _control;
        private bool _capsLock = true;
        private bool _shiftPressed;
        private bool _isRussia;
        private string _content;

        public VirtualKeyboard()
        {
            InitializeComponent();
        }
        public VirtualKeyboard(Control control)
        {
            InitializeComponent();
            _control = control;

            Top = SystemParameters.PrimaryScreenHeight - Height - 50;
            Left = SystemParameters.PrimaryScreenWidth/2 - Width/2;
        }

        public new string Show()
        {
            ShowDialog();
            return _content;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            switch (btn?.Content.ToString().ToUpper())
            {
                case "TAB":
                    if (_shiftPressed) _shiftPressed = false;

                    break;
                case "CAPS LOCK":
                    if (_shiftPressed) _shiftPressed = false;
                    _capsLock = !_capsLock;

                    if (_capsLock)
                    {
                        Gr1.ToUpper();
                        Gr2.ToUpper();
                        Gr3.ToUpper();
                        Gr4.ToUpper();
                        Gr5.ToUpper();
                    }
                    else
                    {
                        Gr1.ToLower();
                        Gr2.ToLower();
                        Gr3.ToLower();
                        Gr4.ToLower();
                        Gr5.ToLower();
                    }
                    break;
                case "ENTER":
                    if (_shiftPressed) _shiftPressed = false;
                    Close();
                    break;
                case "SHIFT":
                    _shiftPressed = !_shiftPressed;
                    break;
                case "CTRL":
                    if (_shiftPressed) _shiftPressed = false;
                    break;
                case "ALT":
                    if (_shiftPressed)
                    {
                        Translate();
                        _shiftPressed = false;
                    }
                    break;
                case "WIN":
                    if (_shiftPressed) _shiftPressed = false;

                    break;
                case "SPACE":
                    {
                        if (_shiftPressed) _shiftPressed = false;
                        if (_control is TextBox tb) tb.Text += " ";
                        if (_control is PasswordBox pb) pb.Password += " ";
                    }
                    break;
                case "FN":
                    if (_shiftPressed) _shiftPressed = false;
                    break;
                case "BACKSPACE":
                    {
                        if (_shiftPressed) _shiftPressed = false;
                        if (_control is TextBox tb) tb.Text = tb.Text.Substring(0, tb.Text.Length - 1);
                        if (_control is PasswordBox pb) pb.Password = pb.Password.Substring(0, pb.Password.Length - 1);
                    }
                    break;
                default:
                    {
                        if (_shiftPressed) _shiftPressed = false;
                        if (_control is TextBox tb) tb.Text += btn?.Content.ToString() ?? string.Empty;
                        if (_control is PasswordBox pb) pb.Password += btn?.Content.ToString() ?? string.Empty;
                    }
                    break;
            }
        }

        private void Translate()
        {
            Key016.Content = _isRussia ? "q" : "й";
            Key017.Content = _isRussia ? "w" : "ц";
            Key018.Content = _isRussia ? "e" : "у";
            Key019.Content = _isRussia ? "r" : "к";
            Key020.Content = _isRussia ? "t" : "е";
            Key021.Content = _isRussia ? "y" : "н";
            Key022.Content = _isRussia ? "u" : "г";
            Key023.Content = _isRussia ? "i" : "ш";
            Key024.Content = _isRussia ? "o" : "щ";
            Key025.Content = _isRussia ? "p" : "з";
            Key026.Content = _isRussia ? "[" : "х";
            Key027.Content = _isRussia ? "]" : "ъ";
            Key029.Content = _isRussia ? "a" : "ф";
            Key030.Content = _isRussia ? "s" : "ы";
            Key031.Content = _isRussia ? "d" : "в";
            Key032.Content = _isRussia ? "f" : "а";
            Key033.Content = _isRussia ? "g" : "п";
            Key034.Content = _isRussia ? "h" : "р";
            Key035.Content = _isRussia ? "j" : "о";
            Key036.Content = _isRussia ? "k" : "л";
            Key037.Content = _isRussia ? "l" : "д";
            Key038.Content = _isRussia ? ";" : "ж";
            Key039.Content = _isRussia ? "'" : "э";
            Key042.Content = _isRussia ? "z" : "я";
            Key043.Content = _isRussia ? "x" : "ч";
            Key044.Content = _isRussia ? "c" : "с";
            Key045.Content = _isRussia ? "v" : "м";
            Key046.Content = _isRussia ? "b" : "и";
            Key047.Content = _isRussia ? "n" : "т";
            Key048.Content = _isRussia ? "m" : "ь";
            Key049.Content = _isRussia ? "," : "б";
            Key050.Content = _isRussia ? "." : "ю";
            Key051.Content = _isRussia ? "/" : ".";
            _isRussia = !_isRussia;
            if (_capsLock)
            {
                Gr1.ToUpper();
                Gr2.ToUpper();
                Gr3.ToUpper();
                Gr4.ToUpper();
                Gr5.ToUpper();
            }
            else
            {
                Gr1.ToLower();
                Gr2.ToLower();
                Gr3.ToLower();
                Gr4.ToLower();
                Gr5.ToLower();
            }
        }
    }

    internal static class ExtensionsKeyboard
    {
        internal static void ToUpper(this Grid grid)
        {
            grid.Children.OfType<Button>().ToList().ForEach(x => x.Content = x.Content.ToString().ToUpper());
        }
        internal static void ToLower(this Grid grid)
        {
            grid.Children.OfType<Button>().ToList().ForEach(x => x.Content = x.Content.ToString().ToLower());
        }
    }
}
