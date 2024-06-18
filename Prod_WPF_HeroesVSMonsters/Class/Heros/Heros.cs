namespace Heroes_VS_Monsters.Class.Heros
{
    public class Heros : Charactere
    {
        #region Fields
        public string Name { get; set; }
        #endregion



        #region Constructor
        public Heros(string name, int @for, int end, double pdv, double position, int gold, int cuir)
            : base(@for, end, pdv, position, gold, cuir)
        {
            Name = name;
        }
        #endregion



        #region Fonction
        protected override void Mourir()
        {
            Console.WriteLine($"Perdu !");
        }

        public void RecevoirGold(int gold)
        {
            this.Gold += gold;
        }

        public void RecevoirCuir(int cuir)
        {
            this.Cuir += cuir;
        }

        public void RecevoirRessourcesDe(Charactere monstre)
        {
            RecevoirGold(monstre.Gold);
            RecevoirCuir(monstre.Cuir);
        }
        #endregion

    }
}