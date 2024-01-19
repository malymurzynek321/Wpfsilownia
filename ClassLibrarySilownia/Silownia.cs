using ClassLibrarySilownia.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrarySilownia
{
    public class Silownia
    {
        public ObservableCollection<Dieta> diety = new ObservableCollection<Dieta>();
        public ObservableCollection<Trening> treningi = new ObservableCollection<Trening>();
        public ObservableCollection<BMI> bmii = new ObservableCollection<BMI>();
        public void DodajDiete(string opisDiety)
        {
            diety.Add(new Dieta { OpisDiety = opisDiety });
        }

        public void DodajTrening(string opisTrenigu, int iloscPowtorzen)
        {
            treningi.Add(new Trening { OpisTreningu = opisTrenigu, IloscPowtorzen = iloscPowtorzen });
        }

        public double ObliczBMI(double waga, double wzrost)
        {
            if (wzrost <= 0)
            {
                throw new InvalidOperationException("Wzrost musi być większy niż zero.");
            }
            double bmi = waga / (wzrost * wzrost) * 10000.0;
            bmii.Add(new BMI { Waga = waga, Wzrost = wzrost, WynikBMI = bmi });
            return bmi;
        }
    }
}
