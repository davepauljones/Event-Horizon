﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FontAwesome.WPF;
using System.Windows.Media;

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
                    break;
                case 2:
                    MainWindow.mw.F2ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F2FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F2FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    break;
                case 3:
                    MainWindow.mw.F3ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F3FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F3FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    break;
                case 4:
                    MainWindow.mw.F4ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F4FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F4FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    break;
                case 5:
                    MainWindow.mw.F5ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F5FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F5FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    break;
                case 6:
                    MainWindow.mw.F6ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F6FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F6FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    break;
                case 7:
                    MainWindow.mw.F7ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F7FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F7FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    break;
                case 8:
                    MainWindow.mw.F8ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F8FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F8FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    break;
                case 9:
                    MainWindow.mw.F9ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F9FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F9FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    break;
                case 10:
                    MainWindow.mw.F10ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F10FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F10FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    break;
                case 11:
                    MainWindow.mw.F11ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F11FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F11FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    break;
                case 12:
                    MainWindow.mw.F12ButtonLabel.Content = eventType.Name;
                    MainWindow.mw.F12FontAwesomeIcon.Icon = eventType.Icon;
                    MainWindow.mw.F12FontAwesomeIconBorder.Background = new SolidColorBrush(eventType.Color);
                    break;
            }
        }

        internal static void PopulateFunctionKeys(int bank)
        {
            int functionKey = 0;

            string DefaultEventTypeName = "Spare";
            FontAwesomeIcon DefaultFontAwesomeIcon = FontAwesomeIcon.CircleOutline;
            Color DefaultIconBorderBackground = Colors.DodgerBlue;

            MainWindow.mw.F1ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F1FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F1FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);

            MainWindow.mw.F2ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F2FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F2FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);

            MainWindow.mw.F3ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F3FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F3FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);

            MainWindow.mw.F4ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F4FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F4FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);

            MainWindow.mw.F5ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F5FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F5FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);

            MainWindow.mw.F6ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F6FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F6FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);

            MainWindow.mw.F7ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F7FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F7FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);

            MainWindow.mw.F8ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F8FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F8FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);

            MainWindow.mw.F9ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F9FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F9FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);

            MainWindow.mw.F10ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F10FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F10FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);

            MainWindow.mw.F11ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F11FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F11FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);

            MainWindow.mw.F12ButtonLabel.Content = DefaultEventTypeName;
            MainWindow.mw.F12FontAwesomeIcon.Icon = DefaultFontAwesomeIcon;
            MainWindow.mw.F12FontAwesomeIconBorder.Background = new SolidColorBrush(DefaultIconBorderBackground);

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