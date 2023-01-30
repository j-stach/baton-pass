#region Using declarations

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Dapper;

#endregion

namespace BatonPass_Dapper
{
    #region SQL row object models

    public class MarketDataRow
    {
        public DateTime Time_ { get; set; }
        public string Instrument_ { get; set; }
        public double Price_ { get; set; }
        public long Volume_ { get; set; }
    }

    public class AskLadderRow
    {
        public int AskPosition { get; set; }
        public double AskPrice { get; set; }
        public long AskVolume { get; set; }
    }

    public class BidLadderRow
    {
        public int BidPosition { get; set; }
        public double BidPrice { get; set; }
        public long BidVolume { get; set; }
    }

    #endregion

    // Contains methods for accessing BatonPass_DB
    public class DatabaseAccess
    {
        // Adds new MarketDataRow to marketData_table
        public static void AddMarketData(DateTime time, string instrument, double price, long volume)
        {
            using (IDbConnection connection = new SqlConnection(@"Server=localhost\SQLEXPRESS;Database=BatonPass_DB;Trusted_Connection=True;"))
            {
                List<MarketDataRow> marketData_table = new List<MarketDataRow>();
                marketData_table.Add(new MarketDataRow { Time_ = time, Instrument_ = instrument, Price_ = price, Volume_ = volume });
                connection.Execute("dbo.AddMarketData @Time_, @Instrument_, @Price_, @Volume_", marketData_table);
            }
        }

        // Updates askLadder_table
        public static void UpdateAskLadder(int position, double price, long volume)
        {
            using (IDbConnection connection = new SqlConnection(@"Server=localhost\SQLEXPRESS;Database=BatonPass_DB;Trusted_Connection=True;"))
            {
                List<AskLadderRow> askLadder_table = new List<AskLadderRow>();
                askLadder_table.Add(new AskLadderRow { AskPosition = position, AskPrice = price, AskVolume = volume });
                connection.Execute("dbo.UpdateAskLadder @AskPosition, @AskPrice, @AskVolume", askLadder_table);
            }
        }

        // Updates bidLadder_table
        public static void UpdateBidLadder(int position, double price, long volume)
        {
            using (IDbConnection connection = new SqlConnection(@"Server=localhost\SQLEXPRESS;Database=BatonPass_DB;Trusted_Connection=True;"))
            {
                List<BidLadderRow> bidLadder_table = new List<BidLadderRow>();
                bidLadder_table.Add(new BidLadderRow { BidPosition = position, BidPrice = price, BidVolume = volume });
                connection.Execute("dbo.UpdateBidLadder @BidPosition, @BidPrice, @BidVolume", bidLadder_table);
            }
        }
    }
}
