using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Heroes_VS_Monsters.Class.Monstre
{
    public class Monstre : Charactere
    {
        #region field
        private Charactere @for;
        private Charactere end;
        private Charactere pdv;
        public string Race { get; set; }
        #endregion



        #region Properties
        public virtual int Cuir
        {
            get => 0;
        }
        public virtual int Gold
        {
            get => 0;
        }

        #endregion



        #region constructor
        public Monstre(int @for, int end, double pdv, double position, int gold, int cuir, string race)
            : base(@for, end, pdv, position, gold, cuir)
        {
            Race = race;
        }
        #endregion



        #region Fonction
        protected override void Mourir()
        {
            Console.WriteLine($"Monstre mort");
        }

        #endregion
    }
}

