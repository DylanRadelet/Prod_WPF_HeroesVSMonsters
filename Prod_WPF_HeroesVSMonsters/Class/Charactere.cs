using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heroes_VS_Monsters.Class
{
    public class Charactere : INotifyPropertyChanged
    {
        #region Fields
        public int For { get; set; }
        public int End { get; set; }
        private double _pdv;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Gold { get; set; }
        public int Cuir { get; set; }
        public double Position { get; set; }
        #endregion



        #region Constructor
        public Charactere(int @for, int end, double pdv, double position, int gold, int cuir)
        {
            For = @for;
            End = end;
            Pdv = pdv;
            Position = position;
            Gold = gold;
            Cuir = cuir;
        }
        #endregion



        #region  Properties
        public virtual double BonusFor => 0;
        public virtual double BonusEnd => 0;

        public double Pdv
        {
            get { return _pdv; }
            set
            {
                _pdv = value;
                OnPropertyChanged();
                if (_pdv <= 0)
                {
                    Mourir();
                }
            }
        }
        #endregion



        #region  Fonctions
        public void RecevoirDegats(int degats)
        {
            Pdv -= degats;
        }

        protected virtual void Mourir() { }

        public int ModifierAttaque(int score)
        {
            if (score < 5) return -1;
            else if (score < 10) return 0;
            else if (score < 15) return 1;
            else return 2;
        }

        public void Frapper(Charactere cible)
        {
            int degats = (int)(this.For - cible.End);
            if (degats > 0)
            {
                cible.RecevoirDegats(degats + ModifierAttaque(For));
            }
            else
            {
                Console.WriteLine("Pas de dégâts infligés.");
            }
        }

        protected void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
        #endregion

    }
}
