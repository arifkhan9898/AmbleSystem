﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AmbleClient.Order.PoMgr;

namespace AmbleClient.PO
{
    public partial class PoItemsControl : UserControl
    {
        int poItemId;

        public PoItemsControl()
        {
            InitializeComponent();
            this.cbCurrency.Items.AddRange(
            Enum.GetNames(typeof(Currency))
              );

        }

        public void FillTheItems(poitems item,bool isNewCreatePo)
        {
            if (isNewCreatePo)
            {
                this.btFiles.Enabled = false;
            }
            this.poItemId = item.poItemsId;
            tbPartNo.Text = item.partNo;
            tbMfg.Text = item.mfg;
            tbDc.Text = item.dc;
            tbVendorIntPartNo.Text = item.vendorIntPartNo;
            tbCoo.Text = item.coo;
            if (UserInfo.Job == JobDescription.LogisticsManager || UserInfo.Job == JobDescription.Boss || UserInfo.Job == JobDescription.Admin)
            {
                tbCoo.ReadOnly = false;

            }
            else
            {
                tbCoo.ReadOnly = true;
            }

            tbQty.Text = item.qty.ToString();
            tbQtyRevd.Text=item.qtyRecd.ToString();
            tbQtyCorrected.Text=item.qtyCorrected.ToString();
            tbQtyAccept.Text=item.qtyAccept.ToString();
            tbQtyRejected.Text = item.qtyRejected.ToString();
            tbQtyRtv.Text = item.qtyRTV.ToString();
            tbQcPending.Text = item.qcPending.ToString();
            cbCurrency.SelectedIndex =(int)item.currency;
            tbUnitPrice.Text = item.unitPrice.ToString();
            tbTotal.Text = (item.qty * item.unitPrice).ToString();
            dateTimePicker1.Value = item.dockDate;
            tbSalesAgent.Text = AllAccountInfo.GetNameAccordingToId(item.salesAgent);

            if (item.receiveDate == null)
            {
                dateTimePicker2.Checked = false;
            }
            else
            {
                dateTimePicker2.Value = item.receiveDate.Value;
            }
            tbStepCode.Text = item.stepCode;
            tbSalesAgent.Text = AllAccountInfo.GetNameAccordingToId(item.salesAgent);
            tbNoteToVendor.Text = item.noteToVendor;







        }

        public poitems GetPoItem()
        {

            DateTime? datetime;
            if (dateTimePicker2.Checked)
            {
                datetime = dateTimePicker2.Value.Date;
            }
            else
            {
                datetime = null;
            }
                int? qtyR;
                 if(tbQtyRevd.Text.Trim().Length==0)
                     qtyR=null;
                 else
                     qtyR=int.Parse(tbQtyRevd.Text.Trim());
              int? qtyC;
                 if(tbQtyCorrected.Text.Trim().Length==0)
                     qtyC=null;
                 else
                     qtyC= int.Parse(tbQtyCorrected.Text.Trim());
             int? qtyA;
                 if(tbQtyAccept.Text.Trim().Length==0)
                     qtyA=null;
                 else
                     qtyA=int.Parse(tbQtyAccept.Text.Trim());
              int? qtyRej;
                 if(tbQtyRejected.Text.Trim().Length==0)
                     qtyRej=null;
                 else
                     qtyRej=int.Parse(tbQtyRejected.Text.Trim());
               int? qtyRt;
                  if(tbQtyRtv.Text.Trim().Length==0)
                      qtyRt=null;
                  else
                      qtyRt=int.Parse(tbQtyRtv.Text.Trim());
               int? qcP;
                   if(tbQcPending.Text.Trim().Length==0)
                       qcP=null;
                   else
                       qcP= int.Parse(tbQcPending.Text.Trim());


            return new poitems
            {
                partNo = tbPartNo.Text.Trim().ToUpper(),
                mfg = tbMfg.Text.Trim().ToUpper(),
                dc = tbDc.Text.Trim(),
                vendorIntPartNo = tbVendorIntPartNo.Text.Trim().ToUpper(),
                coo = tbCoo.Text.Trim(),
                qty = int.Parse(tbQty.Text.Trim()),
                qtyRecd =qtyR,
                qtyCorrected = qtyC,
                qtyAccept = qtyA,
                qtyRejected =qtyRej,
                qtyRTV =qtyRt,
                qcPending = qcP,
                currency = (sbyte)cbCurrency.SelectedIndex,
                unitPrice = float.Parse(tbUnitPrice.Text.Trim()),
                dockDate = dateTimePicker1.Value,
                receiveDate = datetime,
                stepCode = tbStepCode.Text.Trim(),
                salesAgent = 0,
                noteToVendor = tbNoteToVendor.Text.Trim()
            };

       
        }

        public bool CheckValues()
        {
            if (ItemsCheck.CheckTextBoxEmpty(tbPartNo) == false)
            {
                MessageBox.Show("Please input the Part Number.");
                 return false;
            }

            if (!ItemsCheck.CheckTextBoxEmpty(tbMfg))
            {
                MessageBox.Show("Please input the MFG.");
                return false;
            }

            if (!ItemsCheck.CheckTextBoxEmpty(tbDc))
            {
                MessageBox.Show("Please input the D/C.");
                return false;
            }

            if (!ItemsCheck.CheckTextBoxEmpty(tbQty))
            {
                MessageBox.Show("Please input the Qty.");
                return false;
            }
            else
            {
                if (!ItemsCheck.CheckIntNumber(tbQty))
                {
                    MessageBox.Show("The Qty should be an integer value.");
                    tbQty.Focus();
                    return false;
                
                }
            
            }

            if (ItemsCheck.CheckIntNumber(tbQtyRevd) && (!ItemsCheck.CheckIntNumber(tbQtyRevd)))
            {
                MessageBox.Show("The Qty Recv should be an integer value.");
                tbQtyRevd.Focus();
                return false;
            }
            if (ItemsCheck.CheckIntNumber(tbQtyCorrected) && (!ItemsCheck.CheckIntNumber(tbQtyCorrected)))
            {
                MessageBox.Show("The Qty Corrected should be an integer value.");
                tbQtyCorrected.Focus();
                return false;
            }
            if (ItemsCheck.CheckIntNumber(tbQtyAccept) && (!ItemsCheck.CheckIntNumber(tbQtyAccept)))
            {
                MessageBox.Show("The Qty Accept should be an integer value.");
                tbQtyAccept.Focus();
                return false;
            }
            if (ItemsCheck.CheckIntNumber(tbQtyRejected) && (!ItemsCheck.CheckIntNumber(tbQtyRejected)))
            {
                MessageBox.Show("The Qty Rejected should be an integer value.");
                tbQtyRejected.Focus();
                return false;
            }
            if (ItemsCheck.CheckIntNumber(tbQtyRtv) && (!ItemsCheck.CheckIntNumber(tbQtyRtv)))
            {
                MessageBox.Show("The Qty RTV should be an integer value.");
                tbQtyRtv.Focus();
                return false;
            }
            if (ItemsCheck.CheckIntNumber(tbQcPending) && (!ItemsCheck.CheckIntNumber(tbQcPending)))
            {
                MessageBox.Show("The QC/Pending should be an integer value.");
                tbQcPending.Focus();
                return false;
            }

            if (!ItemsCheck.CheckTextBoxEmpty(tbUnitPrice))
            {
                MessageBox.Show("Please input the Unit Price.");
                tbUnitPrice.Focus();
                return false;
            
            }

            if (!ItemsCheck.CheckFloatNumber(tbUnitPrice))
            {
                MessageBox.Show("The Unit Price should be a float value.");
                tbUnitPrice.Focus();
                return false;
            }

            return true;
        }

        private void tbUnitPrice_TextChanged(object sender, EventArgs e)
        {
            if ((!string.IsNullOrWhiteSpace(tbUnitPrice.Text.Trim())) && ItemsCheck.CheckFloatNumber(tbUnitPrice))
            {
                if (ItemsCheck.CheckIntNumber(tbQty))
                {
                    tbTotal.Text = (Convert.ToInt32(tbQty.Text.Trim()) * Convert.ToSingle(tbUnitPrice.Text.Trim())).ToString();
                }
                else
                {
                    tbTotal.Text = "";
                }
            }
        }

        private void tbQty_TextChanged(object sender, EventArgs e)
        {
            if ((!string.IsNullOrWhiteSpace(tbQty.Text.Trim())) && ItemsCheck.CheckIntNumber(tbQty))
            {
                if (ItemsCheck.CheckFloatNumber(tbUnitPrice))
                {
                    tbTotal.Text = (Convert.ToInt32(tbQty.Text.Trim()) * Convert.ToSingle(tbUnitPrice.Text.Trim())).ToString();
                }
                else
                {
                    tbTotal.Text = "";
                }
            }
        }

        private void btFiles_Click(object sender, EventArgs e)
        {
            Order.PoView.PoMaterials pm = new Order.PoView.PoMaterials(this.poItemId);
            pm.ShowDialog();
        }
    
    }
}
