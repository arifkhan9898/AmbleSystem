﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbleClient.Order.PoMgr;

namespace AmbleClient.Order
{
    public class FLPoListView:PoListView
    {

        protected override void ViewStart()
        {
            base.ViewStart();
            this.Text = "PO List for Finance && logistics";
            tscbList.Enabled = false;
        
        }

        protected override void FillTheDataGrid()
        {

            dataGridView1.Rows.Clear();

            poList = Order.PoMgr.PoMgr.GetPoAccordingToFilter(1/*Admin*/, true, filterColumn, filterString, intStateList);

            foreach (po poItem in poList)
            {
                dataGridView1.Rows.Add(poItem.poId, Tool.Get6DigitalNumberAccordingToId(poItem.poId), poItem.vendorName, poItem.contact, AllAccountInfo.GetNameAccordingToId((int)poItem.pa), 
                    poItem.poDate.ToShortDateString(),
                    poItem.paymentTerms,
                    poItem.freight, poItem.vendorNumber, Enum.GetName(typeof(PoStatesEnum),poItem.poStates));
            }
        }




    }
}
