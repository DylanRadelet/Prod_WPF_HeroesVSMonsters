using Heroes_VS_Monsters.Class;
using Heroes_VS_Monsters.Class.Heros;
using Heroes_VS_Monsters.Class.Monstre;
using Prod_WPF_HeroesVSMonsters;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Numerics;
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
        public int CurrentLevel { get; set; } = 1;

        public List<Rectangle> Buissons { get; set; } = new List<Rectangle>();

        public Heros Hero { get; set; } = new Heros("Arthur", 10, 5, 100, 0, 0, 0);

        public List<Monstre> Monstres { get; set; } =
        [
            new Orc("Orc", 8, 3, 80, 1),
            new Loup("Loup", 7,7,50,1),
            new Dragonnet("Dragonnet", 8,4,90,1)
        ];

        public Dictionary<Monstre, Rectangle> MonstreRectangles { get; set; } = new Dictionary<Monstre, Rectangle>();

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += OnKeyDown;
            DataContext = Hero;

            CreationMontres(CurrentLevel);
            InitializeBuissons();
        }

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
                double left = random.Next(0, (int)(1020 - rect.Width));
                double top = random.Next(0, (int)(700 - rect.Height));

                Canvas.SetLeft(rect, left);
                Canvas.SetTop(rect, top);

                rect.Fill = monst.Race switch
                {
                    "Orc" => new SolidColorBrush(Colors.Purple),
                    "Loup" => new SolidColorBrush(Colors.Yellow),
                    "Dragonnet" => new SolidColorBrush(Colors.Red),
                    _ => new SolidColorBrush(Colors.Black)
                };

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
                    StartCombat(Hero, pair.Key);
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

            if (!Collision(newX, newY))
            {
                if (!CollisionMonstre(newX, newY))
                {
                    if (!CheckCollisionWithBuissons(newX, newY))
                    {
                        Canvas.SetLeft(Player, newX);
                        Canvas.SetTop(Player, newY);
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
                CreationMontres(CurrentLevel);
                MessageBox.Show($"Bienvenue au niveau {CurrentLevel}!");
                if (CurrentLevel < 3)
                {
                    Hero.Gold += 10;
                    Hero.Cuir += 10;
                    Hero.Pdv = CurrentLevel * 25;
                }
                else if (CurrentLevel > 3 && CurrentLevel < 6)
                {
                    Hero.Gold += 15;
                    Hero.Cuir += 15;
                    Hero.Pdv = CurrentLevel * 15;
                }
                else if (CurrentLevel > 6 && CurrentLevel < 9)
                {
                    Hero.Gold += 20;
                    Hero.Cuir += 20;
                    Hero.Pdv = CurrentLevel * 5;
                }
                else
                {

                }
            }
        }
        #endregion

        #region BUISSONS
        public void InitializeBuissons()
        {
            Buissons.Add(CreateBuisson(134, 402, 842, 304));
            Buissons.Add(CreateBuisson(875, 604, 100, 102));
            Buissons.Add(CreateBuisson(681, 91, 294, 616));
            Buissons.Add(CreateBuisson(708, 360, 267, 347));
            Buissons.Add(CreateBuisson(383, 578, 593, 127));
            Buissons.Add(CreateBuisson(245, 238, 731, 469));

            foreach (var buisson in Buissons)
            {
                Map.Children.Add(buisson);
            }
        }
        public Rectangle CreateBuisson(int left, int top, int right, int bottom)
        {
            var color = Color.FromRgb(0x3A, 0x9D, 0x23);

            var rect = new Rectangle
            {
                Width = 50,
                Height = 30,
                Fill = new SolidColorBrush(color),
                
            };

            
            Canvas.SetLeft(rect, left);
            Canvas.SetTop(rect, top);

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
                MessageBox.Show("Vous avez trouvé 5 Gold !");
            }
            else if (chance > 33 && chance < 66)
            {
                Hero.Cuir += 5;
                Hero.Pdv += 0;
                MessageBox.Show("Vous avez trouvé 5 Cuir !");
            }
            else if (chance > 66)
            {
                Hero.Pdv += 20;
                MessageBox.Show("Vous vous êtes soignée de 20 points de vie !");
            }
            
        }
        #endregion

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
                Hero.For += 1;
                Hero.End += 1;
                Hero.Pdv += 15;
                MessageBox.Show($"Tu as gagné\n+1 Force, +1 Endurence et tu as été soigné de 15hp.");
                CheckLevel();
            }
            else if (Hero.Pdv <= 0)
            {
                MessageBox.Show($"GAME OVER");
            }
            Hero.RecevoirRessourcesDe(monstre);
        }
    }
}