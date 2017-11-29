﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using CommSubSystem;
using CommSubSystem.Commands;
using CommSubSystem.Receive;
using Messages;
using SharedObjects;

namespace LobbyApp
{
    public partial class ClientForm : Form
    {
        private readonly SendInvoker _SendingInvoker = new SendInvoker();
        private readonly ReceiveInvoker _ReceivingInvoker = new ReceiveInvoker();

        public Thread _receivingThread;
        public LobbyReceive _ReceivingProcess;

        private static readonly object MyLock = new object();
        

        public ClientForm()
        {
            InitializeComponent();

            UDPClient.UDPInstance.SetupAndRun(1025);
            CommandFactory.Instance.SendInvoker = _SendingInvoker;
            ReceivingFactory.Instance.ReceiveInvoker = _ReceivingInvoker;

            _ReceivingProcess = new LobbyReceive();
            _receivingThread = new Thread(_ReceivingProcess.Receiving);
            _receivingThread.IsBackground = true;

            _SendingInvoker.Start();
            _ReceivingInvoker.Start();
            _receivingThread.Start();
            

            ReceivingFactory.Instance.Start();
        }

        

        private void ClientForm_Load(object sender, EventArgs e)
        {

        }

        private void LobbyForm_FormClosed(object sender, EventArgs e)
        {
            _SendingInvoker.Stop();
            _ReceivingInvoker.Stop();
            ReceivingFactory.Instance.Stop();
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            //call a method that needs to be refreshed a second
            //something that needs to redraw
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            //gets the address, port, and message to be sent from the textfields
            CommandFactory.Instance.CreateAndExecute("send", AddressTextBox.Text, textBox2.Text);
        }
    }
}
