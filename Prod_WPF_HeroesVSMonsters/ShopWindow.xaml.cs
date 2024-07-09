using Heroes_VS_Monsters.Class.Heros;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Prod_WPF_HeroesVSMonsters
{
    public partial class ShopWindow : Window, INotifyPropertyChanged
    {
        private readonly Heros _hero;
        private readonly int _level;
        private int prix;
        private int _achat;

        public int Prix
        {
            get { return prix; }
            set
            {
                if (prix != value)
                {
                    prix = value;
                    OnPropertyChanged(nameof(Prix));
                }
            }
        }

        public ShopWindow(Heros hero, int CurrentLevel, int achat)
        {
            InitializeComponent();

            _hero = hero;
            _level = CurrentLevel;
            _achat = 2;
            Prix = 5 * CurrentLevel;
            
            DataContext = this;
        }

        private void AchatVie(object sender, RoutedEventArgs e)
        {
            if (_hero.Gold >= Prix && _achat > 0)
            {
                _hero.Gold -= Prix;
                _hero.Pdv += 30 + (_level * 5);
                _hero.Pdv += 0;
                _achat -= 1;
            }
            else
            {
                MessageBox.Show("Vous ne pouvez pas effectuer cette action.");
            }
        }

        private void AchatForce(object sender, RoutedEventArgs e)
        {
            if (_hero.Cuir >= Prix && _achat > 0)
            {
                _hero.Cuir -= Prix;
                _hero.For += 1 + _level;
                _hero.Pdv += 0;
                _achat -= 1;
            }
            else
            {
                MessageBox.Show("Vous ne pouvez pas effectuer cette action.");
            }
        }

        private void AchatEndurance(object sender, RoutedEventArgs e)
        {
            if (_hero.Gold >= Prix && _achat > 0)
            {
                _hero.Gold -= Prix;
                _hero.End += 1 + _level;
                _hero.Pdv += 0;
                _achat -= 1;
            }
            else
            {
                MessageBox.Show("Vous ne pouvez pas effectuer cette action.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
