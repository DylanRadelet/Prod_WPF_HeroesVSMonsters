using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Heroes_VS_Monsters.Class;
using Heroes_VS_Monsters.Class.Heros;
using Heroes_VS_Monsters.Class.Monstre;
using Prod_WPF_HeroesVSMonsters;

namespace HeroesVSMonsters
{
    public partial class MainWindow : Window
    {
        #region INITIALISATION 
        public int CurrentLevel { get; set; } = 1;

        public List<Rectangle> Buissons { get; set; } = new List<Rectangle>();

        public Heros Hero { get; set; } = new Heros("Arthur", 11, 5, 95, 0, 0, 0);

        public List<Monstre> Monstres { get; set; } = new List<Monstre>
        {
            new Orc("Orc", 8, 3, 50, 1),
            new Loup("Loup", 5, 5, 40, 1),
            new Dragonnet("Dragonnet", 8, 4, 60, 1)
        };

        public Dictionary<Monstre, (Image, Rectangle)> MonstreRectangles { get; set; } = new Dictionary<Monstre, (Image, Rectangle)>();

        private GameImage GoldImage;
        private GameImage CuirImage;
        private GameImage VieImage;

        int Achat = 2;

        private List<BitmapImage> playerUpSteps;
        private List<BitmapImage> playerDownSteps;
        private List<BitmapImage> playerLeftSteps;
        private List<BitmapImage> playerRightSteps;
        private BitmapImage playerIdle;

        private BitmapImage orcImage;
        private BitmapImage loupImage;
        private BitmapImage dragonnetImage;

        private int animationIndex = 0;

        bool stop;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
            DataContext = Hero;

            #region IMAGES
            playerUpSteps = new List<BitmapImage>
            {
                new BitmapImage(new Uri("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\Personnage\\Dos\\Dos_step_1-removebg-preview.png")),
                new BitmapImage(new Uri("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\Personnage\\Dos\\Dos_step_2-removebg-preview.png")),
                new BitmapImage(new Uri("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\Personnage\\Dos\\Dos_step_3-removebg-preview.png"))
            };
                    playerDownSteps = new List<BitmapImage>
            {
                new BitmapImage(new Uri("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\Personnage\\Face\\Face_step_1-removebg-preview.png")),
                new BitmapImage(new Uri("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\Personnage\\Face\\Face_step_2-removebg-preview.png")),
                new BitmapImage(new Uri("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\Personnage\\Face\\Face_step_3-removebg-preview.png"))
            };
                    playerLeftSteps = new List<BitmapImage>
            {
                new BitmapImage(new Uri("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\Personnage\\Gauche\\Gauche_step_1-removebg-preview.png")),
                new BitmapImage(new Uri("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\Personnage\\Gauche\\Gauche_step_2-removebg-preview.png")),
                new BitmapImage(new Uri("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\Personnage\\Gauche\\Gauche_step_3-removebg-preview.png"))
            };
                    playerRightSteps = new List<BitmapImage>
            {
                new BitmapImage(new Uri("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\Personnage\\Droite\\Droit_step_1-removebg-preview.png")),
                new BitmapImage(new Uri("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\Personnage\\Droite\\Droit_step_2-removebg-preview.png")),
                new BitmapImage(new Uri("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\Personnage\\Droite\\Droit_step_3-removebg-preview.png"))
            };

            playerIdle = new BitmapImage(new Uri("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\Personnage\\Face\\Face_step_2-removebg-preview.png"));

            Player.Source = playerIdle;

            orcImage = new BitmapImage(new Uri("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\Personnage\\Monstre\\orc.png"));
            loupImage = new BitmapImage(new Uri("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\Personnage\\Monstre\\loup.png"));
            dragonnetImage = new BitmapImage(new Uri("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\Personnage\\Monstre\\dragon.png"));

            GoldImage = new GameImage("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\gold.png", 300, 400, 350, 150, 1001);
            Map.Children.Add(GoldImage.ImageControl);
            GoldImage.Hide();

            CuirImage = new GameImage("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\cuir.png", 300, 400, 350, 150, 1001);
            Map.Children.Add(CuirImage.ImageControl);
            CuirImage.Hide();

            VieImage = new GameImage("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\PointDeVie.png", 300, 400, 350, 150, 1001);
            Map.Children.Add(VieImage.ImageControl);
            VieImage.Hide();

            Canvas.SetZIndex(Player, 1000);
            #endregion

            CreationMontres(CurrentLevel);
            InitializeBuissons();
        }

        #region JEU

        #region MONSTRE
        public void MortMonstre(Monstre monstre)
        {
            if (MonstreRectangles.TryGetValue(monstre, out var pair))
            {
                Map.Children.Remove(pair.Item1);
                Map.Children.Remove(pair.Item2);
                MonstreRectangles.Remove(monstre);
            }
        }

        public void CreationMontres(int level)
        {
            Random random = new Random();
            for (int i = 0; i < level + 2; i++)
            {
                Monstre monst = (i % 3 == 0) ? new Orc("Orc", 8 + level, 3 + level, 80 + (level * 10), 1) :
                                (i % 3 == 1) ? new Loup("Loup", 7 + level, 7 + level, 50 + (level * 10), 1) :
                                new Dragonnet("Dragonnet", 8 + level, 4 + level, 90 + (level * 10), 1);

                Image img = new Image();
                Rectangle rect = new Rectangle
                {
                    Stroke = Brushes.Transparent,
                    Fill = Brushes.Transparent
                };

                switch (monst.Race)
                {
                    case "Orc":
                        img.Width = 75;
                        img.Height = 75;
                        img.Source = orcImage;
                        rect.Width = 50;
                        rect.Height = 50;
                        break;
                    case "Loup":
                        img.Width = 50;
                        img.Height = 50;
                        img.Source = loupImage;
                        rect.Width = 30;
                        rect.Height = 30;
                        break;
                    case "Dragonnet":
                        img.Width = 100;
                        img.Height = 100;
                        img.Source = dragonnetImage;
                        rect.Width = 80;
                        rect.Height = 80;
                        break;
                    default:
                        img.Width = 50;
                        img.Height = 50;
                        rect.Width = 30;
                        rect.Height = 30;
                        break;
                }

                double left, top;
                bool collision;

                do
                {
                    left = random.Next(10, (int)(1010 - img.Width));
                    top = random.Next(10, (int)(690 - img.Height));

                    Canvas.SetLeft(img, left);
                    Canvas.SetTop(img, top);

                    Canvas.SetLeft(rect, left + (img.Width - rect.Width) / 2);
                    Canvas.SetTop(rect, top + (img.Height - rect.Height) / 2);

                    collision = false;

                    foreach (var pair in MonstreRectangles)
                    {
                        Rect existingMonsterRect = new Rect(Canvas.GetLeft(pair.Value.Item2), Canvas.GetTop(pair.Value.Item2), pair.Value.Item2.Width, pair.Value.Item2.Height);
                        Rect newMonsterRect = new Rect(Canvas.GetLeft(rect), Canvas.GetTop(rect), rect.Width, rect.Height);

                        if (existingMonsterRect.IntersectsWith(newMonsterRect))
                        {
                            collision = true;
                            break;
                        }
                    }

                    if (!collision)
                    {
                        foreach (var buisson in Buissons)
                        {
                            Rect buissonRect = new Rect(Canvas.GetLeft(buisson), Canvas.GetTop(buisson), buisson.Width, buisson.Height);
                            Rect newMonsterRect = new Rect(Canvas.GetLeft(rect), Canvas.GetTop(rect), rect.Width, rect.Height);

                            if (buissonRect.IntersectsWith(newMonsterRect))
                            {
                                collision = true;
                                break;
                            }
                        }
                    }

                    if (!collision)
                    {
                        Rect shopRect = new Rect(Canvas.GetLeft(Shop), Canvas.GetTop(Shop), Shop.Width, Shop.Height);
                        Rect newMonsterRect = new Rect(Canvas.GetLeft(rect), Canvas.GetTop(rect), rect.Width, rect.Height);

                        if (shopRect.IntersectsWith(newMonsterRect))
                        {
                            collision = true;
                        }
                    }
                } while (collision);

                Canvas.SetZIndex(img, 2);
                Canvas.SetZIndex(rect, 1);

                Map.Children.Add(rect);
                Map.Children.Add(img);

                MonstreRectangles[monst] = (img, rect);
            }
        }

        private bool CollisionMonstre(double newX, double newY)
        {
            Rect playerRect = new Rect(newX, newY, Player.Width, Player.Height);

            foreach (var pair in MonstreRectangles)
            {
                Rect monsterRect = new Rect(Canvas.GetLeft(pair.Value.Item2), Canvas.GetTop(pair.Value.Item2), pair.Value.Item2.Width, pair.Value.Item2.Height);
                if (playerRect.IntersectsWith(monsterRect))
                {
                    Hero.Pdv += 0;
                    StartCombat(Hero, pair.Key);
                    Hero.Pdv += 0;
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region COLLISION
        private bool EnCollision(FrameworkElement a, FrameworkElement b)
        {
            double aLeft = Canvas.GetLeft(a);
            double aTop = Canvas.GetTop(a);
            double aRight = aLeft + a.Width;
            double aBottom = aTop + a.Height;

            double bLeft = Canvas.GetLeft(b);
            double bTop = Canvas.GetTop(b);
            double bRight = bLeft + b.Width;
            double bBottom = bTop + b.Height;

            return (aLeft < bRight && aRight > bLeft && aTop < bBottom && aBottom > bTop);
        }

        private bool Collision(double newX, double newY)
        {
            if (newX < 5 || newX + Player.Width > Map.ActualWidth - 5 ||
                newY < 5 || newY + Player.Height > Map.ActualHeight - 5)
            {
                Hero.Pdv += 0;
                return true;
            }
            return false;
        }
        #endregion

        #region DEPLACEMENT
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            double newX = Canvas.GetLeft(Player);
            double newY = Canvas.GetTop(Player);

            if (stop == false)
            {
                switch (e.Key)
                {
                    case Key.Up:
                        newY -= 5;
                        Player.Source = playerUpSteps[animationIndex];
                        break;
                    case Key.Down:
                        newY += 5;
                        Player.Source = playerDownSteps[animationIndex];
                        break;
                    case Key.Left:
                        newX -= 5;
                        Player.Source = playerLeftSteps[animationIndex];
                        break;
                    case Key.Right:
                        newX += 5;
                        Player.Source = playerRightSteps[animationIndex];
                        break;
                }
            }
            else
            {
                Player.Source = playerIdle;
            }
            
            #region ImageVisible
            if (e.Key == Key.Space && GoldImage.IsVisible)
            {
                Hero.Pdv += 0;
                GoldImage.Hide();
                stop = false;
            }
            if (e.Key == Key.Space && CuirImage.IsVisible)
            {
                Hero.Pdv += 0;
                CuirImage.Hide();
                stop = false;
            }
            if (e.Key == Key.Space && VieImage.IsVisible)
            {
                Hero.Pdv += 0;
                VieImage.Hide();
                stop = false;
            }
            #endregion

            if (!Collision(newX, newY))
            {
                if (!CollisionMonstre(newX, newY))
                {
                    if (!CheckCollisionWithBuissons(newX, newY))
                    {
                        if (!ShopCollision(newX, newY))
                        {
                            Hero.Pdv += 0;
                            Canvas.SetLeft(Player, newX);
                            Canvas.SetTop(Player, newY);
                            Canvas.SetZIndex(Player, 10);
                        }
                    }
                }
            }

            animationIndex = (animationIndex + 1) % 3;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Player.Source = playerIdle;
        }
        #endregion

        #region LEVEL
        private void CheckLevel()
        {
            if (MonstreRectangles.Count == 0)
            {
                CurrentLevel++;
                Achat = 2;
                CreationMontres(CurrentLevel);
                MessageBox.Show($"Bienvenue au niveau {CurrentLevel}!");
                if (CurrentLevel < 3)
                {
                    Hero.Gold += 10 + CurrentLevel;
                    Hero.Cuir += 10 + CurrentLevel;
                    Hero.Pdv += CurrentLevel * 25;
                }
                else if (CurrentLevel > 3 && CurrentLevel < 6)
                {
                    Hero.Gold += 15 + CurrentLevel;
                    Hero.Cuir += 15 + CurrentLevel;
                }
                else if (CurrentLevel > 6 && CurrentLevel < 9)
                {
                    Hero.Gold += 20 + CurrentLevel;
                    Hero.Cuir += 20 + CurrentLevel;
                    Hero.Pdv += CurrentLevel * 10;
                }
            }
        }
        #endregion

        #region SHOP
        public bool ShopCollision(double newX, double newY)
        {
            Rect playerRect = new Rect(newX, newY, Player.Width, Player.Height);

            Rect shopRect = new Rect(10, 5, 105, 60);

            if (playerRect.IntersectsWith(shopRect))
            {
                Hero.Pdv += 0;
                ShopWindow Shop = new ShopWindow(Hero, CurrentLevel, Achat);
                Shop.Show();
                return true;
            }
            return false;
        }
        #endregion

        #region BUISSONS
        public void InitializeBuissons()
        {
            Buissons.Add(CreateBuisson(134, 402, 842, 304, 1)); 
            Buissons.Add(CreateBuisson(875, 604, 100, 102, 1));
            Buissons.Add(CreateBuisson(681, 91, 294, 616, 1));
            Buissons.Add(CreateBuisson(708, 360, 267, 347, 1));
            Buissons.Add(CreateBuisson(383, 578, 593, 127, 1));
            Buissons.Add(CreateBuisson(245, 238, 731, 469, 1));

            foreach (var buisson in Buissons)
            {
                Map.Children.Add(buisson);
            }
        }

        public Rectangle CreateBuisson(int left, int top, int right, int bottom, int zIndex)
        {
            var color = Color.FromRgb(0x3A, 0x9D, 0x23);

            var rect = new Rectangle
            {
                Width = 80,
                Height = 45,
                Fill = new SolidColorBrush(color)
            };

            Canvas.SetLeft(rect, left);
            Canvas.SetTop(rect, top);
            Canvas.SetZIndex(rect, zIndex);

            return rect;
        }

        public bool CheckCollisionWithBuissons(double newX, double newY)
        {
            Rect playerRect = new Rect(newX, newY, Player.Width, Player.Height);

            foreach (var buisson in Buissons.Where(b => b.IsEnabled))
            {
                Rect buissonRect = new Rect(Canvas.GetLeft(buisson), Canvas.GetTop(buisson), buisson.Width, buisson.Height);
                if (playerRect.IntersectsWith(buissonRect))
                {
                    Hero.Pdv += 0;
                    Random random = new Random();
                    int chance = random.Next(1, 101);
                    if (chance < 15)
                    {
                        RecevoirObjet();
                        return true;
                    }
                    else if (chance > 5)
                    {
                        buisson.IsEnabled = false;
                        Task.Run(() =>
                        {
                            Thread.Sleep(1000);
                            Dispatcher.Invoke(() =>
                            {
                                buisson.IsEnabled = true;
                            });
                        });
                    }
                }
            }
            return false;
        }

        public void RecevoirObjet()
        {
            Random random = new Random();
            int chance = random.Next(1, 101);
            if (chance < 33)
            {
                Hero.Gold += 5;
                stop = true;
                GoldImage.Show();
            }
            else if (chance >= 33 && chance < 66)
            {
                Hero.Cuir += 5;
                stop = true;
                CuirImage.Show();
            }
            else if (chance >= 66)
            {
                Hero.Pdv += 20; 
                stop = true;
                VieImage.Show();
            }
        }
        #endregion

        #region COMBAT
        private void StartCombat(Heros hero, Monstre monstre)
        {
            MessageBox.Show($"Combat lancé entre {hero.Name} et {monstre.Race}!\nPoint de vie : {monstre.Pdv}\nForce : {monstre.For}\nEndurence : {monstre.End}");

            while (monstre.Pdv > 0)
            {
                Hero.Frapper(monstre);
                monstre.Frapper(Hero);
            }
            monstre.Pdv = 0;
            if (monstre.Pdv == 0)
            {
                MortMonstre(monstre);
                Random random = new Random();
                int chance = random.Next(1, 101);
                if (chance <= 25)
                {
                    Hero.Pdv += 35;
                    MessageBox.Show($"Tu as été soigné de 35hp.");
                }
                else if (chance > 25 && chance <= 50)
                {
                    Hero.Pdv += 25;
                    MessageBox.Show($"Tu as été soigné de 25hp.");
                }
                else
                {
                    Hero.Pdv += 15;
                    MessageBox.Show($"Tu as été soigné de 15hp.");
                }
                CheckLevel();
            }
            else if (Hero.Pdv <= 0)
            {
                MessageBox.Show($"GAME OVER");
            }
            Hero.RecevoirRessourcesDe(monstre);
            Hero.Pdv += 0;
        }
        #endregion

        #endregion
    }
}
