﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AmbleClient.Order
{
    public partial class ItemSplit : Form
    {
        int qty;
        int firstQty;
        
        public ItemSplit(int qty,DateTime dockDate)
        {
            InitializeComponent();
            this.qty = qty;
            this.preDtp.Value = dockDate;
            this.preDtp.Enabled = false;
            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = qty;
            numericUpDown1.Increment = 1;
            this.firstDateTime.Value = this.secondDateTime.Value = dockDate;
            
        }

        public int GetFirstQty()
        {
            return firstQty;
        
        }

        public DateTime GetFirstDateTime()
        {
            return firstDateTime.Value;
        
        }

        public DateTime GetSecondDateTime()
        {
            return secondDateTime.Value;
        }

        private void ItemSplit_Load(object sender, EventArgs e)
        {
            lbQty.Text = qty.ToString();
            numericUpDown1.Value=qty;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            firstQty=(int)numericUpDown1.Value;
            lbSecond.Text = (qty - numericUpDown1.Value).ToString();
        }

        private void numericUpDown1_TextChanged(object sender, EventArgs e)
        {
            numericUpDown1_ValueChanged(sender, e);


        }

        private void btOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void numericUpDown1_Validated(object sender, EventArgs e)
        {

        }
    }
}
