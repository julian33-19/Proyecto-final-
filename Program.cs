        using System;

namespace ProyectoFinal_AdivinaElNumero
{
    class Program
    {
        static void Main(string[] args)
        {
            // Arreglos paralelos para almacenar datos de los 2 jugadores
            string[] nombres = new string[2];
            int[] puntajes = new int[2];
            int[] totalIntentos = new int[2];
            int[] rondasJugadas = new int[2];

            int opcion = 0;
            bool jugadoresRegistrados = false;

            Console.WriteLine("=================================================");
            Console.WriteLine("   SALA DE JUEGOS: ADIVINA EL NUMERO CON RANKING ");
            Console.WriteLine("=================================================");

            // Menú principal repetitivo (Regla: ciclo while hasta elegir salir)
            while (opcion != 4)
            {
                Console.WriteLine("\n--- MENÚ PRINCIPAL ---");
                Console.WriteLine("1. Registrar / Cambiar nombres de jugadores");
                Console.WriteLine("2. Jugar ronda (2 Jugadores por turnos)");
                Console.WriteLine("3. Jugar ronda (Reto Extra: 1 Jugador vs CPU)");
                Console.WriteLine("4. Ver Ranking Final y Salir");
                Console.Write("Seleccione una opcion (1-4): ");

                // Validación de entrada del menú
                while (!int.TryParse(Console.ReadLine(), out opcion) || opcion < 1 || opcion > 4)
                {
                    Console.Write("Opción inválida. Ingrese un número entre 1 y 4: ");
                }

                switch (opcion)
                {
                    case 1:
                        RegistrarJugadores(nombres, puntajes, totalIntentos, rondasJugadas);
                        jugadoresRegistrados = true;
                        break;

                    case 2:
                        if (!jugadoresRegistrados)
                        {
                            Console.WriteLine("\n[!] Primero debe registrar los nombres (Opción 1).");
                        }
                        else
                        {
                            JugarRondaDosJugadores(nombres, puntajes, totalIntentos, rondasJugadas);
                        }
                        break;

                    case 3:
                        if (!jugadoresRegistrados)
                        {
                            Console.WriteLine("\n[!] Primero debe registrar al menos el nombre del Jugador 1 (Opción 1).");
                        }
                        else
                        {
                            JugarRondaUnJugador(nombres, puntajes, totalIntentos, rondasJugadas);
                        }
                        break;

                    case 4:
                        if (jugadoresRegistrados)
                        {
                            MostrarRanking(nombres, puntajes, totalIntentos, rondasJugadas);
                        }
                        Console.WriteLine("\n¡Gracias por jugar! Cerrando programa...");
                        break;
                }
            }
        }

        // SUBPROCESO 1: Registrar jugadores y reiniciar estadísticas
        static void RegistrarJugadores(string[] nombres, int[] puntajes, int[] totalIntentos, int[] rondasJugadas)
        {
            Console.WriteLine("\n--- REGISTRO DE JUGADORES ---");
            for (int i = 0; i < 2; i++)
            {
                Console.Write($"Ingrese el nombre del Jugador {i + 1}: ");
                string nombre = Console.ReadLine();

                // Validar dato no vacío
                while (string.IsNullOrWhiteSpace(nombre))
                {
                    Console.Write($"El nombre no puede estar vacío. Ingrese nombre del Jugador {i + 1}: ");
                    nombre = Console.ReadLine();
                }

                nombres[i] = nombre.Trim();
                puntajes[i] = 0;
                totalIntentos[i] = 0;
                rondasJugadas[i] = 0;
            }
            Console.WriteLine("¡Jugadores registrados correctamente y marcadores en 0!");
        }

        // SUBPROCESO 2: Leer número secreto con validación (1 a 100) y ocultar pantalla
        static int LeerSecreto(string nombreRetador)
        {
            int secreto;
            Console.WriteLine($"\nTurno de {nombreRetador} para elegir el número secreto.");
            Console.Write("Digite un número entre 1 y 100 (el oponente no debe mirar): ");

            while (!int.TryParse(Console.ReadLine(), out secreto) || secreto < 1 || secreto > 100)
            {
                Console.Write("Entrada inválida. Digite un número entero entre 1 y 100: ");
            }

            // Limpiar consola para que el otro jugador no vea el número
            Console.Clear();
            Console.WriteLine("Número secreto guardado. ¡Pantalla limpia!");
            return secreto;
        }

        // SUBPROCESO 3: Lógica de adivinanza para un turno (límite 10 intentos)
        static int EjecutarTurnoAdivinanza(string nombreAdivinador, int numeroSecreto)
        {
            const int MAX_INTENTOS = 10;
            int intentos = 0;
            int suposicion = 0;
            bool adivinado = false;

            Console.WriteLine($"\n>>> Turno de adivinar para: {nombreAdivinador} (Máximo {MAX_INTENTOS} intentos) <<<");

            while (intentos < MAX_INTENTOS && !adivinado)
            {
                intentos++;
                Console.Write($"Intento {intentos}/{MAX_INTENTOS} - Ingrese su número (1-100): ");

                while (!int.TryParse(Console.ReadLine(), out suposicion) || suposicion < 1 || suposicion > 100)
                {
                    Console.Write("Número fuera de rango. Ingrese un valor entre 1 y 100: ");
                }

                if (suposicion == numeroSecreto)
                {
                    Console.WriteLine($"¡CORRECTO! {nombreAdivinador} adivinó el número en {intentos} intento(s).");
                    adivinado = true;
                }
                else if (suposicion < numeroSecreto)
                {
                    Console.WriteLine("Pista: MÁS ALTO.");
                }
                else
                {
                    Console.WriteLine("Pista: MÁS BAJO.");
                }
            }

            if (!adivinado)
            {
                Console.WriteLine($"[X] Se agotaron los {MAX_INTENTOS} intentos. El número secreto era: {numeroSecreto}.");
                // Se retorna 11 para indicar que no logró adivinar en el límite de 10
                return 11;
            }

            return intentos;
        }

        // SUBPROCESO 4: Jugar ronda completa invirtiendo roles
        static void JugarRondaDosJugadores(string[] nombres, int[] puntajes, int[] totalIntentos, int[] rondasJugadas)
        {
            // Fase 1: Jugador 1 pone secreto, Jugador 2 adivina
            int secretoJ1 = LeerSecreto(nombres[0]);
            int intentosJ2 = EjecutarTurnoAdivinanza(nombres[1], secretoJ1);

            Console.WriteLine("\nPresione ENTER para cambiar de roles...");
            Console.ReadLine();

            // Fase 2: Jugador 2 pone secreto, Jugador 1 adivina
            int secretoJ2 = LeerSecreto(nombres[1]);
            int intentosJ1 = EjecutarTurnoAdivinanza(nombres[0], secretoJ2);

            // Actualizar estadísticas y puntajes
            ActualizarPuntajes(nombres, puntajes, totalIntentos, rondasJugadas, intentosJ1, intentosJ2);
        }

        // SUBPROCESO 5: Actualizar puntajes (3 pts ganar, 1 pt empatar) y estadísticas
        static void ActualizarPuntajes(string[] nombres, int[] puntajes, int[] totalIntentos, int[] rondasJugadas, int intentosJ1, int intentosJ2)
        {
            // Sumar intentos reales (si sacó 11, gastó sus 10 intentos máximos)
            totalIntentos[0] += (intentosJ1 > 10) ? 10 : intentosJ1;
            totalIntentos[1] += (intentosJ2 > 10) ? 10 : intentosJ2;

            rondasJugadas[0]++;
            rondasJugadas[1]++;

            Console.WriteLine("\n--- RESULTADO DE LA RONDA ---");
            Console.WriteLine($"{nombres[0]}: {(intentosJ1 > 10 ? "No adivinó (10 intentos)" : intentosJ1 + " intentos")}");
            Console.WriteLine($"{nombres[1]}: {(intentosJ2 > 10 ? "No adivinó (10 intentos)" : intentosJ2 + " intentos")}");

            if (intentosJ1 > 10 && intentosJ2 > 10)
            {
                Console.WriteLine("Ninguno adivinó el número. No se otorgan puntos.");
            }
            else if (intentosJ1 < intentosJ2)
            {
                puntajes[0] += 3;
                Console.WriteLine($"¡Ganador de la ronda: {nombres[0]} (+3 puntos)!");
            }
            else if (intentosJ2 < intentosJ1)
            {
                puntajes[1] += 3;
                Console.WriteLine($"¡Ganador de la ronda: {nombres[1]} (+3 puntos)!");
            }
            else
            {
                puntajes[0] += 1;
                puntajes[1] += 1;
                Console.WriteLine("¡Empate! Ambos jugadores reciben +1 punto.");
            }
        }

        // SUBPROCESO 6 (RETO EXTRA): Modo 1 jugador contra número aleatorio del sistema
        static void JugarRondaUnJugador(string[] nombres, int[] puntajes, int[] totalIntentos, int[] rondasJugadas)
        {
            Random rnd = new Random();
            int numeroAleatorio = rnd.Next(1, 101); // Genera entre 1 y 100

            Console.WriteLine($"\n--- MODO SOLITARIO (RETO EXTRA) ---");
            Console.WriteLine($"El sistema ha generado un número secreto entre 1 y 100 para {nombres[0]}.");

            int intentos = EjecutarTurnoAdivinanza(nombres[0], numeroAleatorio);
            totalIntentos[0] += (intentos > 10) ? 10 : intentos;
            rondasJugadas[0]++;

            if (intentos <= 10)
            {
                puntajes[0] += 3;
                Console.WriteLine($"¡Felicidades {nombres[0]}! Venciste a la máquina (+3 puntos).");
            }
            else
            {
                Console.WriteLine("La máquina gana esta ronda. ¡Inténtalo de nuevo!");
            }
        }

        // SUBPROCESO 7: Mostrar ranking final, campeón y promedio de intentos
        static void MostrarRanking(string[] nombres, int[] puntajes, int[] totalIntentos, int[] rondasJugadas)
        {
            Console.WriteLine("\n=================================================");
            Console.WriteLine("             RANKING FINAL Y ESTADÍSTICAS        ");
            Console.WriteLine("=================================================");

            // Determinar orden de visualización (Mayor puntaje primero)
            int primero = 0;
            int segundo = 1;

            if (puntajes[1] > puntajes[0])
            {
                primero = 1;
                segundo = 0;
            }

            int[] orden = { primero, segundo };

            for (int i = 0; i < orden.Length; i++)
            {
                int idx = orden[i];
                double promedio = rondasJugadas[idx] > 0
                    ? (double)totalIntentos[idx] / rondasJugadas[idx]
                    : 0.0;

                Console.WriteLine($"#{i + 1} | Jugador: {nombres[idx]} | Puntos: {puntajes[idx]} | Rondas: {rondasJugadas[idx]} | Promedio intentos: {promedio:F2}");
            }

            Console.WriteLine("-------------------------------------------------");
            if (puntajes[0] > puntajes[1])
            {
                Console.WriteLine($"🏆 CAMPEÓN DEFINITIVO: {nombres[0]} con {puntajes[0]} puntos.");
            }
            else if (puntajes[1] > puntajes[0])
            {
                Console.WriteLine($"🏆 CAMPEÓN DEFINITIVO: {nombres[1]} con {puntajes[1]} puntos.");
            }
            else
            {
                Console.WriteLine("🤝 RESULTADO FINAL: EMPATE EN EL MARCADOR.");
            }
            Console.WriteLine("=================================================");
        }
    }
}
