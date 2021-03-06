﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmbleClient.OfferGui.OfferMgr
{
  public class Offer
  {
     public  int offerId;
     public int rfqNo;
     public string mpn;
     public string mfg;
     public string vendorName;
     public string contact;
     public string phone;
     public string fax;
     public string email;
     public string packing;
     public int quantity;
     public string LT;
     public float price;
     public int buyerId;
     public DateTime offerDate;
     public int offerStates;
     public string notes;
   }


  public enum OfferState
  { 
   New=0,
   Routed=1,
   Closed=2
  };


}
