using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS
{
    public abstract class Subject
    {
        private readonly List<IObserver> observadores = new List<IObserver>();

        public void Suscribir(IObserver observer)
        {
            if (!observadores.Contains(observer))
                observadores.Add(observer);
        }

        public void Desuscribir(IObserver observer)
        {
            if (observadores.Contains(observer))
                observadores.Remove(observer);
        }

        protected void Notificar()
        {
            string idiomaActual = Singleton.Instancia.IdiomaActual.ToString();

       
            foreach (IObserver observer in observadores.ToList())
            {
                try
                {
                    observer.ActualizarIdioma(idiomaActual);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error al traducir un observador: " + ex);
                }
            }
        }
    }
}
