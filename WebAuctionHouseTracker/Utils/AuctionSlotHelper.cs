﻿namespace WebApplication1.Utils;

public class AuctionSlotHelper
{
    public static string MoneyHumaniser(long coppercoins)
    {
        long gold = coppercoins / 10000;
        long silver = (coppercoins % 10000) / 100;
        long copper = (coppercoins % 100);
        
        return $"{gold}g{silver}s{copper}c"   ;
    }
}