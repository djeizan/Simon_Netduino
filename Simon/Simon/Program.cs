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
    public class Program
    {
       /************************************************************************
        *                          Simon Minimalista                           *
        ************************************************************************
        * Autor     : Jaison Carvalho                                          *
        * Data      : 04.01.2015                                               *
        ************************************************************************
        * Descrição : Versão Mininalista do Simon para Netduino Plus 2 :)      *
        ************************************************************************/
        public static void Main()
        {                             
            while (true)
            {
                //Aguarda o botão ser pressionado a primeira vez para iniciar o jogo
                if (GPIO.Botao.Read())
                Debug.Print(System.DateTime.Now.ToString() + " " + "Iniciando Jogo...");
                    Simon.IniciaJogo();                    
                }
                    
            }

        }

    }
