﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmbleClient.RfqGui.RfqManager;
using log4net;

namespace AmbleClient.RfqGui
{
   public class SalesRfqItems:RfqItems
    {

       List<int> mySubs;

       public SalesRfqItems()
       { 
       }

       public bool UpdateInfo(int rfqId)
       {
           Rfq rfq = new Rfq();
           GetValuesFromGui(rfq);
           rfq.rfqNo = rfqId;
           rfq.salesId = mySubs[cbSales.SelectedIndex];

           if((rfq.rfqStates!=(int)RfqStatesEnum.Closed)&&(rfq.closeReason.HasValue))
           {
            rfq.closeReason=null;
           }


           bool suc;

           try
           {
               suc = RfqMgr.UpdateRfq(rfq);
           }
           catch (Exception ex)
           {
               suc = false;
               Logger.Error(ex.StackTrace);
           }
           return suc;
       }


       public override void FillTheTable(Rfq rfq)
       {
           base.FillTheTable(rfq);

           //Fill the salesId
           tbCustomer.Text = rfq.customerName;
           //select the sales ID
           //获得下级号和名字

           mySubs = AmbleClient.Admin.AccountMgr.AccountMgr.GetAllSubsId(UserInfo.UserId, UserCombine.GetUserCanBeSales());

           Dictionary<int, string> mySubsIdAndName = AmbleClient.Admin.AccountMgr.AccountMgr.GetIdsAndNames(mySubs);
           foreach (string name in mySubsIdAndName.Values)
           {
               cbSales.Items.Add(name);

           }
           for (int i = 0; i < mySubs.Count; i++)
           {
               if (mySubs[i] == rfq.salesId)
               {
                   cbSales.SelectedIndex = i;
                   break;

               }

           }

          List<int> pAList=new List<int>();
           //Fill the PA
          if (rfq.firstPA.HasValue)
              pAList.Add(rfq.firstPA.Value);
          if (rfq.secondPA.HasValue)
              pAList.Add(rfq.secondPA.Value);

          if (pAList.Count > 0)
          {
              Dictionary<int, string> paIDAndName = AmbleClient.Admin.AccountMgr.AccountMgr.GetIdsAndNames(pAList);

              if (rfq.firstPA.HasValue&&rfq.secondPA.HasValue)
              {
                  cbPrimaryPA.Text = paIDAndName[rfq.firstPA.Value];
                  cbAltPA.Text = paIDAndName[rfq.secondPA.Value];
              }
              else if (rfq.firstPA.HasValue)
              {
                  cbPrimaryPA.Text = paIDAndName[rfq.firstPA.Value];
              }
              else if(rfq.secondPA.HasValue)
              { 
                 cbAltPA.Text = paIDAndName[rfq.secondPA.Value];
              }
              else
              {
                  //error
              }

          }
         




       }

       public int GetAssignedSaleId()
       {
           return mySubs[cbSales.SelectedIndex];
       
       }



    }
}
