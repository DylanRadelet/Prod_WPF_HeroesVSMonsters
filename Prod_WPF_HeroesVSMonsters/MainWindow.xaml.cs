using Heroes_VS_Monsters.Class;
using Heroes_VS_Monsters.Class.Heros;
using Heroes_VS_Monsters.Class.Monstre;
using Prod_WPF_HeroesVSMonsters;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAnimatedGif;
using static System.Net.Mime.MediaTypeNames;

namespace HeroesVSMonsters
{
    public partial class MainWindow : Window
    {
        #region INITIALISATION 

        public int CurrentLevel { get; set; } = 1;

        public List<Rectangle> Buissons { get; set; } = new List<Rectangle>();

        public Heros Hero { get; set; } = new Heros("Arthur", 11, 5, 95, 0, 0, 0);

        public List<Monstre> Monstres { get; set; } =
        [
            new Orc("Orc", 8, 3, 50, 1),
            new Loup("Loup", 5,5,40,1),
            new Dragonnet("Dragonnet", 8,4,60,1)
        ];

        public Dictionary<Monstre, Rectangle> MonstreRectangles { get; set; } = new Dictionary<Monstre, Rectangle>();
        
        #endregion

        private GameImage GoldImage;
        private GameImage CuirImage;

        int Achat = 2;

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += OnKeyDown;
            DataContext = Hero;

            CreationMontres(CurrentLevel);
            InitializeBuissons();

            #region IMAGES
            GoldImage = new GameImage("img/gold.png", 300, 300, 350, 200);
            Map.Children.Add(GoldImage.ImageControl);
            GoldImage.Hide();

            CuirImage = new GameImage("img/Cuir.png", 300, 300, 350, 200);
            Map.Children.Add(CuirImage.ImageControl);
            GoldImage.Hide();
            #endregion 
        }

        #region JEU

        #region MONSTRE
        public void MortMonstre(Monstre monstre)
        {
            if (MonstreRectangles.TryGetValue(monstre, out Rectangle rect))
            {
                Map.Children.Remove(rect);
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

                Rectangle rect = new Rectangle { Width = 20, Height = 20 };
                double left = random.Next(10, (int)(1010 - rect.Width));
                double top = random.Next(10, (int)(690 - rect.Height));

                Canvas.SetLeft(rect, left);
                Canvas.SetTop(rect, top);

                rect.Fill = monst.Race switch
                {
                    "Orc" => new SolidColorBrush(Colors.Purple),
                    "Loup" => new SolidColorBrush(Colors.Yellow),
                    "Dragonnet" => new SolidColorBrush(Colors.Red),
                    _ => new SolidColorBrush(Colors.Black)
                };

                Canvas.SetZIndex(rect, 2);
                Map.Children.Add(rect);
                MonstreRectangles[monst] = rect;
            }
        }

        private bool CollisionMonstre(double newX, double newY)
        {
            Rectangle playerRect = new Rectangle { Width = Player.Width, Height = Player.Height };
            Canvas.SetLeft(playerRect, newX);
            Canvas.SetTop(playerRect, newY);

            foreach (var pair in MonstreRectangles)
            {
                if (EnCollision(playerRect, pair.Value))
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
        private bool EnCollision(Rectangle a, Rectangle b)
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
            if (newX < 5 || newX + Player.Width > Border1.ActualWidth - 5 ||
                newY < 5 || newY + Player.Height > Border1.ActualHeight - 5)
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

            switch (e.Key)
            {
                case Key.Up:
                    newY -= 5;
                    break;
                case Key.Down:
                    newY += 5;
                    break;
                case Key.Left:
                    newX -= 5;
                    break;
                case Key.Right:
                    newX += 5;
                    break;
            }

            #region ImageVisible
            if (e.Key == Key.Space && GoldImage.IsVisible)
            {
                Hero.Pdv += 0;
                GoldImage.Hide();
            }
            if (e.Key == Key.Space && CuirImage.IsVisible)
            {
                Hero.Pdv += 0;
                CuirImage.Hide();
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
                        }
                    }
                }
            }
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
                    Hero.Pdv += CurrentLevel * 15;
                }
                else if (CurrentLevel > 6 && CurrentLevel < 9)
                {
                    Hero.Gold += 20 + CurrentLevel;
                    Hero.Cuir += 20 + CurrentLevel;
                    Hero.Pdv += CurrentLevel * 10;
                }
                else
                {

                }
            }
        }
        #endregion

        #region SHOP
        public bool ShopCollision(double newX, double newY)
        {
            Rectangle playerRect = new Rectangle { Width = Player.Width, Height = Player.Height };
            Canvas.SetLeft(playerRect, newX);
            Canvas.SetTop(playerRect, newY);

            Rectangle shopRect = new Rectangle { Width = 48, Height = 45 };
            Canvas.SetLeft(shopRect, 10);
            Canvas.SetTop(shopRect, 5);

            

            if (EnCollision(playerRect, shopRect))
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
            Buissons.Add(CreateBuisson(134, 402, 842, 304, 0));
            Buissons.Add(CreateBuisson(875, 604, 100, 102, 0));
            Buissons.Add(CreateBuisson(681, 91, 294, 616, 0));
            Buissons.Add(CreateBuisson(708, 360, 267, 347, 0));
            Buissons.Add(CreateBuisson(383, 578, 593, 127, 0));
            Buissons.Add(CreateBuisson(245, 238, 731, 469, 0));

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
                Width = 50,
                Height = 30,
                Fill = new SolidColorBrush(color)
            };

            
            Canvas.SetLeft(rect, left);
            Canvas.SetTop(rect, top);
            Canvas.SetZIndex(rect, zIndex);

            return rect;
        }

        public bool CheckCollisionWithBuissons(double newX, double newY)
        {
            Rectangle playerRect = new Rectangle { Width = Player.Width, Height = Player.Height };
            Canvas.SetLeft(playerRect, newX);
            Canvas.SetTop(playerRect, newY);

            foreach (var buisson in Buissons.Where(b => b.IsEnabled)) 
            {
                if (EnCollision(playerRect, buisson))
                {
                    Hero.Pdv += 0;
                    Random random = new Random();
                    int chance = random.Next(1, 101);
                    if (chance < 15)
                    {
                        RecevoirObjet();
                        return true;
                    }
                    else if (chance > 15)
                    {
                        buisson.IsEnabled = false;
                        Task.Run(() =>
                        {
                            Thread.Sleep(5000);
                            Dispatcher.Invoke(new Action(() =>
                            {
                                buisson.IsEnabled = true;
                            }));
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
                Hero.Pdv += 0;
                GoldImage.Show();
            }
            else if (chance > 33 && chance < 66)
            {
                Hero.Cuir += 5;
                Hero.Pdv += 0;
                CuirImage.Show();
            }
            else if (chance > 66)
            {
                Hero.Pdv += 20;
                MessageBox.Show("Vous vous êtes soignée de 20 points de vie !");
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