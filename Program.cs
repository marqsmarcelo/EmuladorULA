using System;

namespace Emulador_ULA
{
    class Program
    {
        // Emulador de ULA desenvolvido para a matéria de Arquitetura de Computadores e Sistemas Operacionais da Universidade Veiga de Almeida

        // 1º Semestre de 2017

        // Aluno: Marcelo Emanuel Volpato Marques
        // Matrícula: 20122104110

        // Referências e Materiais:
        //      https://aulasdec.wordpress.com/2010/11/24/uma-aprofundada-em-operadores-e-logica-em-c/
        //      http://conversordemedidas.info/sistema-binario.php
        //      https://pt.wikipedia.org/wiki/Unidade_l%C3%B3gica_e_aritm%C3%A9tica
        //      http://stackoverflow.com/questions/856845/how-to-best-way-to-draw-table-in-console-app-c 

        // Registrador A (Acc)
        public static byte registradorA = 0;

        // Registrador B
        public static byte registradorB = 0;

        // Controle F
        public static byte controleF = 0;

        // OPCODE
        public static byte opcode;

        // Registrador de flags
        public static byte[] flags = new byte[4] { 0, 0, 0, 0 };

        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("\nMenu Principal da ULA \n");
                Console.WriteLine("   001. Definir registrador A"); //1
                Console.WriteLine("   010. Definir registrador B"); //2
                Console.WriteLine("   011. Ler registrador A (Acc)"); //3
                Console.WriteLine("   100. Ler registrador B"); //4
                Console.WriteLine("   101. Ler registrador de flags"); //5
                Console.WriteLine("   110. Definir operação"); //6
                Console.WriteLine("   111. Executar ULA"); //7
                Console.WriteLine("   000. Sair"); //0
                Console.Write("\nEscolha a opção => ", "n");

                try
                {
                    controleF = Convert.ToByte(BinToDec(Console.ReadLine()));
                    if (controleF > 7)
                        flags[3] = 1;
                    if (controleF != 5 && controleF < 8)
                        flags[3] = 0;
                    switch (controleF)
                    {
                        case 0: // 000: //0
                            Console.WriteLine("\nEmulador ULA Encerrado.\n");
                            Environment.Exit(0);
                            break;
                        case 1: // 001: //1
                            Console.Write("Valor: ");
                            registradorA = Convert.ToByte(BinToDec(Console.ReadLine()));
                            break;
                        case 2: // 010: //2
                            Console.Write("Valor: ");
                            registradorB = Convert.ToByte(BinToDec(Console.ReadLine()));
                            break;
                        case 3: // 011: //3
                            Console.WriteLine("Registrador A (Acc): " + DecToBin(registradorA) == "" ? "0" : DecToBin(registradorA));
                            EsperarLeitura();
                            break;
                        case 4: // 100: //4
                            Console.WriteLine("Registrador B é: " + DecToBin(registradorB) == "" ? "0" : DecToBin(registradorB));
                            EsperarLeitura();
                            break;
                        case 5: // 101: //5
                                /*
                                 FLAGS:
                                 | 0 | 0 | 0 | 0 | 
                                 | x | x | x | 1 | Controle F Inválido
                                 | x | x | 1 | x | OPCODE Inválido
                                 | x | 1 | x | x | OverflowException 
                                 | 1 | x | x | x | DivideByZeroException
                                */
                            foreach (var flag in flags)
                                Console.Write(flag);
                            EsperarLeitura();
                            flags[3] = 0;
                            break;
                        case 6: // 110: //6
                            Console.Clear();
                            flags[2] = 0;
                            Console.WriteLine(" ==========================");
                            Console.WriteLine(" | OPCODE |    OPERAÇÃO   |");
                            Console.WriteLine(" ==========================");
                            Console.WriteLine(" |  0001  |  A (Acc) + B  |"); //1
                            Console.WriteLine(" |  0010  |     B - A     |"); //2
                            Console.WriteLine(" |  0011  |     A * B     |"); //3
                            Console.WriteLine(" |  0100  |     B / A     |"); //4
                            Console.WriteLine(" |  0101  |    A AND B    |"); //5
                            Console.WriteLine(" |  0110  |    A OR B     |"); //6
                            Console.WriteLine(" |  0111  |    A XOR B    |"); //7
                            Console.WriteLine(" |  1000  |  NOT A (Acc)  |"); //8
                            Console.WriteLine(" ==========================");
                            //Console.WriteLine("   000011. Subtrair A de B");
                            //Console.WriteLine("   000110. Dividir A por B");
                            Console.Write("\nEscolha a opção => ", "n");
                            try
                            {
                                opcode = Convert.ToByte(BinToDec(Console.ReadLine()));
                                if (opcode > 8)
                                {
                                    flags[2] = 1;
                                    opcode = 0;
                                }
                            }
                            catch
                            {
                                flags[2] = 1;
                                opcode = 0;
                            }
                            break;
                        case 7:
                            switch (opcode)
                            {
                                case 1: // 0001: //1
                                    Somar(registradorA, registradorB);
                                    break;
                                case 2: // 0010: //2
                                    Subtrair(registradorA, registradorB);
                                    break;
                                case 3: // 0011: //3
                                    Multiplicar(registradorA, registradorB);
                                    break;
                                case 4: // 0100: //4
                                    Dividir(registradorB, registradorA);
                                    break;
                                case 5: // 0101: //5
                                    OperaçãoAnd(registradorA, registradorB);
                                    break;
                                case 6: // 0110: //6
                                    OperaçãoOr(registradorA, registradorB);
                                    break;
                                case 7: // 0111: //7
                                    OperaçãoXor(registradorA, registradorB);
                                    break;
                                case 8: // 1000: //8
                                    OperaçãoNot(registradorA);
                                    break;
                            }
                            break;
                     }
                }
                catch
                {
                    flags[3] = 1;
                    controleF = 0;
                }
                finally
                {
                    controleF = 0;
                    Console.Clear();
                }
            }
            while (controleF == 0);
        }

        static void EsperarLeitura()
        {
            Console.WriteLine("\nENTER para continuar...\n");
            Console.ReadLine();
        }

        static byte OperaçãoAnd (byte RegistradorA, byte RegistradorB)
        {
            // Compara o Registrador A e B bit-a-bit com o E lógico
            registradorA = Convert.ToByte(RegistradorA & RegistradorB);
            return registradorA;
        }

        static byte OperaçãoOr (byte RegistradorA, byte RegistradorB)
        {
            // Compara o Registrador A e B bit-a-bit com o E lógico
            registradorA = Convert.ToByte(RegistradorA | RegistradorB);
            return registradorA;
        }

        static byte OperaçãoXor(byte RegistradorA, byte RegistradorB)
        {
            // Compara o Registrador A e B bit-a-bit com o E lógico
            registradorA = Convert.ToByte(RegistradorA ^ RegistradorB);
            return registradorA;
        }

        static byte OperaçãoNot(byte RegistradorA)
        {
            // Compara o Registrador A e B bit-a-bit com o E lógico
            registradorA = Convert.ToByte(~RegistradorA);
            return registradorA;
        }

        static byte Somar (byte SomandoA, byte SomandoB)
        {
            try
            {
                flags[1] = 0;
                byte Carregar;
                // Enquanto houverem bits para somar no Registrador B
                while (SomandoB != 0)
                {
                    // Compara o Registrador A e B bit-a-bit com o E lógico
                    // Como na operação binária 1 + 1 é igual a 10, a operação vê se "leva 1" para somar ao próximo bit
                    Carregar = Convert.ToByte(SomandoA & SomandoB);

                    // Na prática é como somar usando XOR (OU Exclusivo)
                    // Compara bit-a-bit:
                    //      Se 1+1, retorna 0, o Carregar cuida de levar o 1 para o próximo bit
                    //      Se 0+0, retorna 0
                    //      Se 1+0, retorna 1
                    SomandoA ^= SomandoB;

                    // Se encarrega de andar o bit do Carregar um bit a frente, armazenando o valor no Somando B
                    SomandoB = Convert.ToByte(Carregar << 1);
                }

                registradorA = SomandoA;
            }
            catch (OverflowException)
            {
                flags[1] = 1;
            }

            return SomandoA;
        }

        static byte Subtrair(byte Subtraendo, byte Minuendo)
        {
            byte resultado = 0;

            // Executa operação lógica NÂO sobre o Minuendo e o soma com 1 para dar seu valor negativo
            Minuendo = Somar(Convert.ToByte(~Minuendo), 1);

            // Como a subtração é uma extensão da adição, somamos o Subtraendo com o valor negativo do Minuendo
            resultado = Somar(Subtraendo, Minuendo);

            registradorA = resultado;

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
        static byte Multiplicar(byte Multiplicando, byte Multiplicador)
        {
            byte resultado = 0;

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
            }

            registradorA = resultado;

            return resultado;   
        }

        /*1º – Você deve inicializar o quociente em ZERO
        2º – Subtraia o divisor do dividendo para obter o resultado parcial(RP)
        * Se RP >= (maior ou igual) a ZERO, incremente o quociente e continue.
        *Se RP <= (menor ou igual) a ZERO, pare
        3º = O resto torna-se dividendo.Vá para o passo 2*/
        static byte Dividir(byte Dividendo, byte Divisor)
        {
            byte resultado = 0;

            if (Divisor > 0 && Dividendo > 0)
            {
                try
                {
                    int Quociente, Resto;

                    Resto = Divisor;
                    Quociente = 0;
                    while (Resto < (1 << 30) && Resto < Dividendo) // Should be 1 << (Bitsize - 2)
                    {
                        Resto <<= 1;
                    }
                    while (Resto >= Divisor)
                    {
                        Quociente <<= 1;
                        if (Dividendo >= Resto)
                        {
                            Dividendo = Subtrair(Dividendo, Convert.ToByte(Resto));
                            Quociente |= 1;
                        }
                        Resto >>= 1;
                    }
                    resultado = Convert.ToByte(Quociente);
                    registradorA = resultado;
                }
                catch (DivideByZeroException)
                {
                    flags[0] = 1;
                }
            }
            else
            {
                flags[0] = 1;
            }

            return resultado;
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
    }
}
