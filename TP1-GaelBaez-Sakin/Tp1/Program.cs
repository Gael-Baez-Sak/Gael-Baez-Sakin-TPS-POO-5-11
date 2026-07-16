using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tp1
{
    //Realizar un programa que represente una simulación de copos de nieve cayendo en la consola, utilizando el símbolo "*" para cada copo.
    //El programa debe cumplir con las siguientes condiciones:
    //Definir una clase Configuracion que almacene parámetros de la simulación, como la cantidad de filas, columnas y la velocidad de caída de los copos.
    //Definir una clase Copo que modele el comportamiento de un copo de nieve. Cada copo debe tener una posición en la consola y un método para mostrarse y desplazarse hacia abajo.
    //Usar una lista para administrar todos los copos activos durante la simulación.
    //Implementar una lógica que controle la caída de los copos de nieve, evitando que se superpongan en la misma posición.
    //Al completarse una fila con copos en todas las columnas, esta debe eliminarse para permitir que continúe la simulación.
    //El programa debe ejecutarse en un ciclo continuo, simulando de manera animada la caída de los copos

    class Configuracion
    {
        // clase configuracion con los parametros del largo, ancho y velocidad en que caen los copos de nieve
        public int Filas = 15;
        public int Columnas = 10;
        public int Velocidad = 250;
    }

    class Copo
    {
        // clase copo con los parametro de las posiciones x (columna) e y (fila)
        public int X;
        public int Y;

        // constructor de copo que recibe la posicion X y la posicion Y se inicializa en 0 pq los copos siempre aparecen arriba del todo
        public Copo(int x)
        {
            X = x;
            Y = 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // oculta el cursor para que se vea mas limpio todo
            Console.CursorVisible = false;

            Configuracion config = new Configuracion();

            // lista de los copos cayendo 
            List<Copo> copos = new List<Copo>();

            // Matriz para los copos ya apoyados
            // si es false = vacío
            // si es true = hay un copo
            bool[,] suelo = new bool[config.Filas, config.Columnas];

            Random random = new Random();

            while (true)
            {
                // poner un copo en una columna aleatoria
                int columna = random.Next(config.Columnas);
                if (!suelo[0, columna])
                {   // si la primer fila de esa columna esa libre, añade un copo
                    copos.Add(new Copo(columna));
                }

                // mover copos
                for (int i = copos.Count - 1; i >= 0; i--)
                {
                    Copo c = copos[i];

                    // se fija si el copo llego a la última fila
                    bool llegoAlFondo = c.Y == config.Filas - 1;

                    // si todavia no llego al fondo, revisar si abajo hay un copo apoyado.
                    bool chocaConOtro = !llegoAlFondo && suelo[c.Y + 1, c.X];

                    // si llegó al fondo o chocó con otro copo
                    if (llegoAlFondo || chocaConOtro)
                    {
                        // queda fijo en el suelo
                        suelo[c.Y, c.X] = true;

                        // ya no sigue cayendo y se elimina de la list
                        copos.RemoveAt(i);
                    }
                    else
                    {
                        // si no choco, baja una fila
                        c.Y++;
                    }
                }

                // borrar las filas completas
                // revisar todas las filas desde abajo hacia arriba
                for (int fila = config.Filas - 1; fila >= 0; fila--)
                {
                    bool completa = true;

                    // comprueba si toda la fila esta llena.
                    for (int x = 0; x < config.Columnas; x++)
                    {
                        if (!suelo[fila, x])
                        {
                            // si hay un espacio vacio, la fila no esta completa
                            completa = false;
                            break;
                        }
                    }

                    // si la fila esta completa, se borra la fila
                    if (completa)
                    {
                        for (int y = fila; y > 0; y--)
                        {
                            for (int x = 0; x < config.Columnas; x++)
                            {
                                // todas las filas se mueven una fila hacia abajo, copiando la fila de arriba
                                suelo[y, x] = suelo[y - 1, x];
                            }
                        }

                        // la primera fila queda vacía porque no hay ninguna fila por encima para copiar
                        for (int x = 0; x < config.Columnas; x++)
                            suelo[0, x] = false;

                        fila++; // como las filas bajaron, vuelve a revisar la misma fila despues de mover
                    }
                }

                Console.Clear();

                // imprimir copos acumulados
                for (int y = 0; y < config.Filas; y++)
                {
                    for (int x = 0; x < config.Columnas; x++)
                    {
                        if (suelo[y, x])
                        {
                            Console.SetCursorPosition(x, y);
                            Console.Write("*");
                        }
                    }
                }

                // imprimir copos cayendo
                foreach (Copo c in copos)
                {
                    Console.SetCursorPosition(c.X, c.Y);
                    Console.Write("*");
                }

                // espera 250 milisegundos antes de la siguiente actualizacion
                Thread.Sleep(config.Velocidad);
            }
        }
    }
}
