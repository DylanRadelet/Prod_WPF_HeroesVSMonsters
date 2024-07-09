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
using System.Windows.Threading;
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

        public Heros Hero { get; set; } = new Heros("Arthur", 11, 6, 100, 0, 0, 0);

        public List<Monstre> Monstres { get; set; } = new List<Monstre>
        {
            new Orc("Orc", 8, 2, 50, 1),
            new Loup("Loup", 6, 4, 40, 1),
            new Dragonnet("Dragonnet", 7, 3, 60, 1)
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
        private BitmapImage bossImage;

        private int animationIndex = 0;

        private int buissonsTraverses = 0;
        private DispatcherTimer bossMoveTimer;
        private const double BossMovementSpeed = 5;
        public bool isPaused = false;
        int countMessage = 0;
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
            bossImage = new BitmapImage(new Uri("C:\\Users\\Student\\source\\repos\\Prod_WPF_HeroesVSMonsters\\Prod_WPF_HeroesVSMonsters\\img\\Personnage\\Monstre\\Boss.png"));
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

            #region Boss
            bossMoveTimer = new DispatcherTimer();
            bossMoveTimer.Interval = TimeSpan.FromSeconds(0.05); // TimeSpan => Intérvalle de temps
            bossMoveTimer.Tick += BossMove; // Tick => se produit à chaque unité de temps prédifinie dans => bossMoveTimer.Interval
            bossMoveTimer.Start();
            #endregion

            if (CurrentLevel < 4)
            {
                CreationMontres(CurrentLevel);
            }
            else
            {
                if (CurrentLevel == 4)
                {
                    Hero.Gold += 1000;
                    Hero.Cuir += 1000;
                    Hero.Pdv += 500;
                }

                Achat = 100000;
                MessageBox.Show($"Bienvenue au niveau {CurrentLevel}! Votre nombre d'achat dans la boutique est infini, ressourcez vous !");  
            }
            CheckAllBuissonsTraversed();

            InitializeBuissons();
        }

        #region JEU

        #region MONSTRE
        private void BossMove(object sender, EventArgs e)
        {
            if (!isPaused)
            {
                foreach (var pair in MonstreRectangles)
                {
                    Monstre monstre = pair.Key;
                    var img = pair.Value.Item1;
                    var rect = pair.Value.Item2;

                    double currentLeft = Canvas.GetLeft(img);
                    double currentTop = Canvas.GetTop(img);
                    double playerLeft = Canvas.GetLeft(Player);
                    double playerTop = Canvas.GetTop(Player);

                    double dx = playerLeft - currentLeft;
                    double dy = playerTop - currentTop;

                    double distance = Math.Sqrt(dx * dx + dy * dy); // Calcule la distance entre deux point 2D => Math.Sqrt => méthode qui calcule la racine carrée

                    if (distance != 0)
                    {
                        dx /= distance;
                        dy /= distance;
                    }
                     
                    Canvas.SetLeft(img, currentLeft + dx * BossMovementSpeed);
                    Canvas.SetTop(img, currentTop + dy * BossMovementSpeed);

                    Canvas.SetLeft(rect, Canvas.GetLeft(img) + (img.Width - rect.Width) / 2);
                    Canvas.SetTop(rect, Canvas.GetTop(img) + (img.Height - rect.Height) / 2);
                }
            }
        }

        public void MortMonstre(Monstre monstre)
        {
            if (MonstreRectangles.TryGetValue(monstre, out var pair)) // TryGetValue => méthode utilisé avec des dictionary pour tenter de récupérer les valeur associée à une clé spécifique
            {
                Map.Children.Remove(pair.Item1);
                Map.Children.Remove(pair.Item2);
                MonstreRectangles.Remove(monstre);
            }
        }

        public void CreationMontres(int level)
        {
            Random random = new Random();
            int numberOfMonsters = level < 4 ? level + 2 : 1;

            for (int i = 0; i < numberOfMonsters; i++)
            {
                Monstre monstre;
                Image img = new Image();
                Rectangle rect = new Rectangle
                {
                    Stroke = Brushes.Transparent,
                    Fill = Brushes.Transparent
                };

                if (level < 4)
                {
                    monstre = (i % 3 == 0) ? new Orc("Orc", 8 + level, 3 + level, 80 + (level * 10), 1) : //if
                              (i % 3 == 1) ? new Loup("Loup", 7 + level, 7 + level, 50 + (level * 10), 1) : //else if 
                              new Dragonnet("Dragonnet", 8 + level, 4 + level, 90 + (level * 10), 1); //else
                }
                else
                {
                    monstre = new Orc("Boss", 1000, 5000, 20000, 1000);
                }

                ConfigureMonsterGraphics(monstre, img, rect);

                PlaceMonsterOnMap(monstre, img, rect, random);
            }
        }

        private void ConfigureMonsterGraphics(Monstre monstre, Image img, Rectangle rect)
        {
            switch (monstre.Race)
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
                case "Boss":
                    img.Width = 275;
                    img.Height = 275;
                    img.Source = bossImage;
                    rect.Width = 250;
                    rect.Height = 250;
                    break;
                default:
                    img.Width = 50;
                    img.Height = 50;
                    rect.Width = 30;
                    rect.Height = 30;
                    break;
            }
        }

        private void PlaceMonsterOnMap(Monstre monstre, Image img, Rectangle rect, Random random)
        {
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

            MonstreRectangles[monstre] = (img, rect);
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
        //private bool EnCollision(FrameworkElement a, FrameworkElement b)
        //{
        //    double aLeft = Canvas.GetLeft(a);
        //    double aTop = Canvas.GetTop(a);
        //    double aRight = aLeft + a.Width;
        //    double aBottom = aTop + a.Height;

        //    double bLeft = Canvas.GetLeft(b);
        //    double bTop = Canvas.GetTop(b);
        //    double bRight = bLeft + b.Width;
        //    double bBottom = bTop + b.Height;

        //    return (aLeft < bRight && aRight > bLeft && aTop < bBottom && aBottom > bTop);
        //}

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

            #region ImageVisible
            if (e.Key == Key.Space && GoldImage.IsVisible)
            {
                Hero.Pdv += 0;
                GoldImage.Hide();
                isPaused = false;
            }
            if (e.Key == Key.Space && CuirImage.IsVisible)
            {
                Hero.Pdv += 0;
                CuirImage.Hide();
                isPaused = false;
            }
            if (e.Key == Key.Space && VieImage.IsVisible)
            {
                Hero.Pdv += 0;
                VieImage.Hide();
                isPaused = false;
            }
            #endregion

            if (!isPaused)
            {
                switch (e.Key)
                {
                    case Key.Up:
                        newY -= 6;
                        Player.Source = playerUpSteps[animationIndex];
                        break;
                    case Key.Down:
                        newY += 6;
                        Player.Source = playerDownSteps[animationIndex];
                        break;
                    case Key.Left:
                        newX -= 6;
                        Player.Source = playerLeftSteps[animationIndex];
                        break;
                    case Key.Right:
                        newX += 6;
                        Player.Source = playerRightSteps[animationIndex];
                        break;
                }
            }
            else
            {
                Player.Source = playerIdle;
            }

            

            if(!Collision(newX, newY))
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
            if (!isPaused)
            {
                Player.Source = playerIdle;
            }
        }
        #endregion

        #region LEVEL
        private void CheckLevel()
        {

            if (CurrentLevel < 4)
            {
                if (MonstreRectangles.Count == 0)
                {
                    CurrentLevel++;
                    Achat = 2;
                    CreationMontres(CurrentLevel);
                    MessageBox.Show($"Bienvenue au niveau {CurrentLevel}!");
                    if (CurrentLevel <= 3)
                    {
                        Hero.Gold += 10 + CurrentLevel;
                        Hero.Cuir += 10 + CurrentLevel;
                        Hero.Pdv += CurrentLevel * 25;
                    }
                    else if (CurrentLevel == 4)
                    {
                        Hero.Gold += 1000;
                        Hero.Cuir += 1000;
                        Hero.Pdv += 500;
                    }
                }
            }
        }
        #endregion

        #region SHOP
        public bool ShopCollision(double newX, double newY)
        {
            Rect playerRect = new Rect(newX, newY, Player.Width, Player.Height);
            Rect shopRect = new Rect(10, 5, 105, 60);
            ShopWindow Shop = new ShopWindow(Hero, CurrentLevel, Achat);

            if (playerRect.IntersectsWith(shopRect))
            {
                isPaused = true;
                Hero.Pdv += 0;
                Shop.ShowDialog(); //Modal
                isPaused = false;
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

        private void CheckAllBuissonsTraversed()
        {
            if (buissonsTraverses >= 6)
            {
                ActiverEasterEgg();
            }
        }

        private void ActiverEasterEgg()
        {
            if (countMessage == 0) 
            { 
                isPaused = true;
                countMessage = 1;
                MessageBox.Show("Un pouvoir vous submerge, vous devenez invincible!");
                isPaused = false;
                Hero.For = 25000;
                Hero.End = 25000;
                Hero.Pdv = 25000;
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
                if (playerRect.IntersectsWith(buissonRect)) // IntersectsWith => utilisée pour déterminer si deux rectangles se chevauchent ou s'intersectent
                {
                    Hero.Pdv += 0;
                    Random random = new Random();
                    int chance = random.Next(1, 101);
                    if (chance < 30)
                    {
                        RecevoirObjet();
                        isPaused = true;

                        return true;
                    }
                    else if (chance > 30)
                    {
                        buisson.IsEnabled = false;
                        Task.Run(() => // Lance une tâche en arrière plan de manière asynchrone
                        {
                            Thread.Sleep(1000); // met le thread actuel en pause
                            Dispatcher.Invoke(() => // exécuter une action sur le thread de l'interface utilisateur
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
                GoldImage.Show();
            }
            else if (chance >= 33 && chance < 66)
            {
                Hero.Cuir += 5;
                CuirImage.Show();
            }
            else if (chance >= 66)
            {
                Hero.Pdv += 20;
                VieImage.Show();
            }

            buissonsTraverses++;
            CheckAllBuissonsTraversed();
        }
        #endregion

        #region COMBAT
        private void StartCombat(Heros hero, Monstre monstre)
        {
            isPaused = true;

            MessageBox.Show($"Combat lancé entre {hero.Name} et {monstre.Race}!\nPoint de vie : {monstre.Pdv}\nForce : {monstre.For}\nEndurence : {monstre.End}");

            while (monstre.Pdv > 0 && hero.Pdv > 0)
            {
                hero.Frapper(monstre);
                if (monstre.Pdv > 0)
                {
                    monstre.Frapper(hero);
                }

                if (hero.Pdv <= 0)
                {
                    MessageBox.Show($"Game Over");
                    Application.Current.Shutdown(); 
                    return;
                }
            }

            if (monstre.Pdv <= 0)
            {
                monstre.Pdv = 0;
                MortMonstre(monstre);
                Random random = new Random();
                int chance = random.Next(1, 101);
                if (chance <= 25)
                {
                    hero.Pdv += 35;
                    MessageBox.Show($"Tu as été soigné de 35hp.");
                }
                else if (chance > 25 && chance <= 50)
                {
                    hero.Pdv += 25;
                    MessageBox.Show($"Tu as été soigné de 25hp.");
                }
                else
                {
                    hero.Pdv += 15;
                    MessageBox.Show($"Tu as été soigné de 15hp.");
                }
                CheckLevel();
            }

            hero.RecevoirRessourcesDe(monstre);
            isPaused = false;
        }
        #endregion

        #endregion
    }


}
