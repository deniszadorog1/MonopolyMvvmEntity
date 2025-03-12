using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Timers;
using System.Threading;
using Timer = System.Timers.Timer;
using System.Windows.Media;
using MonopolyEntity.Windows.Pages;

namespace MonopolyEntity.Windows.UserControls.GameControls.Other
{
    /// <summary>
    /// Логика взаимодействия для UserTimer.xaml
    /// </summary>
    public partial class UserTimer : UserControl
    {
        Frame _frame = null;
        public UserTimer()
        {
            InitializeComponent();     
        }

        public void SetFrame(Frame frame)
        {
            _frame = frame;
        }

        private const int _timeForMove = 75;
        public int _timeLeft = 0;
        public Timer _timer = null;

        public void SetTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Elapsed -= TimerElapsed; 
                _timer.Dispose();
                _timer = null;
            }

            const int interval = 1000;
            _timeLeft = _timeForMove;

            _timer = new Timer(interval);
            _timer.Elapsed -= TimerElapsed;
            _timer.Elapsed += TimerElapsed;
            _timer.AutoReset = true;
            _timer.Start();

            TimerTextBlock.Text = _timeLeft.ToString();
        }

        public void ToStopTimer()
        {

        }

        public void UpdateTimeOnTimer()
        {
            if (_timer is null) return;
            SetTimer();
            return;

            _timeLeft = _timeForMove;
            TimerTextBlock.Text = _timeLeft.ToString();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_timeLeft > 0)
            {
                _timeLeft--;
            }
            else if(!(_timer is null))
            {
                _timer.Stop();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (_frame.Content is GamePage game)
                    {
                        game._field.PlayerGaveUp(null);
                    }
                });
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                TimerTextBlock.Text = _timeLeft.ToString();
            });
        }

        public void StopTimer()
        {
            if (_timer is null) return;
            _timer.Stop();
            _timer = null;
        }
    }
}
