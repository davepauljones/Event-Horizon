using FontAwesome.WPF;
using System;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Effects;

namespace Event_Horizon
{
    public class FunctionKeyManager
    {
        internal static int FunctionKeyBank = FunctionKeyBanks.Bank0;
        internal static int NumberOfFunctionKeyBanks = 1;

        internal static void ToggleFunctionKeyBank()
        {
            if (FunctionKeyBank < NumberOfFunctionKeyBanks)
            {
                FunctionKeyBank++;
                PopulateFunctionKeys(FunctionKeyBank);
            }
            else
                PopulateFunctionKeys(FunctionKeyBanks.Bank0);
        }

        internal static Button CreateButton(string buttonContentString, int width=60, int height=30)
        {
            Button button = new Button { Background = new SolidColorBrush(Colors.LightSlateGray) };

            button.Height = 60;
            button.SetResourceReference(Control.StyleProperty, "EventHorizonButtonStyle");
            //button.Margin = new System.Windows.Thickness(6, 0, 8.9, 0);
            button.Width = width;
            button.Height = height;

            StackPanel outerStackPanel = new StackPanel { Orientation = Orientation.Vertical };
            StackPanel innerStackPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };

            Label functionKeyLabelLabel = new Label { Content = buttonContentString, FontSize = 14, Margin = new System.Windows.Thickness(0, 0, 0, 0), Foreground = new SolidColorBrush(Colors.White), HorizontalAlignment = HorizontalAlignment.Center };

            outerStackPanel.Children.Add(innerStackPanel);
            outerStackPanel.Children.Add(functionKeyLabelLabel);
            button.Content = outerStackPanel;

            return button;
        }

        internal static StackPanel CreateSearchTextBox(string textBoxHeadingString, string textBoxTextString)
        {
            StackPanel outerStackPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Left };

            Label comboBoxHeadingLabel = new Label { Content = textBoxHeadingString, VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Medium, Margin = new Thickness(5, 8, 0, 0) };

            StackPanel innerStackPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 10, 0, 0) };

            Border border = new Border { CornerRadius = new CornerRadius(6), BorderBrush = new SolidColorBrush(Colors.DodgerBlue), Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new Thickness(1),  Width = 280, Margin = new Thickness(0,0,0,0) };

            Grid grid = new Grid();

            grid.Children.Add(border);

            Grid innerGrid = new Grid { Margin = new Thickness(0, 0, 0, 0) };

            Label textBoxTextLabel = new Label { Content = textBoxTextString, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center, FontSize = 18, FontWeight = FontWeights.Medium, Margin = new Thickness(6, 4, 4, 4), Width = 266, Height = 30 };

            innerGrid.Children.Add(textBoxTextLabel);

            grid.Children.Add(innerGrid);

            innerStackPanel.Children.Add(grid);

            outerStackPanel.Children.Add(comboBoxHeadingLabel);

            outerStackPanel.Children.Add(innerStackPanel);

            return outerStackPanel;
        }

        internal static StackPanel CreateEventTypeComboBox(string comboBoxHeadingString, EventType eventType)
        {
            StackPanel outerStackPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Left };
            
            Label comboBoxHeadingLabel = new Label { Content = comboBoxHeadingString, VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Medium, Margin = new Thickness(0, 8, 0, 0) };
            
            StackPanel innerStackPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 10, 0, 0) };
            
            ComboBox comboBox = new ComboBox { Background = new SolidColorBrush(Colors.Transparent), Width = 280, Height = 40 };
            comboBox.SetResourceReference(Control.StyleProperty, "FrequencyComboBoxStyle");
            comboBox.ItemContainerStyle = (Style)Application.Current.Resources["theComboBoxItem"];

            comboBox.SelectedItem = 0;

            Grid grid = new Grid();

            grid.Children.Add(comboBox);

            Grid innerGrid = new Grid { Margin = new Thickness(10, 4, 0, 0) };

            innerGrid.Children.Add(EventHorizonEventTypes.GetEventTypeStackPanel(eventType, true));

            grid.Children.Add(innerGrid);

            innerStackPanel.Children.Add(grid);

            outerStackPanel.Children.Add(comboBoxHeadingLabel);

            outerStackPanel.Children.Add(innerStackPanel);

            return outerStackPanel;
        }

        internal static Button CreateFunctionKey(string functionKeyNumberString, FontAwesomeIcon fontAwesomeIcon, string functionKeyLabelString)
        {
            Button button = new Button();

            button.Height = 60;
            button.SetResourceReference(Control.StyleProperty, "EventHorizonButtonStyle");
            button.Margin = new System.Windows.Thickness(10, 0, 8.9, 0);

            StackPanel outerStackPanel = new StackPanel { Orientation = Orientation.Vertical };
            StackPanel innerStackPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };
            Label functionKeyNumberLabel = new Label { Content = functionKeyNumberString, FontSize = 22, Margin = new System.Windows.Thickness(0,-4,0,0), Foreground = new SolidColorBrush(Colors.White), FontWeight = FontWeights.Black, HorizontalAlignment = HorizontalAlignment.Center };
            Border border = new Border { Margin = new Thickness(10, 4, 0, 0) };
            border.SetResourceReference(Control.StyleProperty, "EventTypeTokenFunctionKey_BorderStyle");
            FontAwesome.WPF.FontAwesome fa = new FontAwesome.WPF.FontAwesome { Icon = fontAwesomeIcon };
            fa.SetResourceReference(Control.StyleProperty, "EventTypeToken_FontAwesomeStyle");
            Label functionKeyLabelLabel = new Label { Content = functionKeyLabelString, FontSize = 14, Margin = new System.Windows.Thickness(0, -7, 0, 0), Foreground = new SolidColorBrush(Colors.White), HorizontalAlignment = HorizontalAlignment.Center };

            border.Child = fa;
            innerStackPanel.Children.Add(functionKeyNumberLabel);
            innerStackPanel.Children.Add(border);
            outerStackPanel.Children.Add(innerStackPanel);
            outerStackPanel.Children.Add(functionKeyLabelLabel);
            button.Content = outerStackPanel;

            return button;
        }
        internal static Button CreateHelpFunctionKey(string functionKeyNumberString, FontAwesomeIcon fontAwesomeIcon, string functionKeyLabelString, Color color)
        {
            Button button = new Button { Background = new SolidColorBrush(Colors.LightSlateGray) };

            button.Height = 60;
            button.SetResourceReference(Control.StyleProperty, "EventHorizonButtonStyle");
            button.Margin = new System.Windows.Thickness(6, 0, 8.9, 0);

            StackPanel outerStackPanel = new StackPanel { Orientation = Orientation.Vertical };
            StackPanel innerStackPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };
            
            Label functionKeyNumberLabel = new Label { Content = functionKeyNumberString, FontSize = 22, Margin = new System.Windows.Thickness(0, -4, 0, 0), Foreground = new SolidColorBrush(Colors.White), FontWeight = FontWeights.Black, HorizontalAlignment = HorizontalAlignment.Center };
            
            Border border = new Border { Margin = new Thickness(10, 4, 0, 0), Background = new SolidColorBrush(color) };
            border.SetResourceReference(Control.StyleProperty, "EventTypeTokenFunctionKeyHelpReport_BorderStyle");

            FontAwesome.WPF.FontAwesome fa = new FontAwesome.WPF.FontAwesome { Icon = fontAwesomeIcon, Margin = new Thickness(0,6,0,0) };
            fa.SetResourceReference(Control.StyleProperty, "EventTypeToken_FontAwesomeStyle");
            
            Label functionKeyLabelLabel = new Label { Content = functionKeyLabelString, FontSize = 14, Margin = new System.Windows.Thickness(0, -7, 0, 0), Foreground = new SolidColorBrush(Colors.White), HorizontalAlignment = HorizontalAlignment.Center };

            border.Child = fa;
            innerStackPanel.Children.Add(functionKeyNumberLabel);
            innerStackPanel.Children.Add(border);
            outerStackPanel.Children.Add(innerStackPanel);
            outerStackPanel.Children.Add(functionKeyLabelLabel);
            button.Content = outerStackPanel;

            return button;
        }

        internal static void GetEventTypeFromFunctionKey(int functionKey)
        {
            int functionKeyAssignedEvent = 0;

            switch (FunctionKeyBank)
            {
                case FunctionKeyBanks.Bank0:
                    functionKeyAssignedEvent = functionKey;
                    break;
                case FunctionKeyBanks.Bank1:
                    functionKeyAssignedEvent = functionKey + 12;
                    break;
                case FunctionKeyBanks.Bank2:
                    functionKeyAssignedEvent = functionKey + 24;
                    break;
                case FunctionKeyBanks.Bank3:
                    functionKeyAssignedEvent = functionKey + 36;
                    break;
                case FunctionKeyBanks.Bank4:
                    functionKeyAssignedEvent = functionKey + 48;
                    break;
                case FunctionKeyBanks.Bank5:
                    functionKeyAssignedEvent = functionKey + 60;
                    break;
                case FunctionKeyBanks.Bank6:
                    functionKeyAssignedEvent = functionKey + 72;
                    break;
                case FunctionKeyBanks.Bank7:
                    functionKeyAssignedEvent = functionKey + 84;
                    break;
                case FunctionKeyBanks.Bank8:
                    functionKeyAssignedEvent = functionKey + 96;
                    break;
                case FunctionKeyBanks.Bank9:
                    functionKeyAssignedEvent = functionKey + 108;
                    break;
            }

            MainWindow.mw.NewEventWindow(functionKeyAssignedEvent);
        }
        internal static void SetEventTypeFromFunctionKey(int functionKey)
        {
            int functionKeyAssignedEvent = 0;

            switch (FunctionKeyBank)
            {
                case FunctionKeyBanks.Bank0:
                    functionKeyAssignedEvent = functionKey;
                    break;
                case FunctionKeyBanks.Bank1:
                    functionKeyAssignedEvent = functionKey + 12;
                    break;
                case FunctionKeyBanks.Bank2:
                    functionKeyAssignedEvent = functionKey + 24;
                    break;
                case FunctionKeyBanks.Bank3:
                    functionKeyAssignedEvent = functionKey + 36;
                    break;
                case FunctionKeyBanks.Bank4:
                    functionKeyAssignedEvent = functionKey + 48;
                    break;
                case FunctionKeyBanks.Bank5:
                    functionKeyAssignedEvent = functionKey + 60;
                    break;
                case FunctionKeyBanks.Bank6:
                    functionKeyAssignedEvent = functionKey + 72;
                    break;
                case FunctionKeyBanks.Bank7:
                    functionKeyAssignedEvent = functionKey + 84;
                    break;
                case FunctionKeyBanks.Bank8:
                    functionKeyAssignedEvent = functionKey + 96;
                    break;
                case FunctionKeyBanks.Bank9:
                    functionKeyAssignedEvent = functionKey + 108;
                    break;
            }

            MainWindow.mw.EventTypeComboBox.SelectedIndex = functionKeyAssignedEvent;
        }

        internal static void LoadBank(int functionKey, EventType eventType)
        {
            switch (functionKey)
            {
                case 1:
                    MainWindow.mw.F1ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F1FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F1FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    MainWindow.mw.F1FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 2:
                    MainWindow.mw.F2ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F2FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F2FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    MainWindow.mw.F2FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 3:
                    MainWindow.mw.F3ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F3FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F3FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    MainWindow.mw.F3FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 4:
                    MainWindow.mw.F4ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F4FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F4FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    MainWindow.mw.F4FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 5:
                    MainWindow.mw.F5ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F5FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F5FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    MainWindow.mw.F5FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 6:
                    MainWindow.mw.F6ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F6FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F6FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    MainWindow.mw.F6FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 7:
                    MainWindow.mw.F7ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F7FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F7FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    MainWindow.mw.F7FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 8:
                    MainWindow.mw.F8ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F8FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F8FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    MainWindow.mw.F8FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 9:
                    MainWindow.mw.F9ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F9FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F9FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    MainWindow.mw.F9FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 10:
                    MainWindow.mw.F10ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F10FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F10FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    MainWindow.mw.F10FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 11:
                    MainWindow.mw.F11ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F11FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F11FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    MainWindow.mw.F11FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 12:
                    MainWindow.mw.F12ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F12FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F12FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    MainWindow.mw.F12FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
        }

        internal static void PopulateFunctionKeys(int bank)
        {
            int functionKey = 0;

            string DefaultEventTypeName = "";
            FontAwesomeIcon DefaultFontAwesomeIcon = FontAwesomeIcon.None;
            Color DefaultIconBorderBackground = Colors.Transparent;

            MainWindow.mw.F1ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F1FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F1FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);
            MainWindow.mw.F1FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Hidden;

            MainWindow.mw.F2ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F2FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F2FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);
            MainWindow.mw.F2FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Hidden;

            MainWindow.mw.F3ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F3FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F3FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);
            MainWindow.mw.F3FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Hidden;

            MainWindow.mw.F4ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F4FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F4FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);
            MainWindow.mw.F4FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Hidden;

            MainWindow.mw.F5ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F5FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F5FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);
            MainWindow.mw.F5FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Hidden;

            MainWindow.mw.F6ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F6FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F6FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);
            MainWindow.mw.F6FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Hidden;

            MainWindow.mw.F7ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F7FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F7FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);
            MainWindow.mw.F7FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Hidden;

            MainWindow.mw.F8ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F8FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F8FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);
            MainWindow.mw.F8FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Hidden;

            MainWindow.mw.F9ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F9FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F9FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);
            MainWindow.mw.F9FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Hidden;

            MainWindow.mw.F10ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F10FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F10FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);
            MainWindow.mw.F10FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Hidden;

            MainWindow.mw.F11ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F11FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F11FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);
            MainWindow.mw.F11FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Hidden;

            MainWindow.mw.F12ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F12FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F12FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);
            MainWindow.mw.F12FontAwesomeIconBorder.Visibility = System.Windows.Visibility.Hidden;

            switch (bank)
            {
                case FunctionKeyBanks.Bank0:
                    FunctionKeyBank = FunctionKeyBanks.Bank0;

                    foreach (EventType eventType in XMLReaderWriter.EventTypesList)
                    {
                        if (functionKey >= 1 && functionKey <= 12) LoadBank(functionKey, eventType);
                        functionKey++;
                    }
                    break;
                case FunctionKeyBanks.Bank1:
                    FunctionKeyBank = FunctionKeyBanks.Bank1;

                    foreach (EventType eventType in XMLReaderWriter.EventTypesList)
                    {
                        if (functionKey >= 13 && functionKey <= 24) LoadBank(functionKey-12, eventType);
                        functionKey++;
                    }
                    break;
                case FunctionKeyBanks.Bank2:
                    FunctionKeyBank = FunctionKeyBanks.Bank2;

                    foreach (EventType eventType in XMLReaderWriter.EventTypesList)
                    {
                        if (functionKey >= 25 && functionKey <= 36) LoadBank(functionKey-24, eventType);
                        functionKey++;
                    }
                    break;
                case FunctionKeyBanks.Bank3:
                    FunctionKeyBank = FunctionKeyBanks.Bank3;

                    foreach (EventType eventType in XMLReaderWriter.EventTypesList)
                    {
                        if (functionKey >= 37 && functionKey <= 48) LoadBank(functionKey-36, eventType);
                        functionKey++;
                    }
                    break;
                case FunctionKeyBanks.Bank4:
                    FunctionKeyBank = FunctionKeyBanks.Bank4;

                    foreach (EventType eventType in XMLReaderWriter.EventTypesList)
                    {
                        if (functionKey >= 49 && functionKey <= 60) LoadBank(functionKey-48, eventType);
                        functionKey++;
                    }
                    break;
                case FunctionKeyBanks.Bank5:
                    FunctionKeyBank = FunctionKeyBanks.Bank5;

                    foreach (EventType eventType in XMLReaderWriter.EventTypesList)
                    {
                        if (functionKey >= 61 && functionKey <= 72) LoadBank(functionKey-60, eventType);
                        functionKey++;
                    }
                    break;
                case FunctionKeyBanks.Bank6:
                    FunctionKeyBank = FunctionKeyBanks.Bank6;

                    foreach (EventType eventType in XMLReaderWriter.EventTypesList)
                    {
                        if (functionKey >= 73 && functionKey <= 84) LoadBank(functionKey-72, eventType);
                        functionKey++;
                    }
                    break;
                case FunctionKeyBanks.Bank7:
                    FunctionKeyBank = FunctionKeyBanks.Bank7;

                    foreach (EventType eventType in XMLReaderWriter.EventTypesList)
                    {
                        if (functionKey >= 85 && functionKey <= 96) LoadBank(functionKey-84, eventType);
                        functionKey++;
                    }
                    break;
                case FunctionKeyBanks.Bank8:
                    FunctionKeyBank = FunctionKeyBanks.Bank8;

                    foreach (EventType eventType in XMLReaderWriter.EventTypesList)
                    {
                        if (functionKey >= 97 && functionKey <= 108) LoadBank(functionKey-96, eventType);
                        functionKey++;
                    }
                    break;
                case FunctionKeyBanks.Bank9:
                    FunctionKeyBank = FunctionKeyBanks.Bank9;

                    foreach (EventType eventType in XMLReaderWriter.EventTypesList)
                    {
                        if (functionKey >= 109 && functionKey <= 120) LoadBank(functionKey-108, eventType);
                        functionKey++;
                    }
                    break;
            }
        }
    }
}