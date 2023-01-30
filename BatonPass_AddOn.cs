#region Using declarations

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using NinjaTrader.Cbi;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Data;
using System.Windows.Controls;
using System.Windows.Automation;

using BatonPass_Dapper;

#endregion


namespace NinjaTrader.NinjaScript.Indicators	        // Namespace for NT Indicators
{
	public class BatonPass_AddOn : Indicator            // BatonPass
    {
        #region UI declarations

        private Chart chartWindow;
        private Grid chartTraderGrid;
        private Button passButton;

        #endregion

        // "Main" function equivalent
        protected override void OnStateChange()
        {
            // Debug
            Print("State." + State.ToString());

            #region States

            // Default info
            if (State == State.SetDefaults)
            {
				Description		= @"Passes market data to SQL database (BatonPassDB)";
				Name			= "BatonPass";

				Calculate					= Calculate.OnEachTick;
                DisplayInDataBox			= false;
                PaintPriceMarkers			= false;
                IsSuspendedWhileInactive	= false;
            }

            // Configure values
            else if (State == State.Configure)
            {
                // passButton is OFF by default
                marketDataPass = false;
            }

            // Load resources
            else if (State == State.Historical)
            {
                if (ChartControl != null)
                {
                    ChartControl.Dispatcher.InvokeAsync(() =>
                    {
                        CreateWPFControls();
                    });
                }
            }

            // Cleanup resources
            else if (State == State.Terminated)
            {
                if (ChartControl != null)
                {
                    ChartControl.Dispatcher.InvokeAsync(() =>
                    {
                        DisposeWPFControls();
                    });
                }
            }
            
            #endregion
        }

        // Creates resources in State.Historical
        protected void CreateWPFControls()
        {
            // Finds ChartTrader grid to modify
            chartWindow = Window.GetWindow(ChartControl.Parent) as Chart;
            if (chartWindow == null)
                return;
            chartTraderGrid = (chartWindow.FindFirst("ChartWindowChartTraderControl") as ChartTrader).Content as Grid;

            // Adds enough rows to chartTraderGrid to fit passButton
            if (chartTraderGrid.RowDefinitions.Count <= 7)
                chartTraderGrid.RowDefinitions.Add(new RowDefinition());

            #region Pass Button
            passButton = new Button
            {
                Style = Application.Current.TryFindResource("BasicEntryButton") as Style,
                Background = new SolidColorBrush(Colors.Black),
                Content = "Baton Pass"
            };

            passButton.Click += PassButton_Click;
            AutomationProperties.SetAutomationId(passButton, "PassButton");
            #endregion

            // Add passButton to new row at bottom of button grid
            Grid.SetRow(passButton, 8);
            chartTraderGrid.Children.Add(passButton);
        }

        // Cleans up resources in State.Terminated
        private void DisposeWPFControls()
        {
            if (chartTraderGrid != null || passButton != null)
                chartTraderGrid.Children.Remove(passButton);

            // Other resources to be removed?
        }

        // Function executed by passButton.Click
        private void PassButton_Click(object sender, RoutedEventArgs e)
        {
            passButton = sender as Button;
            if (passButton != null)
            {
                // Enables OnMarketData and OnMarketDepth methods
                marketDataPass = !marketDataPass;

                if (marketDataPass == true) Print("Baton Pass is streaming market data");
                else Print("Data stream ended");
            }
        }
        

        #region Data access declarations

        private bool marketDataPass;

        private List<PriceLadderRow> askRows = new List<PriceLadderRow>(10);
        private List<PriceLadderRow> bidRows = new List<PriceLadderRow>(10);

        #endregion

        // Object model of PriceLadder row
        public class PriceLadderRow
        {
            public double Price;
            public long Volume;
        }

        // Adds new market data to DB on each MarketDataEvent
        protected override void OnMarketData(MarketDataEventArgs e)
        {
            if (marketDataPass == true)
            {
                if (e.MarketDataType == MarketDataType.Last)
                {
                    DatabaseAccess.AddMarketData(e.Time, e.Instrument.ToString(), e.Price, e.Volume);
                }
            }
        }

        // Updates price ladders on each MarketDepthEvent
        protected override void OnMarketDepth(MarketDepthEventArgs e)
        {
            // Constructs PriceLadder synchronously
            lock (e.Instrument.SyncMarketDepth)
            {
                // Creates list to hold PriceLadder
                List<PriceLadderRow> rows = (e.MarketDataType == MarketDataType.Ask ? askRows : bidRows);
                PriceLadderRow row = new PriceLadderRow { Price = e.Price, Volume = e.Volume };

                // Add row
                if (e.Operation == Operation.Add || (e.Operation == Operation.Update && (rows.Count == 0 || rows.Count <= e.Position)))
                {
                    if (rows.Count <= e.Position) rows.Add(row);
                    else rows.Insert(e.Position, row);
                }

                // Remove row
                else if (e.Operation == Operation.Remove && rows.Count > e.Position)
                    rows.RemoveAt(e.Position);

                // Update row
                else if (e.Operation == Operation.Update)
                {
                    if (rows[e.Position] == null) 
                        rows[e.Position] = row;
                    else
                    {
                        rows[e.Position].Price = e.Price;
                        rows[e.Position].Volume = e.Volume;
                    }
                }
            }
            
            // Passes new ladders to DB
            if (marketDataPass == true)
            {
                for (int idx = 0; idx < askRows.Count; idx++)
                    DatabaseAccess.UpdateAskLadder(idx, askRows[idx].Price, askRows[idx].Volume);
                
                for (int idx = 0; idx < bidRows.Count; idx++)
                    DatabaseAccess.UpdateBidLadder(idx, bidRows[idx].Price, bidRows[idx].Volume);
            }
        }
    }
}


// Ignore the following:

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private BatonPass_AddOn[] cacheBatonPass_AddOn;
		public BatonPass_AddOn BatonPass_AddOn()
		{
			return BatonPass_AddOn(Input);
		}

		public BatonPass_AddOn BatonPass_AddOn(ISeries<double> input)
		{
			if (cacheBatonPass_AddOn != null)
				for (int idx = 0; idx < cacheBatonPass_AddOn.Length; idx++)
					if (cacheBatonPass_AddOn[idx] != null &&  cacheBatonPass_AddOn[idx].EqualsInput(input))
						return cacheBatonPass_AddOn[idx];
			return CacheIndicator<BatonPass_AddOn>(new BatonPass_AddOn(), input, ref cacheBatonPass_AddOn);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.BatonPass_AddOn BatonPass_AddOn()
		{
			return indicator.BatonPass_AddOn(Input);
		}

		public Indicators.BatonPass_AddOn BatonPass_AddOn(ISeries<double> input )
		{
			return indicator.BatonPass_AddOn(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.BatonPass_AddOn BatonPass_AddOn()
		{
			return indicator.BatonPass_AddOn(Input);
		}

		public Indicators.BatonPass_AddOn BatonPass_AddOn(ISeries<double> input )
		{
			return indicator.BatonPass_AddOn(input);
		}
	}
}

#endregion
