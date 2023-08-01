# baton-pass
 Plugin for NinjaTrader 8 to collect market data into local SQL database
 
### Setup:
1. Requires a SQLEXPRESS server instance.
2. Run the scripts from BatonPass_DB or make a publish.xml with VS to setup the database. <br>
Alternatively, create your own and change the connection strings in BatonPass_Dapper.cs. <br>
To modify and recompile BatonPass_Dapper, you will require .NET Framework 4.8.
3. Add BatonPass.cs and BatonPass_Dapper/ to the /bin/Custom/Indicators/ folder in your NinjaTrader 8 directory. 
4. Open a NinjaScript Editor window and use the Explorer to open BatonPass.cs. <br>
Right-click anywhere in the source code and select compile, then restart NT8 if needed.
5. Open a NinjaScript Output window and a trading chart. Add the BatonPass indicator to the chart and apply, then open the Chart Trader. <br>
You should see a large "BatonPass" button. Clicking the button starts or stops a data stream and prints its status to the output window. 
