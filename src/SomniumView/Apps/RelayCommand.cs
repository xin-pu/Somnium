﻿using System;
using System.Windows.Input;

namespace SomniumView.Apps
{
    public class RelayCommand : ICommand
    {
        #region

        private readonly Func<bool> _canExecute;
        private readonly Action _execute;
        #endregion

        public RelayCommand(Action execute) : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }


        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() != false;
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }


    }
}