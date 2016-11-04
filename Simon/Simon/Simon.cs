using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace Simon
{
    public static class Simon
    {
        private static int[] iRodadasJogo;
        private static int _ContadorPulsos;
        public static void AcertouRodada()
        {

            GPIO.Led.Write(true);
            Thread.Sleep(1000);
            GPIO.Led.Write(false);
            for (int j = 0; j < 2; j++)
            {
                GPIO.Led.Write(true);
                Thread.Sleep(50);
                GPIO.Led.Write(false);
                Thread.Sleep(50);
            }
            Thread.Sleep(1000);
        }
        public static void FinalizaJogo()
        {
            //Rotina de Encerramento
            for (int j = 0; j < 10; j++)
            {
                GPIO.Led.Write(true);
                Thread.Sleep(25);
                GPIO.Led.Write(false);
                Thread.Sleep(25);
            }
            iRodadasJogo = null;

        }
        public static void IniciaJogo()
        {

            //Rotina de Inicialização
            for (int j = 0; j < 5; j++)
            {
                GPIO.Led.Write(true);
                Thread.Sleep(50);
                GPIO.Led.Write(false);
                Thread.Sleep(50);
            }
            Thread.Sleep(1000);
            Debug.Print(System.DateTime.Now.ToString() + " " + "Jogo Iniciado");

            while (true)
            {

                bool auxAcertou = false;
                Sorteia();
                //Pisca LED com numeros aleatórios sorteados.
                foreach (var itemiRodadasJogo1 in iRodadasJogo)
                {
                    Pisca(itemiRodadasJogo1);
                    Debug.Print(System.DateTime.Now.ToString() + " " + "Numero Sorteado:" + itemiRodadasJogo1);
                    Thread.Sleep(500);

                }
                Thread.Sleep(50);
                GPIO.Led.Write(true);
                Thread.Sleep(25);
                GPIO.Led.Write(false);
                Thread.Sleep(25);

                //Verifica se o botão está sendo pressionado de acordo com o sorteio
                foreach (var itemiRodadasJogo in iRodadasJogo)
                {

                    Conta(itemiRodadasJogo);
                    if (Simon.ContadorPulsos != itemiRodadasJogo)
                    {
                        Debug.Print(System.DateTime.Now.ToString() + " " + "Numero Pressionado:" + ContadorPulsos + " Errou");
                        ContadorPulsos = 0;
                        FinalizaJogo();
                        auxAcertou = false;
                        break;
                    }
                    else
                    {
                        Debug.Print(System.DateTime.Now.ToString() + " " + "Numero Pressionado:" + ContadorPulsos + " Acertou");

                        auxAcertou = true;
                    }
                    //Reseta contador de pulsos no botão para próxima rodada
                    ContadorPulsos = 0;
                }
                if (auxAcertou)
                {
                    Thread.Sleep(500);
                    AcertouRodada();
                }
                else
                {
                    break;
                }

            }

        }
        public static bool Pisca(int pContador)
        {
            for (int i = 0; i < pContador; i++)
            {
                GPIO.Led.Write(true);
                Thread.Sleep(250);
                GPIO.Led.Write(false);
                Thread.Sleep(250);
            }
            return true;
        }
        public static bool AdicionaPassoJogo()
        {
            int[] tmpiRodadasJogo;

            if (iRodadasJogo == null)
            {
                iRodadasJogo = new int[1];
            }
            else
            {
                tmpiRodadasJogo = new int[iRodadasJogo.Length + 1];
                iRodadasJogo.CopyTo(tmpiRodadasJogo, 0);
                iRodadasJogo = new int[tmpiRodadasJogo.Length];
                tmpiRodadasJogo.CopyTo(iRodadasJogo, 0);
            }
            return true;
        }
        public static bool Sorteia()
        {
            AdicionaPassoJogo();
            Random rnd = new Random();
            iRodadasJogo[iRodadasJogo.Length - 1] = rnd.Next(9) + 1;
            Debug.Print(System.DateTime.Now.ToString() + " " + "Sorteio Rodada:" + iRodadasJogo.Length.ToString());

            return true;
        }
        public static void Conta(int pContador)
        {
            bool bAuxBotao = new bool();
            DateTime dtDelayBotao;
            dtDelayBotao = System.DateTime.Now;

            while (true)
            {

                //Se botão não for pressionado em 2 segundos cancela a rodada
                if (System.DateTime.Now >= dtDelayBotao.AddSeconds(2))
                    break;

                #region ControleBotao
                if (GPIO.Botao.Read() && bAuxBotao == false && System.DateTime.Now > dtDelayBotao.AddMilliseconds(500))
                {
                    dtDelayBotao = System.DateTime.Now;
                    ContadorPulsos = ContadorPulsos + 1;
                    Debug.Print(System.DateTime.Now.ToString() + " " + "Botão Pressionado " + ContadorPulsos.ToString() + " Intervalo:" + System.DateTime.Now.Subtract(dtDelayBotao).ToString());
                    bAuxBotao = true;
                    GPIO.Led.Write(true);
                }
                else if (!GPIO.Botao.Read() && bAuxBotao == true)
                {
                    Debug.Print(System.DateTime.Now.ToString() + " " + "Botão solto");
                    bAuxBotao = false;
                    GPIO.Led.Write(false);
                    if (pContador == ContadorPulsos)
                        break;
                }
                #endregion
            }
        }
        public static int ContadorPulsos
        {
            get { return _ContadorPulsos; }
            set { _ContadorPulsos = value; }
        }
    }
}