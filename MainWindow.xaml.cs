using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace Space_Invaders
{
    public partial class MainWindow : Window
    {
        bool goLeft, goRight = false;
        List<Rectangle> itemstoremove = new List<Rectangle>();
        int enemyImages = 0;
        int bulletTimer;
        int bulletTimerLimit = 90;
        int totalEnemeis;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        ImageBrush playerSkin = new ImageBrush();
        int enemySpeed = 6;

        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += gameEngine;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20);
            dispatcherTimer.Start();
            playerSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/shooter.png"));
            player1.Fill = playerSkin;
            makeEnemies(30);
        }

        private void Canvas_KeyisDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = true;
            }
            if (e.Key == Key.Right)
            {
                goRight = true;
            }
        }

        private void Canvas_KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = false;
            }
            if (e.Key == Key.Right)
            {
                goRight = false;
            }
            if (e.Key == Key.Space)
            {
                itemstoremove.Clear();
                Rectangle newBullet = new Rectangle
                {
                    Tag = "bullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Red
                };
                Canvas.SetTop(newBullet, Canvas.GetTop(player1) - newBullet.Height);
                Canvas.SetLeft(newBullet, Canvas.GetLeft(player1) + player1.Width / 2);
                myCanvas.Children.Add(newBullet);
            }
        }

        private void enemyBulletMaker(double x, double y)
        {
            Rectangle newEnemyBullet = new Rectangle
            {
                Tag = "enemyBullet",
                Height = 40,
                Width = 15,
                Fill = Brushes.Yellow,
                Stroke = Brushes.Black,
                StrokeThickness = 5
            };
            // now we place the bullets top location to the Y variable
            Canvas.SetTop(newEnemyBullet, y);
            // set the left location to the X location
            Canvas.SetLeft(newEnemyBullet, x);
            // add the bullet to the screen
            myCanvas.Children.Add(newEnemyBullet);
        }

        private void makeEnemies(int limit)
        {
            int left = 0;
            totalEnemeis = limit;
            for (int i = 0; i < limit; i++)
            {
                ImageBrush enemySkin = new ImageBrush();
                Rectangle newEnemy = new Rectangle
                {
                    Tag = "enemy",
                    Height = 45,
                    Width = 45,
                    Fill = enemySkin,
                };
                Canvas.SetTop(newEnemy, 10);
                Canvas.SetLeft(newEnemy, left);
                myCanvas.Children.Add(newEnemy);
                left -= 60;
                enemyImages++;
                if (enemyImages > 8)
                {
                    enemyImages = 1;
                }
                switch (enemyImages)
                {
                    case 1:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/1zomb.png"));
                        break;
                    case 2:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/2zomb.png"));
                        break;
                    case 3:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/3zomb.png"));
                        break;
                    case 4:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/4zomb.png"));
                        break;
                    case 5:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/5zomb.png"));
                        break;
                    case 6:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/6zomb.png"));
                        break;
                    case 7:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/7zomb.png"));
                        break;
                    case 8:
                        enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/8zomb.png"));
                        break;               
                    }
                }
            }

        private void gameEngine(object sender, EventArgs e)
        {
            Rect player = new Rect(Canvas.GetLeft(player1), Canvas.GetTop(player1), player1.Width, player1.Height);
            enemiesLeft.Content = "Invaders Left: " + totalEnemeis;
            if (goLeft && Canvas.GetLeft(player1) > 0)
            {
                Canvas.SetLeft(player1, Canvas.GetLeft(player1) - 10);
            }
            else if (goRight && Canvas.GetLeft(player1) + 80 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player1, Canvas.GetLeft(player1) + 10);
            }
            bulletTimer -= 3;
            if (bulletTimer < 0)
            {
                enemyBulletMaker((Canvas.GetLeft(player1) + 20), 10);
                bulletTimer = bulletTimerLimit;
            }
            if (totalEnemeis < 10)
            {
                enemySpeed = 20;
            }
            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);
                    Rect bullet = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Canvas.GetTop(x) < 10)
                    {
                        itemstoremove.Add(x);
                    }
                    foreach (var y in myCanvas.Children.OfType<Rectangle>())
                    {
                        if (y is Rectangle && (string)y.Tag == "enemy")
                        {
                            Rect enemy = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (bullet.IntersectsWith(enemy))
                            {
                                itemstoremove.Add(x);
                                itemstoremove.Add(y);
                                totalEnemeis -= 1;
                            }
                        }
                    }
                }
                if (x is Rectangle && (string)x.Tag == "enemy")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) + enemySpeed);
                    if (Canvas.GetLeft(x) > 820)
                    {
                        Canvas.SetLeft(x, -80);
                        Canvas.SetTop(x, Canvas.GetTop(x) + (x.Height + 10));
                    }
                    Rect enemy = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (player.IntersectsWith(enemy))
                    {
                        dispatcherTimer.Stop();
                        MessageBox.Show("you lose");
                    }
                }
                if (x is Rectangle && (string)x.Tag == "enemyBullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 10);
                    if (Canvas.GetTop(x) > 480)
                    {
                        itemstoremove.Add(x);
                    }
                    Rect enemyBullets = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (enemyBullets.IntersectsWith(player))
                    {
                        dispatcherTimer.Stop();
                        MessageBox.Show("you lose");
                    }
                }
            }
            foreach (Rectangle y in itemstoremove)
            {
                myCanvas.Children.Remove(y);
            }
            if (totalEnemeis < 1)
            {
                dispatcherTimer.Stop();
                MessageBox.Show("you win");
            }
        }
    }
}
