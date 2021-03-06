﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AmbleClient.RfqGui
{
   public class BuyerRfqItems:RfqItems
    {

       public BuyerRfqItems()
       {

       }
       public override void FillTheTable(AmbleClient.RfqGui.RfqManager.Rfq rfq)
       {
           base.FillTheTable(rfq);


           this.tbCustomer.Text = string.Empty; this.tbCustomer.ReadOnly = true;
           this.tbContact.Text = string.Empty; this.tbContact.ReadOnly = true;
           this.tbPhone.Text = string.Empty; this.tbPhone.ReadOnly = true;
           this.tbFax.Text = string.Empty; this.tbFax.ReadOnly = true;

           cbSales.Items.Add(AmbleClient.Admin.AccountMgr.AccountMgr.GetNameById(rfq.salesId));
           cbSales.SelectedIndex = 0;


          if (rfq.firstPA != null) 
           {
               cbPrimaryPA.Items.Add(AmbleClient.Admin.AccountMgr.AccountMgr.GetNameById(rfq.firstPA.Value));
               cbPrimaryPA.SelectedIndex = 0;

           }
           if (rfq.secondPA != null)
           {
               cbAltPA.Items.Add(AmbleClient.Admin.AccountMgr.AccountMgr.GetNameById(rfq.secondPA.Value));
              cbAltPA.SelectedIndex = 0;

           }

       }

    }
}
