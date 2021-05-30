using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CalcLibrary;

namespace Calculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        delegate bool ButtonSwitchingCondition(Button btn);

        int prevNumSystem = 10;
        Key prevKey = Key.None;
        int numSystem = 10;
        string savedText = "0";
        Dictionary<string, int> numSystemToInt = new Dictionary<string, int>
        {
            { "BIN", 2 },
            { "OCT", 8 },
            { "DEC", 10 },
            { "HEX", 16 }
        };

        Dictionary<char, Key> charToKey = new Dictionary<char, Key>
        {
            { '-', Key.Subtract },
            { ',', Key.OemComma },
            { '!', Key.D1 },
            { '^', Key.D6 },
            { '/', Key.Divide },
            { '*', Key.Multiply },
            { '+', Key.Add },
            { '|', Key.OemPipe },
            { 'A', Key.A },
            { 'B', Key.B },
            { 'C', Key.C },
            { 'D', Key.D },
            { 'E', Key.E },
            { 'e', Key.E },
            { 'F', Key.F },
            { 'X', Key.X },
            { 'O', Key.O },
            { 'R', Key.R },
            { 'I', Key.I },
            { 'V', Key.V },
            { 'M', Key.M },
            { 'N', Key.N },
            { 'T', Key.T },
            { 'S', Key.S },
            { 'π', Key.P },
            { 'G', Key.G },
            { '0', Key.D0 },
            { '1', Key.D1 },
            { '2', Key.D2 },
            { '3', Key.D3 },
            { '4', Key.D4 },
            { '5', Key.D5 },
            { '6', Key.D6 },
            { '7', Key.D7 },
            { '8', Key.D8 },
            { '9', Key.D9 }
        };

        Dictionary<Key, char> keyToChar = new Dictionary<Key, char>
        {
            { Key.OemMinus, '-' },
            { Key.OemPlus, '+' },
            { Key.Subtract, '-' },
            { Key.OemComma, ',' },
            { Key.Divide, '/' },
            { Key.Multiply, '*' },
            { Key.Add, '+' },
            { Key.OemPipe, '|' },
            { Key.A, 'A' },
            { Key.B, 'B' },
            { Key.C, 'C' },
            { Key.D, 'D' },
            { Key.E, 'E' },
            { Key.F, 'F' },
            { Key.X, 'X' },
            { Key.O, 'O' },
            { Key.R, 'R' },
            { Key.I, 'I' },
            { Key.V, 'V' },
            { Key.M, 'M' },
            { Key.N, 'N' },
            { Key.T, 'T' },
            { Key.S, 'S' },
            { Key.G, 'G' },
            { Key.P, 'π' },
            { Key.D0, '0' },
            { Key.D1, '1' },
            { Key.D2, '2' },
            { Key.D3, '3' },
            { Key.D4, '4' },
            { Key.D5, '5' },
            { Key.D6, '6' },
            { Key.D7, '7' },
            { Key.D8, '8' },
            { Key.D9, '9' },
            { Key.NumPad0, '0' },
            { Key.NumPad1, '1' },
            { Key.NumPad2, '2' },
            { Key.NumPad3, '3' },
            { Key.NumPad4, '4' },
            { Key.NumPad5, '5' },
            { Key.NumPad6, '6' },
            { Key.NumPad7, '7' },
            { Key.NumPad8, '8' },
            { Key.NumPad9, '9' }
        };

        public MainWindow()
        {
            InitializeComponent();
            textBox.Text = "0";
            textBox.Focus();
            textBox.SelectionStart = 1;
            ChangeButtonsState();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
                ChangeTextBoxContent(sender as Button);
            else
                throw new ArgumentException();
        }

        private void ChangeTextBoxContent(Button pressedBtn)
        {
            int currIndex = textBox.IsSelectionActive ? textBox.SelectionStart : textBox.Text.Length;
            string content = pressedBtn.Content.ToString();
            string answer = textBlock.Text;
            if (content == "=")
            {
                if (Calc.IsDoubleNumber(answer, numSystem))
                {
                    prevNumSystem = numSystem;
                    textBox.Text = Calc.ConvertToBase(answer, numSystem, prevNumSystem);
                    textBox.SelectionStart = answer.Length;
                }
                else
                {
                    try
                    {
                        string input = textBox.Text;
                        textBlock.Text = Calc.DoOperation(input, numSystem);
                    }
                    catch (Exception error)
                    {
                        textBlock.Text = error.Message.ToString();
                    }
                }
            }
            else if (content == "Clear")
            {
                textBox.Text = "0";
                textBlock.Text = "";
            }
            else if (content == "⌫" && currIndex > 0)
                textBox.Text = textBox.Text.Remove(currIndex - 1, 1);
            else if ((!charToKey.ContainsKey(pressedBtn.Content.ToString()[0]) || pressedBtn.Content.ToString() == "e^x")
                || CheckAvailableString(savedText, charToKey[pressedBtn.Content.ToString()[0]]))
            {
                textBlock.Text = "";
                if (textBox.Text == "0" && (content == "-" || content == "," || Calc.IsDoubleNumber(content, numSystem)))
                {
                    --currIndex;
                    textBox.Clear();
                }
                if (content.Length == 1 && (char.IsDigit(content[0]) || (content[0] >= 'A' && content[0] <= 'F')))
                    textBox.Text = textBox.Text.Insert(currIndex, content);
                else if (content == "/" || content == "*" || content == "-" || content == "+")
                    textBox.Text = textBox.Text.Insert(currIndex, content);
                else if (content == ",")
                {
                    if (textBox.Text.Length == 0 || !Calc.IsDoubleNumber(textBox.Text[textBox.Text.Length - 1].ToString(), numSystem))
                        textBox.Text = textBox.Text.Insert(currIndex++, "0");
                    textBox.Text = textBox.Text.Insert(currIndex, content);
                }
                else if (content == "1/x" || content == "sin(x)" || content == "cos(x)"
                    || content == "tg(x)" || content == "ctg(x)" || content == "|x|" || content == "ln(x)"
                    || content == "e^x" || content == "x^2" || content == "√x")
                {
                    if (Calc.IsDoubleNumber(textBox.Text, numSystem))
                        textBox.Text = content.Replace("x", textBox.Text);
                    else
                        textBlock.Text = "Во входных данных должно быть число";
                }
                else if (content == "div" || content == "mod")
                {
                    if (Calc.IsIntNumber(textBox.Text, numSystem))
                        textBox.Text = textBox.Text.Insert(currIndex, content);
                    else
                        textBlock.Text = "Указано не целое число";
                }
                else if (content == "or" || content == "and" || content == "xor")
                {
                    if (Calc.IsUIntNumber(textBox.Text, numSystem))
                        textBox.Text = textBox.Text.Insert(currIndex, content);
                    else
                        textBlock.Text = "Необходимо целое неотрицательное число";
                }
                else if (content == "x^y")
                {
                    content = content.Remove(2, 1);
                    if (Calc.IsDoubleNumber(textBox.Text, numSystem))
                        textBox.Text = content.Replace("x", textBox.Text);
                    else
                        textBlock.Text = "Во входных данных должно быть число";
                }
                else if (content == "n!")
                {
                    if (Calc.IsUIntNumber(textBox.Text, numSystem))
                        textBox.Text = textBox.Text.Insert(currIndex, "!");
                    else
                        textBlock.Text = "Нужно целое неотрицательное число";
                }
                else if (content == "π" || content == "e")
                    textBox.Text = textBox.Text.Insert(currIndex, content);
                else if (content == "±")
                {
                    if (Calc.IsDoubleNumber(textBox.Text, numSystem))
                    {
                        try
                        {
                            textBox.Text = Calc.DoOperation("±" + textBox.Text, numSystem);
                        }
                        catch (Exception error)
                        {
                            textBlock.Text = error.Message.ToString();
                        }
                    }
                    else
                        textBlock.Text = "Во входных данных должно быть число";
                }
            }

            if (content == "⌫" && currIndex > 0)
                textBox.SelectionStart = currIndex - 1;
            else if (content != "=")
            {
                currIndex += content.Length;
                if (content.IndexOf('x') != -1 && content.IndexOf('o') == -1)
                    --currIndex;
                if (content.IndexOf('(') != -1 || content.IndexOf('|') != -1)
                    --currIndex;
                textBox.SelectionStart = currIndex;
            }

            savedText = textBox.Text;
        }

        private void NumSystems_DropDownClosed(object sender, EventArgs e)
        {
            prevNumSystem = numSystem;
            textBlock.Text = "";

            if (Calc.IsDoubleNumber(textBox.Text, numSystem))
            {
                try
                {
                    textBlock.Text = Calc.RemovePointlessZeros(Calc.ConvertToBase(textBox.Text, numSystem, numSystemToInt[NumSystems.Text]));
                }
                catch (Exception error)
                {
                    textBlock.Text = error.Message.ToString();
                }
            }

            numSystem = numSystemToInt[NumSystems.Text];
            ChangeButtonsState();
        }

        private void ChangeButtonsState()
        {
            ((Button)LayoutRoot.Children.Cast<UIElement>()
                    .First(x => Grid.GetRow(x) == 5 && Grid.GetColumn(x) == 3)).Content = "x^2";
            switch (numSystem)
            {
                case 16:
                    ActivateDigits(new ButtonSwitchingCondition(ActivateAllDigits));
                    break;
                case 10:
                    ActivateDigits(new ButtonSwitchingCondition(ActivateDecDigits));
                    break;
                case 8:
                    ActivateDigits(new ButtonSwitchingCondition(ActivateOctDigits));
                    break;
                case 2:
                    ((Button)LayoutRoot.Children.Cast<UIElement>()
                    .First(x => Grid.GetRow(x) == 5 && Grid.GetColumn(x) == 3)).Content = "x^10";
                    ActivateDigits(new ButtonSwitchingCondition(ActivateBinDigits));
                    break;
            }
        }

        private bool ActivateAllDigits(Button btn)
        {
            return true;
        }

        private bool ActivateDecDigits(Button btn)
        {
            if (btn.Content.ToString().Length == 1)
            {
                if (char.IsDigit(Convert.ToString(btn.Content)[0])
                    || (Convert.ToString(btn.Content)[0] >= 'A' && Convert.ToString(btn.Content)[0] <= 'F'))
                    return Convert.ToString(btn.Content)[0] - 48 >= 0
                        && Convert.ToString(btn.Content)[0] - 48 <= 9;

                return true;
            }

            return true;
        }

        private bool ActivateOctDigits(Button btn)
        {
            if (btn.Content.ToString().Length == 1)
            {
                if (char.IsDigit(Convert.ToString(btn.Content)[0])
                    || (Convert.ToString(btn.Content)[0] >= 'A' && Convert.ToString(btn.Content)[0] <= 'F'))
                    return Convert.ToString(btn.Content)[0] - 48 >= 0
                        && Convert.ToString(btn.Content)[0] - 48 <= 7;

                return true;
            }

            return true;
        }

        private bool ActivateBinDigits(Button btn)
        {
            if (btn.Content.ToString().Length == 1)
            {
                if (char.IsDigit(Convert.ToString(btn.Content)[0])
                    || (Convert.ToString(btn.Content)[0] >= 'A' && Convert.ToString(btn.Content)[0] <= 'F'))
                    return Convert.ToString(btn.Content)[0] - 48 >= 0
                        && Convert.ToString(btn.Content)[0] - 48 <= 1;

                return true;
            }

            return true;
        }

        private void ActivateDigits(ButtonSwitchingCondition condition)
        {
            for (int i = 0; i < LayoutRoot.Children.Count; ++i)
            {
                if (LayoutRoot.Children[i] is Button)
                    LayoutRoot.Children[i].IsEnabled = condition(LayoutRoot.Children[i] as Button);
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ChangeTextBoxContent((Button)LayoutRoot.Children.Cast<UIElement>()
                    .First(x => Grid.GetRow(x) == 6 && Grid.GetColumn(x) == 8));
            }
            else if (e.Key == Key.Back)
                textBlock.Text = "";
            else if (sender is TextBox)
            {
                if (!CheckAvailableString((sender as TextBox).Text, e.Key))
                    e.Handled = true;
                else
                {
                    if (textBox.Text == "0")
                        textBox.Clear();
                    textBlock.Text = "";
                    if (e.Key == Key.P)
                    {
                        e.Handled = true;
                        textBox.Text = textBox.Text.Insert(textBox.SelectionStart, "π");
                        textBox.SelectionStart += 2;
                    }
                }
            }
            prevKey = e.Key;
        }

        private bool CheckAvailableString(string str, Key pressedKey)
        {
            if (str.Length != 0 && str[str.Length - 1] == ',')
                str = str.Insert(str.Length - 1, ",");

            char[] operations = { '!', '^', '*', '/', '-', '+', ',' };

            char operation = ' ';
            if (keyToChar.ContainsKey(pressedKey))
            {
                operation = keyToChar[pressedKey];
                if (prevKey == Key.LeftShift || prevKey == Key.RightShift || prevKey == Key.D1 || prevKey == Key.D6)
                {
                    if (keyToChar[pressedKey] == '1')
                        operation = '!';
                    else if (keyToChar[pressedKey] == '6')
                        operation = '^';
                }
            }
            else
                return false;

            try
            {
                if (textBox.SelectionStart > 1 && !IsNumber(pressedKey) && textBox.Text[textBox.SelectionStart - 1] == ',')
                    return false;
                Calc.DoOperation(str, numSystem);
                if ((textBox.SelectionStart == 0 && !IsLongOperations(str))
                    || (textBox.SelectionStart > 0 && textBox.SelectionStart < textBox.Text.Length
                    && (operations.Contains(str[textBox.SelectionStart - 1])
                    || operations.Contains(str[textBox.SelectionStart]))))
                    throw new Exception();
                if (Calc.IsDoubleNumber(str, numSystem))
                    return (str.IndexOf(',') == -1 || keyToChar[pressedKey] != ',') && pressedKey != Key.P && pressedKey != Key.E;
                else if ((textBox.SelectionStart > 0 && Calc.IsDoubleNumber(str[textBox.SelectionStart - 1].ToString(), numSystem))
                    || (textBox.Text.Length > textBox.SelectionStart && Calc.IsDoubleNumber(str[textBox.SelectionStart].ToString(), numSystem)))
                {
                    bool isNum = (IsNumber(pressedKey) && keyToChar[pressedKey] != 'e' && keyToChar[pressedKey] != 'π')
                        || (keyToChar[pressedKey] == ',' && str.Count(chr => chr == ',') <= 1);
                    return isNum;
                }
                else
                    return false;
            }
            catch
            {
                if (operations.Contains(keyToChar[pressedKey]) || operation == '!' || operation == '^')
                {
                    return keyToChar[pressedKey] == '-' ||
                        (textBox.SelectionStart > 0 && textBox.SelectionStart < textBox.Text.Length && str[textBox.SelectionStart] == '-'
                        && Calc.IsDoubleNumber(str[textBox.SelectionStart - 1].ToString(), numSystem))
                        || (str.Length > 0 && textBox.SelectionStart > 1
                        && !(str.Count(chr => chr == operation) == 2 || str[textBox.SelectionStart - 2] == operation)
                    && (str.Length == 1 || (!operations.Contains(str[textBox.SelectionStart - 1]) && !IsLongOperations(str))));
                }
                else
                    return true;
            }

        }

        private bool IsLongOperations(string str)
        {
            return str.Contains("xor") || str.Contains("and") || str.Contains("or") || str.Contains("sin") || str.Contains("cos")
                || str.Contains("tg") || str.Contains("ctg") || str.Contains("div") || str.Contains("mod") || str.Contains("ln")
                || str.Contains("|") || str.Contains("√");
        }

        private bool IsNumber(Key key)
        {
            return key == Key.NumPad0 || key == Key.NumPad1 || key == Key.NumPad2 || key == Key.NumPad3 ||
                key == Key.NumPad4 || key == Key.NumPad5 || key == Key.NumPad6 || key == Key.NumPad7 ||
                key == Key.NumPad8 || key == Key.NumPad9 || key == Key.D0 || (key == Key.D1 && prevKey != Key.LeftShift && prevKey != Key.RightShift) || key == Key.D2 ||
                key == Key.D3 || key == Key.D4 || key == Key.D5 || (key == Key.D6 && prevKey != Key.LeftShift && prevKey != Key.RightShift) || key == Key.D7 || key == Key.D8 ||
                key == Key.D9;
        }

    }
}
