# baton-pass
 Plugin for NinjaTrader 8 to collect market data into local SQL database
 
 Sorry, not sure where the indenting errors in BatonPass.cs are coming from. 

Setup:
1. Requires a SQLEXPRESS server instance. 
2. Run the scripts from BatonPass_DB or use the publish.xml in VS to setup the database. 
Alternatively, create your own and change the connection strings in BatonPass_Dapper.cs.
To modify and recompile BatonPass_Dapper, you will require .NET Framework 4.8.
4. Add BatonPass_AddOn.cs and \BatonPass_Dapper to the \bin\Custom\Indicators folder in your NinjaTrader 8 directory. 
5. Open a NinjaScript Editor window and use the Explorer to open to the BatonPass_AddOn file.
6. Right-click and select compile. Once compiled, open a NinjaScript Output window and a trading chart. 
7. Add the BatonPass indicator to the chart and apply, then open the Chart Trader. You should see a large "BatonPass" button. 
Selecting the button prints the status of the data stream to the output window. 
