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
using System.Windows.Threading;

namespace snake
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        List<Point> snakePoints = new List<Point>();
        DispatcherTimer timer;
        Rect foodRect;
        Rect snakeRect;
        Ellipse food;
        Random rnd;
        int stepX = 0;
        int stepY = 0;
        const int size = 16;
        bool bl = false;
        int kol = 0;
        int length = 100;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rnd = new Random(); //форма еды
            foodRect = new Rect(rnd.Next(size, (int)Map.ActualWidth - size), rnd.Next(size, (int)Map.ActualHeight - size), size, size);
            food = CreateFood(foodRect, Brushes.Red);
            Map.Children.Add(food);//таймер еды
            Kol.Text = Convert.ToString(kol);
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick; // событие таймера
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10); //интервал
            timer.IsEnabled = true; // включаем таймер
            snakeRect = new Rect(rnd.Next(size, (int)Map.ActualWidth - size), rnd.Next(size, (int)Map.ActualHeight - size), size, size);
            CreateSnake(snakeRect, Brushes.Green); // создание снейка
        }

        private void CreateSnake(Rect snakeRect, SolidColorBrush red)
        {
            //снаке
            Ellipse newEllipse = new Ellipse();
            newEllipse.Fill = red;
            newEllipse.Width = size;
            newEllipse.Height = size;
            Canvas.SetTop(newEllipse, snakeRect.Y);
            Canvas.SetLeft(newEllipse, snakeRect.X);
            int count = Map.Children.Count;
            Map.Children.Add(newEllipse);
            Point point = new Point(snakeRect.X, snakeRect.Y);
            snakePoints.Add(point);
            //ограничение хвоста
            if (count > length)
            {
                Map.Children.RemoveAt(1);
                snakePoints.RemoveAt(0);
            }
        }

        private Ellipse CreateFood(Rect foodRect, SolidColorBrush red)
        {
            // создание еды
            Ellipse ellipse = new Ellipse();
            ellipse.Width = foodRect.Width;
            ellipse.Height = foodRect.Height;
            ellipse.Fill = red;
            Canvas.SetLeft(ellipse, foodRect.X);
            Canvas.SetTop(ellipse, foodRect.Y);
            return ellipse;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Move();
            CreateSnake(snakeRect, Brushes.Green);
            GameOver();
            if (snakeRect.IntersectsWith(foodRect) == true)
            {
                kol++;
                Kol.Text = Convert.ToString(kol);
                length += 10;
                MoveFood();
            }
        }

        private void GameOver()
        {
            MessageBoxResult result;
            if (bl == true)
            {
                timer.IsEnabled = false;
                result = MessageBox.Show("Новая игра", "Гаме овер", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    //придумать перезапуск
                    MainWindow main = new MainWindow();
                    this.Close();
                    main.ShowDialog();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void MoveFood()
        {
            //генерируем точку еды
            foodRect = new Rect(rnd.Next(size, (int)Map.ActualWidth - size), rnd.Next(size, (int)Map.ActualHeight - size), size, size);
            Canvas.SetLeft(food, foodRect.X);
            Canvas.SetTop(food, foodRect.Y);
        }

        void Move()
        {
           //перемещение змii
            snakeRect.X += stepX;
            snakeRect.Y += stepY;
            if (snakeRect.X < 0 || snakeRect.X > Map.ActualWidth) { bl = true; }
            if (snakeRect.Y < 0 || snakeRect.Y > Map.ActualHeight) { bl = true; }
            CreateSnake(snakeRect, Brushes.Green);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //просто кнопочки
            if (e.Key == Key.Up)
            {
                stepY = -1;
                stepX = 0;

            }
            if (e.Key == Key.Down)
            {
                stepY = 1;
                stepX = 0;

            }
            if (e.Key == Key.Left)
            {
                stepY = 0;
                stepX = -1;

            }
            if (e.Key == Key.Right)
            {
                stepY = 0;
                stepX = 1;

            }
        }
    }
}
