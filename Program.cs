using System;

namespace Emulador_ULA
{
    class Program
    {
        // Emulador de ULA desenvolvido para a matéria de Arquitetura de Computadores e Sistemas Operacionais da Universidade Veiga de Almeida

        // 1º Semestre de 2017u

        // Aluno: Marcelo Emanuel Volpato Marques
        // Matrícula: 20122104110

        // Referências e Materiais:
        //      https://aulasdec.wordpress.com/2010/11/24/uma-aprofundada-em-operadores-e-logica-em-c/
        //      http://conversordemedidas.info/sistema-binario.php
        //      https://pt.wikipedia.org/wiki/Unidade_l%C3%B3gica_e_aritm%C3%A9tica
        //      http://stackoverflow.com/questions/856845/how-to-best-way-to-draw-table-in-console-app-c
        //      

        // Registrador A (Acc)
        public static int registradorA;

        // Registrador B
        public static int registradorB;

        // Operação a ser realizada pela ULA
        public static int operacao;

        // Largura da tabela para impressão dos resultados
        const int tableWidth = 77;

        static void Main(string[] args)
        {
            int opcao;
            do
            {
                opcao = Menu();

                switch (opcao)
                {
                    case 0:
                        Console.WriteLine("\nO programa será encerrado.\n");
                        Environment.Exit(0);
                        break;
                    case 1:
                        Console.Write("Novo valor para registrador A: ");
                        int.TryParse(Console.ReadLine(), out registradorA);
                        Console.WriteLine("Valor de A definido para: " + registradorA);
                        break;
                    case 2:
                        Console.Write("Novo valor para registrador B: ");
                        int.TryParse(Console.ReadLine(), out registradorB);
                        Console.WriteLine("Valor de B definido para: " + registradorB);
                        break;
                    case 3:
                        Console.WriteLine("O valor do registrador A é: " + registradorA);
                        break;
                    case 4:
                        Console.WriteLine("O valor do registrador B é: " + registradorB);
                        break;
                    case 5:
                        break;
                    case 7:
                        switch (operacao)
                        {
                            case 1:
                                Somar(registradorA, registradorB);
                                break;
                            case 2:
                                Subtrair(registradorA, registradorB);
                                break;
                            case 3:
                                Subtrair(registradorB, registradorA);
                                break;
                            case 4:
                                Multiplicar(registradorA, registradorB);
                                break;
                            case 5:
                                Dividir(registradorB, registradorA);
                                break;
                            case 6:
                                Dividir(registradorA, registradorB);
                                break;
                        }
                        break;
                }
                Continuar();
                Console.Clear();
            }
            while (opcao != 0);
        }

        static void Continuar()
        {
            Console.WriteLine("\nPressione ENTER para continuar...\n");
            Console.ReadLine();
        }

        static void ImprimirRegistradores(int RegistradorA, int RegistradorB)
        {
            PrintLine();
            PrintRow("", "Registrador A", "Registrador B");
            PrintLine();
            PrintRow("Decimal", RegistradorA.ToString(), RegistradorB.ToString());
            PrintRow("Binário", DecToBin(RegistradorA).ToString(), DecToBin(RegistradorB).ToString());
            PrintLine();
        }

        static int Somar (int SomandoA, int SomandoB)
        {
            Console.WriteLine("\nSomando registradores A e B\n");

            ImprimirRegistradores(SomandoA, SomandoB);

            Console.WriteLine();
            PrintLine();
            PrintRow("", "Carregar", "Somando A", "Somando B");
            PrintLine();

            int Carregar;
            // Enquanto houverem bits para somar no Registrador B
            while (SomandoB != 0)
            {
                // Compara o Registrador A e B bit-a-bit com o E lógico
                // Como na operação binária 1 + 1 é igual a 10, a operação vê se "leva 1" para somar ao próximo bit
                Carregar = SomandoA & SomandoB;

                // Na prática é como somar usando XOR (OU Exclusivo)
                // Compara bit-a-bit:
                //      Se 1+1, retorna 0, o Carregar cuida de levar o 1 para o próximo bit
                //      Se 0+0, retorna 0
                //      Se 1+0, retorna 1
                SomandoA ^= SomandoB;

                // Se encarrega de andar o bit do Carregar um bit a frente, armazenando o valor no Somando B
                SomandoB = Carregar << 1;

                PrintRow("Decimal", Carregar.ToString(), SomandoA.ToString(), SomandoB.ToString());
                PrintRow("Binário", DecToBin(Carregar).ToString(), DecToBin(SomandoA).ToString(), DecToBin(SomandoB).ToString());
                PrintLine();
            }

            Console.WriteLine("\nRESULTADO DA SOMA: " + SomandoA + "\n");
            return SomandoA;
        }

        static int Subtrair(int Subtraendo, int Minuendo)
        {
            Console.WriteLine("\nSubtraindo registrador B do registrador A\n");

            ImprimirRegistradores(Subtraendo, Minuendo);

            // Executa operação lógica NÂO sobre o Minuendo e o soma com 1 para dar seu valor negativo
            Minuendo = Somar(~Minuendo, 1);

            // Como a subtração é uma extensão da adição, somamos o Subtraendo com o valor negativo do Minuendo
            int resultado = Somar(Subtraendo, Minuendo);

            Console.WriteLine("\nRESULTADO DA SUBTRAÇÃO: " + resultado + "\n");
            return resultado;
        }

        // Explicação completa em https://en.wikipedia.org/wiki/Binary_multiplier
        /*     1011   (this is 11 in decimal)
             x 1110   (this is 14 in decimal)
             ======
               0000   (this is 1011 x 0)
              1011    (this is 1011 x 1, shifted one position to the left)
             1011     (this is 1011 x 1, shifted two positions to the left)
          + 1011      (this is 1011 x 1, shifted three positions to the left)
          =========
           10011010   (this is 154 in decimal)*/
        static int Multiplicar(int Multiplicando, int Multiplicador)
        {
            Console.WriteLine("\nMultiplicando registradores A e B\n");
            ImprimirRegistradores(Multiplicando, Multiplicador);

            int resultado = 0;

            // Verifica se o multiplicador é 0, o que zera o resultado - que já começa como 0
            while (Multiplicador != 0)
            {
                // Na multiplicação binária só temos os dígitos 0 e 1
                // Vamos multiplicando parcialmente o Multiplicando por cada bit do Multiplicador
                // Se esse bit for 0, pulamos o bit, já que o resultado seria 0
                // Se esse bit for 1, somamos o Multiplicando ao resultado - já que Multiplicando X 1 é o próprio Multiplicando
                if ((Multiplicador & 1) == 1)
                {
                    resultado = Somar(resultado, Multiplicando);
                }
                // Dobra o multiplicando ao mesmo tempo que tira metado do multiplicador
                // Dessa forma vamos "andando" as casas como no exemplo da Wikipedia acima
                Multiplicador >>= 1;
                Multiplicando <<= 1;

                Console.WriteLine();
                PrintLine();
                PrintRow("", "Multiplicando", "Multiplicador", "Resultado");
                PrintLine();
                PrintRow("Decimal", Multiplicando.ToString(), Multiplicador.ToString(), resultado.ToString());
                PrintRow("Binário", DecToBin(Multiplicando).ToString(), DecToBin(Multiplicador).ToString(), DecToBin(resultado).ToString());
                PrintLine();
            }
            Console.WriteLine("\nRESULTADO DA MULTIPLICAÇÃO: " + resultado + "\n");
            return resultado;   
        }

        static int Dividir(int Dividendo, int Divisor)
        {
            int Quociente = 0;
            int Resto = Divisor;

            while (Resto < (1 << 30) && Resto < Dividendo)
            {
                Resto <<= 1;
            }
            while (Resto >= Divisor)
            {
                Quociente <<= 1;
                if (Dividendo >= Resto)
                {
                    Dividendo = Subtrair(Dividendo, Resto);
                    Quociente |= 1;
                }
                Resto >>= 1;
            }
            Console.WriteLine("\nRESULTADO DA DIVISÃO: " + Quociente + "\n");
            Console.WriteLine("\nRESTO DA DIVISÃO: " + Resto + "\n");
            return Quociente;
            /*1º – Você deve inicializar o quociente em ZERO
            2º – Subtraia o divisor do dividendo para obter o resultado parcial(RP)
            * Se RP >= (maior ou igual) a ZERO, incremente o quociente e continue.
            *Se RP <= (menor ou igual) a ZERO, pare
            3º = O resto torna-se dividendo.Vá para o passo 2*/
        }

        // Opções do Menu
        static int Menu()
        {
            int opcao = 0;

            Console.WriteLine("\nMenu Principal da ULA \n");
            Console.WriteLine("   1. Definir registrador A");
            Console.WriteLine("   2. Definir registrador B");
            Console.WriteLine("   3. Ler registrador A (Acc)");
            Console.WriteLine("   4. Ler registrador B");
            Console.WriteLine("   5. Ler registrador de flags");
            Console.WriteLine("   6. Definir operação");
            Console.WriteLine("   7. Executar ULA");
            Console.WriteLine("   0. Sair");
            Console.Write("\nEscolha a opção => ", "n");

            try
            {
                opcao = int.Parse(Console.ReadLine());
                Console.WriteLine();
            }
            catch
            {
                Console.WriteLine("\nOção Inválida, tente novamente.\n");
                Menu();
            }

            if (opcao == 6)
            {
                Console.WriteLine("Qual operação deseja realizar?");
                Console.WriteLine("   1. Somar registradores A e B");
                Console.WriteLine("   2. Subtrair B de A");
                Console.WriteLine("   3. Subtrair A de B");
                Console.WriteLine("   4. Multiplicar registradores A e B");
                Console.WriteLine("   5. Dividir B por A");
                Console.WriteLine("   6. Dividir A por B");
                Console.Write("\nEscolha a opção => ", "n");

                try
                {
                    operacao = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                }
                catch
                {
                    Console.WriteLine("\nOção Inválida, tente novamente.\n");
                    Menu();
                }
            }

            return opcao;
        }

        // Método do Prof. Miguel para converter Decimal para Binário
        static string DecToBin(int numero)
        {
            int resto;
            char[] ArrayBinario;
            string resultado = string.Empty;

            while (numero > 0)
            {
                resto = numero % 2;
                resultado += resto;
                numero = numero / 2;
            }
            ArrayBinario = resultado.ToCharArray();
            Array.Reverse(ArrayBinario);
            resultado = new string(ArrayBinario);

            return resultado;
        }

        // Método do Prof. Miguel para converter Binário para Decimal
        static int BinToDec(string bin)
        {

            long l = Convert.ToInt64(bin, 2);
            int i = (int)l;

            return i;
        }

        // Método para imprimir tabela alinhada
        // Retirado de http://stackoverflow.com/questions/856845/how-to-best-way-to-draw-table-in-console-app-c
        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }
        static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }
        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }

    }
}
